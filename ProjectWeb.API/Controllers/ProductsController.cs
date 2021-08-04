using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.UnitOfWorks;
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
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] ProductPagingRequest request)
        {
            var result = await _unitOfWork.Products.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("byCategories")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] ProductByCategoryIdPagingRequest request)
        {
            var products = await _unitOfWork.Products.GetAllByCategoryId(request);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIDAsync(id);
            if (product == null)
                return BadRequest($"Cannot find product with ID: {id}");
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Products.CreateWithImages(request);
            if (result.Object == Guid.Empty)
                return BadRequest("Create Fail!!!");

            var product = await _unitOfWork.Products.GetByIDAsync(result.Object);

            return CreatedAtAction(nameof(GetById), new { ID = result.Object }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _unitOfWork.Products.UpdateWithImages(request);
            if (affectedResult == 0)
                return BadRequest("Create Fail!!!");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var messages = await _unitOfWork.Products.DeleteByIDAsync(id);
            if (messages == "Fail")
                return BadRequest("Fail");
            else
                return Ok();
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

            return CreatedAtAction(nameof(GetImageById), new { ID = result }, image);
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
            var messages = await _unitOfWork.Images.DeleteByIDAsync(imageID);
            if (messages == "Fail")
                return BadRequest("Fail");
            else
                return Ok();
        }



        [HttpGet("{productID}/images/{imageID}")]
        public async Task<IActionResult> GetImageById(Guid productID, Guid id)
        {
            var image = await _unitOfWork.Images.GetByIDAsync(id);
            if (image == null)
                return BadRequest($"Cannot find Images with ID: {id}");
            return Ok(image);
        }

    }
}
