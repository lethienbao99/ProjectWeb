﻿using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectWeb.Common.Exceptions;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Products
{
    public class ProductServices : Repository<Product>, IProductServices
    {
        private readonly ProjectWebDBContext _context;
        private readonly IStorageServices _storageServices;
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly ILogger<ProductServices> _logger;


        public ProductServices(ProjectWebDBContext context, IStorageServices storageServices, Lazy<IUnitOfWork> unitOfWork, ILogger<ProductServices> logger) : base(context)
        {
            _context = context;
            _storageServices = storageServices;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResultMessage<Guid>> CreateWithImages(ProductCreateRequest request)
        {
            var product = new Product()
            {
                ID = Guid.NewGuid(),
                Code = request.Code,
                ProductName = request.ProductName,
                Description = request.Description,
                Type = request.Type,
                Status = request.Status,
                PriceDollar = request.PriceDollar,
                Price = request.Price,
                Stock = request.Stock,
                Alias = request.Alias,
                DateCreated = DateTime.Now

            };
            if(request.ThumbnailImage != null)
            {
                product.Images = new List<Image>()
                {
                    new Image()
                    {
                        ID = Guid.NewGuid(),
                        Caption = "Thumbnail Image " + request.ProductName,
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true
                    }
                };
            }

            if(request.CategoryId != Guid.Empty && request.CategoryId != null)
            {
                var category = await _context.Categories.FindAsync(request.CategoryId);
                if(category != null)
                {
                    //Add category con đã chọn trên view 
                    var productCategorieChild = new ProductCategory()
                    {
                        ID = Guid.NewGuid(),
                        ProductID = product.ID,
                        CategoryID = request.CategoryId.Value,
                        DateCreated = DateTime.Now,
                    };
                    _unitOfWork.Value.ProductCategories.Insert(productCategorieChild);

                    //Tìm category cha và add vào luôn.
                    if(category.ParentID != null && category.ParentID != Guid.Empty)
                    {
                        var productCategorieParent = new ProductCategory()
                        {
                            ID = Guid.NewGuid(),
                            ProductID = product.ID,
                            CategoryID = category.ParentID.Value,
                            DateCreated = DateTime.Now,
                        };
                        _unitOfWork.Value.ProductCategories.Insert(productCategorieParent);
                    }
                }
                
            }
            _unitOfWork.Value.Products.Insert(product);
            await _unitOfWork.Value.CompleteAsync();
            return new ResultObjectSuccess<Guid>(product.ID);
        }


        public async Task<ResultMessage<int>> UpdateWithImages(ProductModel request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ID == request.ID && request.IsDelete == null);


            if (product == null)
                throw new ProjectWebException($"Can not find a product: {request.ID}");
            else
            {
                product.Code = request.Code;
                product.ProductName = request.ProductName;
                product.Description = request.Description;
                product.Type = request.Type;
                product.Status = request.Status;
                product.Price = request.Price;
                product.Stock = request.Stock;
                product.Alias = request.Alias;
                product.DateUpdated = DateTime.Now;
            }

            //Imgae.
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.Images.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductID == request.ID);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.Images.Update(thumbnailImage);
                }
            }

            var affectedResult = await _context.SaveChangesAsync();
            return new ResultObjectSuccess<int>(affectedResult);

        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageServices.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<PageResultModel<ProductModel>> GetAllByCategoryId(ProductByCategoryIdPagingRequest request)
        {
            var query = from p in _context.Products
                        join pc in _context.ProductCategories on p.ID equals pc.ProductID
                        where pc.IsDelete == null
                        join c in _context.Categories on pc.CategoryID equals c.ID
                        where c.IsDelete == null
                        where p.IsDelete == null
                        select new { p, pc, c };
            //Filter.
            if (request.CategoryId != Guid.Empty)
            {
                query = query.Where(x => x.c.ID == request.CategoryId);
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                 .Select(x => new ProductModel()
                 {
                     ID = x.p.ID,
                     ProductName = x.p.ProductName,
                     Code = x.p.Code,
                     Description = x.p.Description,
                     Type = x.p.Type,
                     Status = x.p.Status,
                     Price = x.p.Price,
                     Stock = x.p.Stock,
                     Alias = x.p.Alias,
                     DateCreated = DateTime.Now,
                 }).Distinct().OrderByDescending(x => x.Sort).ToListAsync();

            var pagedResult = new PageResultModel<ProductModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return pagedResult;
        }
        public async Task<ResultMessage<PageResultModel<ProductViewModel>>> GetAllPaging(ProductPagingRequest request)
        {
            //Ko xài left join như dưới vì khi thêm nhiều category vào product sẽ bị double product theo category đó.
            /*var query = from p in _context.Products
                        join pc in _context.ProductCategories on p.ID equals pc.ProductID into ppc
                        from pc in ppc.DefaultIfEmpty()
                        join c in _context.Categories on pc.CategoryID equals c.ID into cc
                        from c in cc.DefaultIfEmpty()
                        select new { p, c, pc };*/

            try
            {
                var query = from p in _context.Products
                            join i in _context.Images.Where(x => x.IsDefault == true) on p.ID equals i.ProductID into pi
                            from i in pi.DefaultIfEmpty()

                                //Tách riêng categories ra để lấy mảng hoặc join thành chuỗi.
                            let categories = (from pc in _context.ProductCategories
                                              join c in _context.Categories on pc.CategoryID equals c.ID
                                              where p.ID == pc.ProductID && pc.IsDelete == null && c.IsDelete == null
                                              select c.CategoryName).ToList()
                            //Phần này giành cho tìm kiếm.
                            let categorieIDs = (from pc in _context.ProductCategories
                                                join c in _context.Categories on pc.CategoryID equals c.ID
                                                where p.ID == pc.ProductID && pc.IsDelete == null && c.IsDelete == null
                                                select c.ID).ToList()

                            select new { p, categories, categorieIDs, i };


                //List Categories


                //Filter.
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.p.ProductName.Contains(request.Keyword) || x.p.Description.Contains(request.Keyword));
                }

                if (request.CategoryId != Guid.Empty && request.CategoryId != null)
                {
                    query = query.Where(x => x.categorieIDs.Contains(request.CategoryId.Value));
                }

                int totalRow = await query.CountAsync();

                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                     .Select(x => new ProductViewModel()
                     {
                         ID = x.p.ID,
                         ProductName = x.p.ProductName,
                         Code = x.p.Code,
                         Description = x.p.Description,
                         Type = x.p.Type,
                         Status = x.p.Status,
                         Price = x.p.Price,
                         PriceDollar = x.p.PriceDollar,
                         PriceFormat = x.p.Price.ToString("#,##0"),
                         PriceDollarFormat = x.p.PriceDollar.ToString("#,##0"),
                         Stock = x.p.Stock,
                         Alias = x.p.Alias,
                         Sort = x.p.Sort,
                         Views = x.p.Views,
                         DateCreated = DateTime.Now,
                         Categories = x.categories, // Mảng categories
                     CategoriesJoin = string.Join(",", x.categories), // Chuỗi categories 
                     ImgDefaultPath = x.i.ImagePath,
                     }).OrderByDescending(x => x.Sort).ToListAsync();

                var pagedResult = new PageResultModel<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };

                return new ResultObjectSuccess<PageResultModel<ProductViewModel>>(pagedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new ResultObjectError<PageResultModel<ProductViewModel>>(e.Message);
            }

        }

        public async Task<ResultMessage<ProductViewModel>> GetProductByID(Guid ID)
        {

            var product = from p in _context.Products
                          join i in _context.Images.Where(x => x.IsDefault == true) on p.ID equals i.ProductID into pi
                          from i in pi.DefaultIfEmpty()
                          where p.ID == ID
                          select new { p, i };

            if (product != null)
            {
                var query = await (from c in _context.Categories
                                   join pc in _context.ProductCategories on c.ID equals pc.CategoryID
                                   where pc.IsDelete == null
                                   where c.IsDelete == null && pc.ProductID == ID
                                   select c.CategoryName).ToListAsync();

                var CategoryIDChildNode = await (from c in _context.Categories
                                   join pc in _context.ProductCategories on c.ID equals pc.CategoryID
                                   where c.ParentID != null && pc.ProductID == ID
                                   select c.ID).FirstOrDefaultAsync();

                var CategoryNameChildNode = await (from c in _context.Categories
                                                 join pc in _context.ProductCategories on c.ID equals pc.CategoryID
                                                 where c.ParentID != null && pc.ProductID == ID
                                                 select c.CategoryName).FirstOrDefaultAsync();

                var data = await product.Select(x => new ProductViewModel()
                {
                    ID = x.p.ID,
                    ProductName = x.p.ProductName,
                    Code = x.p.Code,
                    Description = x.p.Description,
                    Type = x.p.Type,
                    Status = x.p.Status,
                    PriceFormat = x.p.Price.ToString("#,##0"),
                    PriceDollarFormat = x.p.PriceDollar.ToString("#,##0"),
                    Stock = x.p.Stock,
                    Alias = x.p.Alias,
                    Price = x.p.Price,
                    PriceDollar = x.p.PriceDollar,
                    DateCreated = x.p.DateCreated,
                    DateUpdated = x.p.DateUpdated,
                    DateDeleted = x.p.DateDeleted,
                    Categories = query,
                    CategoryId = CategoryIDChildNode,
                    CategoryName = CategoryNameChildNode,
                    ImgDefaultPath = x.i.ImagePath
                }
                ).FirstOrDefaultAsync();

                return new ResultObjectSuccess<ProductViewModel>(data);
            }

            return new ResultObjectSuccess<ProductViewModel>(null); 
        }

        public async Task<int> UpdateViewCount(Guid ID)
        {
            var product = await _context.Products.FindAsync(ID);
            if (product != null)
                product.Views += 1;

            return await _context.SaveChangesAsync();
        }

        public async Task<ResultMessage<List<ProductViewModel>>> GetSlideProducts()
        {
            var query = from p in _context.Products
                        join i in _context.Images.Where(x => x.IsDefault == true) on p.ID equals i.ProductID into pi
                        from i in pi.DefaultIfEmpty()
                        select new { p, i };

            var data = await query.Take(8)
                 .Select(x => new ProductViewModel()
                 {
                     ID = x.p.ID,
                     ProductName = x.p.ProductName,
                     Description = x.p.Description,
                     Status = x.p.Status,
                     PriceFormat = x.p.Price.ToString("#,##0"),
                     PriceDollarFormat = x.p.PriceDollar.ToString("#,##0"),
                     Stock = x.p.Stock,
                     Views = x.p.Views,
                     DateCreated = DateTime.Now,
                     ImgDefaultPath = x.i.ImagePath,
                 }).OrderByDescending(x => x.Views).ToListAsync();

            if(data != null)
                return new ResultObjectSuccess<List<ProductViewModel>>(data);

            return new ResultObjectError<List<ProductViewModel>>();
        }

        public ResultMessage<ProductViewModel> GetProductByIDUsingStored(Guid ID)
        {
            var cmn = (SqlConnection)_context.Database.GetDbConnection();
            if (cmn.State == ConnectionState.Closed)
                cmn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                var cmt = cmn.CreateCommand();
                cmt.CommandText = "Sp_GetProductByID";
                cmt.CommandType = CommandType.StoredProcedure;
                cmt.Parameters.AddWithValue("@id", ID);

                da.SelectCommand = cmt;
                da.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var result = new ProductViewModel
                    {
                        ID = (Guid)ds.Tables[0].Rows[0]["ID"],
                        ProductName = (string)ds.Tables[0].Rows[0]["ProductName"]
                    };
                    return new ResultObjectSuccess<ProductViewModel>(result);
                }
                return new ResultObjectError<ProductViewModel>("Fail");
            }
            catch (Exception e)
            {
                return new ResultObjectError<ProductViewModel>(e.Message);
            }
        }

        public ResultMessage<PageResultModel<ProductViewModel>> GetAllPagingUsingStored(ProductPagingRequest request)
        {
            List<ProductViewModel> result = new List<ProductViewModel>();
            var cmn = (SqlConnection)_context.Database.GetDbConnection();
            if (cmn.State == ConnectionState.Closed)
                cmn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                var cmt = cmn.CreateCommand();
                cmt.CommandText = "Sp_GetListProducts";
                cmt.CommandType = CommandType.StoredProcedure;
                cmt.Parameters.AddWithValue("@keyword", request.Keyword);
                cmt.Parameters.AddWithValue("@categoryID", request.CategoryId);
                cmt.Parameters.AddWithValue("@pageIndex", request.PageIndex);
                cmt.Parameters.AddWithValue("@pageSize", request.PageSize);

                da.SelectCommand = cmt;
                da.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var product = new ProductViewModel()
                        {
                            ID = (Guid)row["ID"],
                            ProductName = (string)row["ProductName"],
                            Code = (string)row["Code"],
                            Description = (string)row["Description"],
                            Price = (double)row["Price"],
                            PriceDollar = (double)row["PriceDollar"],
                            Stock = (int)row["Stock"],
                            Sort = (int)row["Sort"],
                            Views = (int)row["Views"],
                            DateCreated = DateTime.Now,
                            CategoriesJoin = (string)row["Categories"], // Chuỗi categories 
                            ImgDefaultPath = (string)row["ImagePath"],

                        };
                        result.Add(product);
                    }

                }
                var pagedResult = new PageResultModel<ProductViewModel>()
                {
                    TotalRecords = result.Count,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = result != null ? result : null
                };
                return new ResultObjectSuccess<PageResultModel<ProductViewModel>>(pagedResult);
            }
            catch (Exception e)
            {
                return new ResultObjectError<PageResultModel<ProductViewModel>>(e.Message);
            }
        }
    }
}
