using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("forCreateOrUpdate")]
        public async Task<IActionResult> GetAllByCreateOrUpdate(bool isParent)
        {
            var categories = await _unitOfWork.Categories.GetAllByCreateOrUpdate(isParent);
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _unitOfWork.Categories.GetByIDAsync(id);
            if (product.Object == null)
                return BadRequest($"Cannot find category with ID: {id}");
            return Ok(product);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] CategoryPagingRequest request)
        {
            var result = await _unitOfWork.Categories.GetAllPaging(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateOrUpdateRequest request)
        {
            var result = await _unitOfWork.Categories.Create(request);
            if(result.IsSuccessed)
                return Ok(result);
            return BadRequest("Create Fail!!!");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryCreateOrUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.ID = id;
            var result = await _unitOfWork.Categories.Update(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest("Create Fail!!!");
           
        }

    }
}
