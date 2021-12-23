using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectWeb.API.Controllers;
using ProjectWeb.Bussiness.Services.Products;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductApi.Moq
{
    public class ProductController_UnitTest
    {
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly ProductsController _controller;


        public ProductController_UnitTest()
        {
            _mockRepo = new Mock<IUnitOfWork>();
            _controller = new ProductsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllProduct_ActionExecutes_ReturnsData()
        {
            IEnumerable<Product> listProduct = new List<Product>()
            {
                new Product()
                {
                    ID = Guid.NewGuid(),
                    ProductName = "Test",
                    Code = "Test",
                    DateCreated = DateTime.Now,
                    Description = "Test",
                    Alias = "Test",
                    Price = 1,
                    PriceDollar = 1
                },
                new Product()
                {
                    ID = Guid.NewGuid(),
                    ProductName = "Test",
                    Code = "Test",
                    DateCreated = DateTime.Now,
                    Description = "Test",
                    Alias = "Test",
                    Price = 1,
                    PriceDollar = 1
                },
            };

            var resultObject = new ResultObjectSuccess<IEnumerable<Product>>(listProduct)
            {
                IsSuccessed = true,
                Message = "Success",
                Object = listProduct
            };

            _mockRepo.Setup(repo => repo.Products.GetAllAsync())
                .ReturnsAsync(resultObject);

            var result = await _controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var employees = Assert.IsType<ResultObjectSuccess<IEnumerable<Product>>>(okResult.Value);
            Assert.Equal(listProduct.Count(), employees.Object.Count());
        }


        [Fact]
        public async Task GetProductByID_ActionExecutes_ReturnsData()
        {
            var productId = Guid.NewGuid();
            var productDto = new ProductViewModel()
            {
                ID = productId,
                ProductName = "Test",
                Code = "Test",
                Description = "Test",
                Type = "Test",
                Status = "Test",
                PriceFormat = 10.ToString("#,##0"),
                PriceDollarFormat = 10.ToString("#,##0"),
                Stock = 10,
                Alias = "test",
                Price = 10,
                PriceDollar = 10,
                DateCreated = DateTime.Today
            };

            var resultObject = new ResultObjectSuccess<ProductViewModel>(productDto)
            {
                IsSuccessed = true,
                Message = "Success",
                Object = productDto
            };

            _mockRepo.Setup(repo => repo.Products.GetProductByID(productId))
                .ReturnsAsync(resultObject);

            var result = await _controller.GetByIdCustome(productId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productReturn = Assert.IsType<ResultObjectSuccess<ProductViewModel>>(okResult.Value);
            Assert.Equal(productId, productReturn.Object.ID);
        }


    }
}