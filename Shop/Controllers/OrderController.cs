using Microsoft.AspNetCore.Mvc;
using Shop.ViewModels;
using Shop.Models;
using Shop.Services;
using Shop.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Shop.Controllers
{
    public class OrderController : Controller
    {
        private readonly DatabaseService _databaseService;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        public OrderController(DatabaseService databaseService, AppDbContext context, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _databaseService = databaseService;
            _context = context;
            _emailSender = emailSender;
            _env = env;
        }
        public IActionResult Index()
        {
            string cartToken = Request.Cookies["cartToken"];
            var cartProducts = _context.CartProducts.Where(x => x.CartToken == cartToken).ToList();
            if (cartProducts.Count == 0)
            {
                return Redirect("/Cart");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index([Bind("FirstName, LastName, City, Address, PostCode, Email, PhoneNumber, PaymentMethod, DeliveryMethod")] OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                string? cartToken = Request.Cookies["cartToken"];
                var cartProducts = _databaseService.GetUserCartProducts(cartToken);
                float rawOrderPrice = 0;
                foreach (var item in _databaseService.GetUserCartProducts(cartToken))
                {
                    rawOrderPrice += item.Price * item.Quantity;
                    _context.CartProducts.Remove(item);
                }
                var order = new Order
                {
                    FirstName = orderViewModel.FirstName,
                    LastName = orderViewModel.LastName,
                    City = orderViewModel.City,
                    Address = orderViewModel.Address,
                    PostCode = orderViewModel.PostCode,
                    Email = orderViewModel.Email,
                    PhoneNumber = orderViewModel.PhoneNumber,
                    DeliveryMethod = orderViewModel.DeliveryMethod,
                    PaymentMethod = orderViewModel.PaymentMethod,
                    Created = DateTime.Now,
                    RawPrice = rawOrderPrice,
                    OrderPaid = false,
                    OrderToken = Guid.NewGuid().ToString()
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                foreach(var cartProduct in cartProducts)
                {
                    var orderedProduct = new OrderedProduct
                    {
                        Name = cartProduct.Name,
                        Price = cartProduct.Price,
                        Amount = cartProduct.Quantity,
                        OrderId = order.Id,
                        ProductId = cartProduct.ProductId
                    };
                    var product = _databaseService.GetProduct(orderedProduct.ProductId);
                    product.Stock -= orderedProduct.Amount;
                    string categoryName = _context.Categories.FirstOrDefault(x => x.Id == product.ProductCategoryId).Name;
                    switch (categoryName)
                    {
                        //Monitors
                        case "Monitors":
                            var monitor = _databaseService.GetMonitorById(product.ProductNativeId);
                            monitor.Stock = product.Stock;
                            break;
                        //Smartphones
                        case "Smartphones":
                            var smartphone = _context.Smartphones.FirstOrDefault(x => x.Id == product.ProductNativeId);
                            smartphone.Stock = product.Stock;
                            break;
                        default:
                            break;
                    }
                    _context.OrderedProducts.Add(orderedProduct);
                }
                await _context.SaveChangesAsync();
                var orderedProducts = _context.OrderedProducts.Where(x => x.OrderId == order.Id).ToList();
                float? deliveryPrice = _context.DeliveryMethods.FirstOrDefault(x => x.Name == order.DeliveryMethod)?.Price;
                float? paymentPrice = _context.PaymentMethods.FirstOrDefault(x => x.Name == order.PaymentMethod)?.Price;
                order.DeliveryPrice = deliveryPrice;
                order.PaymentPrice = paymentPrice;
                order.TotalPrice = order.RawPrice + deliveryPrice + paymentPrice;
                var url = Url.Action("Payment", "Order", new { orderToken = order.OrderToken }, protocol: Request.Scheme);
                await _context.SaveChangesAsync();
                string orderedProductsHTML = "";
                foreach(var orderedProduct in orderedProducts)
                {
                    var product = _databaseService.GetProduct(orderedProduct.ProductId);
                    var categoryName = _context.Categories.FirstOrDefault(x => x.Id == product.ProductCategoryId).Name;
                    orderedProductsHTML += 
                        $"<span><span style='margin-top: -10px; font-size: 17px; font-weight: bold;'>{orderedProduct.Name}</span><span style='font-size: 15px; color: grey;'> - {categoryName}</span></span><br/>" +
                        $"<strong>Price: </strong><span>{orderedProduct.Price}$</span><br/>" +
                        $"<strong>Amount: </strong><span>{orderedProduct.Amount}</span>" +
                        $"<hr/>";
                }
                orderedProductsHTML += $"<h3>Delivery method: {order.DeliveryMethod} - {order.DeliveryPrice}$</h3>" +
                    $"<h3>Payment method: {order.PaymentMethod} - {order.PaymentPrice}$</h3>" +
                    $"<hr/>";
                orderedProductsHTML += $"<h2>Total sum: {order.TotalPrice}$</h2>";
                await _emailSender.SendEmailAsync(order.Email, $"Order nr {order.Id} - pay for the order", $"Please pay for your order nr {order.Id} ->> <a href='{url}'>Go to payment page</a>" +
                    $"<h2>Ordered products:</h2>" + orderedProductsHTML);
                return RedirectToAction("Payment", new { orderToken = order.OrderToken });
            }
            return View(orderViewModel);
        }
        public IActionResult Payment(string orderToken)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderToken == orderToken);
            return View(order);
        }
        public async Task<IActionResult> Pay(string orderToken)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderToken == orderToken);
            if (order.OrderPaid)
            {
                return RedirectToAction("Payment", new { orderToken = orderToken });
            }
            if(order == null)
            {
                return NotFound();
            }
            order.OrderPaid = true;
            await _context.SaveChangesAsync();
            await _emailSender.SendEmailAsync(order.Email, $"Order nr {order.Id} - successful payment", "<h1>Thank you for purchase on my page!<h1/>" +
                $"<h1>Your order nr {order.Id} has been paid!<h1/>");
            return RedirectToAction("SuccessfulPayment", new { orderToken = orderToken });
        }
        public IActionResult SuccessfulPayment(string orderToken)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderToken == orderToken);
            return View(order);
        }
    }
}
