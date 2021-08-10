using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Common.Enums;
using ProjectWeb.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductBackendAPI _productBackendAPI;
        private readonly IConfiguration _config;
        public CartController(IProductBackendAPI productBackendAP, IConfiguration config)
        {
            _config = config;
            _productBackendAPI = productBackendAP;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetListCartItems()
        {
            List<CartViewModel> carts = new List<CartViewModel>();
            var currentCartInSession = HttpContext.Session.GetString(EnumConstants.SystemsConstants.CartSession);
            if (currentCartInSession != null)
                carts = JsonConvert.DeserializeObject<List<CartViewModel>>(currentCartInSession);
            return Ok(carts);
        }


        public async Task<IActionResult> AddToCart(Guid id)
        {
            List<CartViewModel> carts = new List<CartViewModel>();
            int quatity = 1;
            var currentCartInSession = HttpContext.Session.GetString(EnumConstants.SystemsConstants.CartSession);
            var result = await _productBackendAPI.GetProductByIDCustome(id);
            var product = result.Object;
            if (currentCartInSession != null)
            {
                carts = JsonConvert.DeserializeObject<List<CartViewModel>>(currentCartInSession);
                if (carts.Any(x => x.ProductID == id))
                {

                    var updateCart = carts.FirstOrDefault(x => x.ProductID == id);
                    quatity = carts.First(x => x.ProductID == id).Quantity + 1;
                    updateCart.Quantity = quatity;
                }
                else
                {
                    var cart = new CartViewModel()
                    {
                        ProductID = id,
                        ProductName = product.ProductName,
                        ImagePath = product.ImgDefaultPath,
                        Description = product.Description,
                        Quantity = quatity,
                        Price = product.Price,
                        PriceDollar = product.PriceDollar,
                    };
                    carts.Add(cart);
                }
            }
            else
            {
                var cart = new CartViewModel()
                {
                    ProductID = id,
                    ProductName = product.ProductName,
                    ImagePath = product.ImgDefaultPath,
                    Description = product.Description,
                    Quantity = quatity,
                    Price = product.Price,
                    PriceDollar = product.PriceDollar,
                };
                carts.Add(cart);
            }

            HttpContext.Session.SetString(EnumConstants.SystemsConstants.CartSession, JsonConvert.SerializeObject(carts));
        
            return Ok(carts);
        }

        public IActionResult UpdateCart(Guid id, string status)
        {
            List<CartViewModel> carts = new List<CartViewModel>();
            int quatity = 1;
            var currentCartInSession = HttpContext.Session.GetString(EnumConstants.SystemsConstants.CartSession);
            if (currentCartInSession != null)
            {
                carts = JsonConvert.DeserializeObject<List<CartViewModel>>(currentCartInSession);
                if(status == "minus")
                {
                    if (carts.Any(x => x.ProductID == id))
                    {

                        var updateCart = carts.FirstOrDefault(x => x.ProductID == id);
                        quatity = carts.First(x => x.ProductID == id).Quantity - 1;
                        updateCart.Quantity = quatity;
                        if(updateCart.Quantity == 0)
                            carts.Remove(updateCart);
                    }
                }
                else if (status == "plus")
                {
                    if (carts.Any(x => x.ProductID == id))
                    {

                        var updateCart = carts.FirstOrDefault(x => x.ProductID == id);
                        quatity = carts.First(x => x.ProductID == id).Quantity + 1;
                        updateCart.Quantity = quatity;
                    }
                }
                else if (status == "delete")
                {
                    if (carts.Any(x => x.ProductID == id))
                    {
                        var item = carts.FirstOrDefault(x => x.ProductID == id);
                        carts.Remove(item);
                    }

                }
            }

            HttpContext.Session.SetString(EnumConstants.SystemsConstants.CartSession, JsonConvert.SerializeObject(carts));

            return Ok(carts);
        }


    }
}
