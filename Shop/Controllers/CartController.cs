using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Services;
using System.Security.Claims;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        private AppDbContext _context;
        private DatabaseService _databaseService;
        private IServiceProvider _serviceProvider;
        public CartController(AppDbContext context, DatabaseService databaseService, IServiceProvider serviceProvider)
        {
            _context = context;
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;
        }
        public IActionResult Index()
        {
            string cartToken = Request.Cookies["cartToken"];
            var cartItems = _context.CartProducts.Where(x => x.CartToken == cartToken).ToList();
            
            return View(cartItems);
        }
        public RedirectResult AddToCart(int id, int amount)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if(User.Identity.IsAuthenticated && product?.Creator == User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return Redirect("/User/AccessDenied");
            }
            if(product.Stock == 0)
            {
                return Redirect("/Cart");
            }
            string cartId = Request.Cookies["cartToken"] ?? Guid.NewGuid().ToString();
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("cartToken", cartId, options);
            var cartItems = _context.CartProducts.ToList();
            CartProduct cartProduct = new CartProduct
            {
                CartToken = cartId,
                Name = product.ProductName,
                Photo = product.ProductPhoto,
                Price = product.ProductPrice,
                Quantity = amount <= 0 ? amount = 1 : amount,
                ProductId = product.Id
            };
            if(cartProduct.Quantity > product.Stock)
            {
                cartProduct.Quantity = product.Stock;
            }
            _context.CartProducts.Add(cartProduct);
            foreach(var item in cartItems)
            {
                if(item.ProductId == cartProduct.ProductId && item.CartToken == cartId)
                {
                    item.Quantity += cartProduct.Quantity;
                    _context.CartProducts.Remove(cartProduct);
                }                
            }
            _context.SaveChanges();
            string previousUrl = Request.Cookies["previousUrl"];
            if(previousUrl == null)
            {
                return Redirect("/" + _context.Categories.FirstOrDefault(x => x.Id == product.ProductCategoryId).Name);
            }
            return Redirect(previousUrl);
        }
        public RedirectResult Delete(int id)
        {
            var cartProduct = _databaseService.GetCartProduct(id);
            string previousUrl = Request.Cookies["previousUrl"];
            _context.CartProducts.Remove(cartProduct);
            _context.SaveChanges();
            return Redirect(previousUrl);
        }
        //Add product in cart(+)
        public RedirectResult AddCartProduct(int id)
        {
            var cartProduct = _databaseService.GetCartProduct(id);
            var product = _databaseService.GetProduct(cartProduct.ProductId);
            if(cartProduct.Quantity < product.Stock)
                cartProduct.Quantity += 1;                   
            _context.SaveChanges();
            var previousUrl = Request.Cookies["previousUrl"];
            return Redirect(previousUrl);
        }
        //Substract product in cart(-)
        public RedirectResult SubstractCartProduct(int id)
        {
            var cartProduct = _databaseService.GetCartProduct(id);
            if (cartProduct.Quantity > 1)
                cartProduct.Quantity -= 1;            
            _context.SaveChanges();
            var previousUrl = Request.Cookies["previousUrl"];
            return Redirect(previousUrl);
        }
    }
}
