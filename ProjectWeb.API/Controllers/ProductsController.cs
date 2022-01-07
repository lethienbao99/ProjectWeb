using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectWeb.Bussiness.Caches;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [Cache(600)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("test log");
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("paging")]
        [Cache(600)]
        public async Task<IActionResult> GetAllPaging([FromQuery] ProductPagingRequest request)
        {
            var result = await _unitOfWork.Products.GetAllPaging(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("pagingV2")]
        [Cache(600)]
        public IActionResult GetAllPagingV2([FromQuery] ProductPagingRequest request)
        {
            var result = _unitOfWork.Products.GetAllPagingUsingStored(request);
            return Ok(result);
        }

        [HttpGet("byCategories")]
        [Cache(600)]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] ProductByCategoryIdPagingRequest request)
        {
            var products = await _unitOfWork.Products.GetAllByCategoryId(request);
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [Cache(600)]
        public async Task<IActionResult> GetById(Guid id)
        {
            //Add View Count
            //await _unitOfWork.Products.UpdateViewCount(id);

            var product = await _unitOfWork.Products.GetByIDAsync(id);
            if (product.Object == null)
                return BadRequest($"Cannot find product with ID: {id}");
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("stored/{id}")]
        [Cache(600)]
        public IActionResult GetByIdUsingStored(Guid id)
        {
            //Add View Count
            //await _unitOfWork.Products.UpdateViewCount(id);

            var product = _unitOfWork.Products.GetProductByIDUsingStored(id);
            if (product.Object == null)
                return BadRequest($"Cannot find product with ID: {id}");
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("{id}/v2")]
        [Cache(600)]
        public async Task<IActionResult> GetByIdCustome(Guid id)
        {
            //Add View Count
            await _unitOfWork.Products.UpdateViewCount(id);

            var product = await _unitOfWork.Products.GetProductByID(id);
            if (product.Object == null)
                return BadRequest($"Cannot find product with ID: {id}");
            return Ok(product);
        }



        [AllowAnonymous]
        [HttpPost("V2")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateV2([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Products.CreateWithImages(request);
            if (result.Object == Guid.Empty)
                return BadRequest("Create Fail!!!");

            var product = await _unitOfWork.Products.GetByIDAsync(result.Object);

            return CreatedAtAction(nameof(GetById), new { ID = result.Object }, product.Object);
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = new Product()
            {
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
            _unitOfWork.Products.Insert(product);

            if (request.ThumbnailImage != null)
            {

                var image = new Image()
                {
                    ProductID = product.ID,
                    Caption = "Thumbnail Image " + request.ProductName,
                    DateCreated = DateTime.Now,
                    FileSize = request.ThumbnailImage.Length,
                    ImagePath = await _unitOfWork.Products.SaveFile(request.ThumbnailImage),
                    IsDefault = true
                };
                _unitOfWork.Images.Insert(image);

            }

            if (request.CategoryId != Guid.Empty && request.CategoryId != null)
            {
                var category = await _unitOfWork.Categories.GetByIDAsync(request.CategoryId.Value);
                if (category.Object != null)
                {
                    //Add category con đã chọn trên view 
                    var productCategorieChild = new ProductCategory()
                    {
                        ProductID = product.ID,
                        CategoryID = request.CategoryId.Value,
                        DateCreated = DateTime.Now,
                    };
                    _unitOfWork.ProductCategories.Insert(productCategorieChild);

                    //Tìm category cha và add vào luôn.
                    if (category.Object.ParentID != null && category.Object.ParentID != Guid.Empty)
                    {
                        var productCategorieParent = new ProductCategory()
                        {
                            ProductID = product.ID,
                            CategoryID = category.Object.ParentID.Value,
                            DateCreated = DateTime.Now,
                        };
                        _unitOfWork.ProductCategories.Insert(productCategorieParent);
                  
                    }
                  
                }

            }
           
            if (_unitOfWork.Complete() == 0)
            {
                return BadRequest("Create Fail!!!");
            }
                
            var productById = await _unitOfWork.Products.GetByIDAsync(product.ID);

            return CreatedAtAction(nameof(GetById), new { ID = product.ID }, productById.Object);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute]Guid id,[FromForm] ProductModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.ID = id;
            var affectedResult = await _unitOfWork.Products.UpdateWithImages(request);
            if (affectedResult.Object == 0)
                return BadRequest("Create Fail!!!");
            return Ok(affectedResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _unitOfWork.Products.DeleteByIDAsync(id);
            return Ok(result);
        }

        [HttpPost("hideRecord/{id}")]
        public async Task<IActionResult> HideRecord(Guid id)
        {
            var messages = await _unitOfWork.Products.DeleteNotSQLByIDAsync(id);
            if (messages == "Fail")
                return BadRequest("Fail");
            else
                return Ok();
        }


        //Images
        [HttpPost("{productID}/images")]
        public async Task<IActionResult> CreateImage(Guid productID, [FromForm] ImageModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Images.AddImages(productID, request);
            if (result == Guid.Empty)
                return BadRequest("Create Fail!!!");

            var image = await _unitOfWork.Images.GetByIDAsync(result);

            return CreatedAtAction(nameof(GetImageById), new { ID = result }, image.Object);
        }

        [HttpPut("{productID}/images/{imageID}")]
        public async Task<IActionResult> UpdateImage(Guid imageID, [FromForm] ImageModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Images.UpdateImage(imageID, request);
            if (result == 0)
                return BadRequest("Create Fail!!!");


            return Ok();
        }

        [HttpDelete("{productID}/images/{imageID}")]
        public async Task<IActionResult> RemoveImage(Guid imageID)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Images.DeleteByIDAsync(imageID);
            return Ok(result);
        }


        [HttpGet("{productID}/images/{imageID}")]
        public async Task<IActionResult> GetImageById(Guid productID, Guid id)
        {
            var image = await _unitOfWork.Images.GetByIDAsync(id);
            if (image.Object == null)
                return BadRequest($"Cannot find Images with ID: {id}");
            return Ok(image);
        }

        [AllowAnonymous]
        [HttpGet("slide")]
        public async Task<IActionResult> GetSlideProducts()
        {
            var products = await _unitOfWork.Products.GetSlideProducts();
            return Ok(products);
        }

    }
}
