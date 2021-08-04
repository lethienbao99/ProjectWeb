using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Common.Exceptions;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
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
        public ProductServices(ProjectWebDBContext context, IStorageServices storageServices) : base(context)
        {
            _context = context;
            _storageServices = storageServices;
        }

        public async Task<Guid> CreateWithImages(ProductCreateRequest request)
        {
            var product = new Product()
            {
                ID = Guid.NewGuid(),
                Code = request.Code,
                ProductName = request.ProductName,
                Description = request.Description,
                Type = request.Type,
                Status = request.Status,
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
                        Caption = "Thumbnail Image " + request.ProductName,
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.ID;
        }


        public async Task<int> UpdateWithImages(ProductModel request)
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

            return await _context.SaveChangesAsync();


        }

        private async Task<string> SaveFile(IFormFile file)
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

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).OrderBy(x => x.p.Sort)
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
                 }).Distinct().ToListAsync();

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
            var query = from p in _context.Products
                        where p.IsDelete == null

                        //List Categories
                        let categories = (from pc in _context.ProductCategories
                                          join c in _context.Categories on pc.CategoryID equals c.ID
                                          where p.ID == pc.ProductID && pc.IsDelete == null && c.IsDelete == null
                                          select c.CategoryName).ToList()

                        select new { p, categories };

            //Filter.
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.ProductName.Contains(request.Keyword));
            }

          /*if (request.CategoryIds != null)
            {
                if (request.CategoryIds.Count > 0)
                {
                    query = query.Where(x => request.CategoryIds.Contains(x.categories));
                }
            }*/


            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).OrderBy(x => x.p.Sort)
                 .Select(x => new ProductViewModel()
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
                     Sort = x.p.Sort,
                     DateCreated = DateTime.Now,
                     Categories = x.categories
                     
                 }).ToListAsync();

            var pagedResult = new PageResultModel<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return new ResultObjectSuccess<PageResultModel<ProductViewModel>>(pagedResult);
        }

        public async Task<ProductViewModel> GetProductByID(Guid ID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.ID == ID && s.IsDelete == null);
            if(product != null)
            {
                var query = await (from c in _context.Categories
                                   join pc in _context.ProductCategories on c.ID equals pc.CategoryID
                                   where pc.IsDelete == null
                                   where c.IsDelete == null && pc.ProductID == ID
                                   select c.CategoryName).ToListAsync();

                var data =  new ProductViewModel()
                {
                    ID = product.ID,
                    ProductName = product.ProductName,
                    Code = product.Code,
                    Description = product.Description,
                    Type = product.Type,
                    Status = product.Status,
                    Price = product.Price,
                    Stock = product.Stock,
                    Alias = product.Alias,
                    DateCreated = product.DateCreated,
                    DateUpdated = product.DateUpdated,
                    DateDeleted = product.DateDeleted,
                    Categories = query,
                };
                return data;
            }

            return null;
        }

    }
}
