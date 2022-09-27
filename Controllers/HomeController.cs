using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CacheSqlMVC.Models;
using Microsoft.Extensions.Caching.Memory;


namespace CacheSqlMVC.Controllers;

public class HomeController : Controller
{

    List<Product> products = new List<Product>()
    {
        new Product(){ProductID=1, ProductName="Chai", SupplierID=1, CategoryID=1,QuantityPerUnit="10 boxes x 20 bags",UnitPrice=18, UnitsInStock=30, Discontinued=0, ReorderLevel=10},
        new Product(){ProductID=1, ProductName="Tea", SupplierID=2, CategoryID=1,QuantityPerUnit="20 boxes x 20 bags",UnitPrice=18, UnitsInStock=30, Discontinued=0, ReorderLevel=10},
        new Product(){ProductID=1, ProductName="Coffee", SupplierID=1, CategoryID=1,QuantityPerUnit="10 boxes x 20 bags",UnitPrice=18, UnitsInStock=30, Discontinued=0, ReorderLevel=10},
        new Product(){ProductID=1, ProductName="Green tea", SupplierID=1, CategoryID=1,QuantityPerUnit="10 boxes x 20 bags",UnitPrice=18, UnitsInStock=30, Discontinued=0, ReorderLevel=10},
        new Product(){ProductID=1, ProductName="Wine", SupplierID=2, CategoryID=1,QuantityPerUnit="50 boxes x 20 bags",UnitPrice=18, UnitsInStock=30, Discontinued=0, ReorderLevel=10},
        new Product(){ProductID=1, ProductName="Beer", SupplierID=3, CategoryID=1,QuantityPerUnit="30 boxes x 20 bags",UnitPrice=18, UnitsInStock=30, Discontinued=0, ReorderLevel=10}
    };

    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<HomeController> _logger;
    private const string CACHE_PRODUCT_KEY = "products";
    private const string CACHE_DATE_KEY = "date";

    public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }



    public IActionResult Index()
    {
        //List<Product> products;
        if (!_memoryCache.TryGetValue(CACHE_PRODUCT_KEY, out List<Product> cacheprods))
        {
            DateTime date = DateTime.Now;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));
            cacheprods = products;
            _memoryCache.Set(CACHE_PRODUCT_KEY,products);
            _memoryCache.Set(CACHE_DATE_KEY, date);
        }
        var prods = cacheprods;
        ViewBag.PreviousDate = _memoryCache.Get(CACHE_DATE_KEY);
        ViewBag.CurrentDate = DateTime.Now;
        return View(prods);
    }

}

