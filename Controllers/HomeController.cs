using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.ML.OnnxRuntime;


namespace BrickVault.Controllers;

public class HomeController : Controller
{
    // onnx
    private readonly InferenceSession _session;
    private readonly IWebHostEnvironment _webHostEnvironment; 

    private IConfiguration _configuration;
    private ILegoRepository _repo;
    // private readonly SignInManager<AspNetUser> _signInManager;
    public HomeController(IConfiguration configuration, ILegoRepository tmp, IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _repo = tmp;
        // _signInManager = signInManager;
        _webHostEnvironment = webHostEnvironment;

        var modelPath = Path.Combine(webHostEnvironment.WebRootPath, "model", "decision_tree_model.onnx");
        // onnx- initialize inference session MAKE SURE IT"S A BOOLEAN
        _session = new InferenceSession(modelPath);
    }

    public IActionResult Index()
    {
        List<int> topItems = [27, 33, 34, 37, 24];

        if (User.IsInRole("Customer"))
        {
            topItems = [9, 25, 17, 16, 37];
        }

        ViewBag.Products = _repo.Products
            .Where(x => topItems.Contains(x.ProductId))
            .ToList();

        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Products(int itemsPerPage = 10, int pageNum = 1, List<int> categories = null, List<string> colors = null)
    {
        var primaryColors = _repo.Products.Select(x => x.PrimaryColor);
        var secondaryColors = _repo.Products.Select(x => x.SecondaryColor);
        var allColors = primaryColors.Concat(secondaryColors).Distinct().ToList();

        // Filter products based on selected categories and colors
        IQueryable<Product> filteredProducts = _repo.Products;

        if (categories != null && categories.Any())
        {
            filteredProducts = filteredProducts.Where(p => p.ProductCategories.Any(pc => categories.Contains(pc.CategoryId)));
        }

        if (colors != null && colors.Any())
        {
            filteredProducts = filteredProducts.Where(p => colors.Contains(p.PrimaryColor) || colors.Contains(p.SecondaryColor));
        }

        var model = new ProductListViewModel
        {
            Products = filteredProducts
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * itemsPerPage)
                .Take(itemsPerPage),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = itemsPerPage,
                TotalItems = filteredProducts.Count()
            },

            Categories = _repo.Categories
                .OrderBy(x => x.CategoryName),

            Colors = allColors,
            SelectedCategories = categories,
            SelectedColors = colors,
            ItemsPerPage = itemsPerPage
        };

        return View(model);
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult ProductDetails(int productId)
    {
        Product product = _repo.Products
            .FirstOrDefault(x => x.ProductId == productId);

        ViewBag.Product = product;

        List<byte?> recommendationIds = _repo.ItemRecommendations
            .Where(x => x.ProductId == productId)
            .Select(x => new List<byte?> { x.Recommendation1, x.Recommendation2, x.Recommendation3, x.Recommendation4, x.Recommendation5 })
            .ToList()
            .SelectMany(x => x)
            .ToList();


        List<Product> recommendationProducts = _repo.Products
            .Where(x => recommendationIds.Contains(x.ProductId))
            .ToList();

        ViewBag.Recommendations = recommendationProducts;

        return View();
    }

    public IActionResult Checkout()
    {
        return View();
    }

    // public IActionResult Checkout(Cart cart)
    // {
    //     // Check if user is signed in
    //     if (_signInManager.IsSignedIn(User))
    //     {
    //         // Get the current signed-in user's email
    //         string? userEmail = _signInManager.UserManager.GetUserName(User);
    //         
    //     }
    //
    //     return View();
    // }


    public IActionResult ReviewOrders()
    {
        var records = _repo.Order.ToList(); // fetch allllll the records
        var predictions = new List<FraudPrediction>();
        // dictionary to interpet the prediction
        var fraudTypeDict = new Dictionary<int, string>
     {
         {0, "Not Fraud" },
         {1, "Fraud"}
     };
        foreach (var record in records)
        {
            var input = new List<float>
         {
             (float)record.Time,
             (float)record.Amount,

             record.CountryOfResidence == "India" ? 1 : 0,
             record.CountryOfResidence == "Russia" ? 1 : 0,
             record.CountryOfResidence == "USA" ? 1 : 0,
             record.CountryOfResidence == "United Kingdom" ? 1 : 0,

             record.DayOfWeek == "Mon" ? 1 : 0,
             record.DayOfWeek == "Tue" ? 1 : 0,
             record.DayOfWeek == "Wed" ? 1 : 0,
             record.DayOfWeek == "Thu" ? 1 : 0,
             record.DayOfWeek == "Fri" ? 1 : 0,
             record.DayOfWeek == "Sat" ? 1 : 0,
             record.DayOfWeek == "Sun" ? 1 : 0,

             record.EntryMode == "PIN" ? 1 : 0,
             record.EntryMode == "Tap" ? 1 : 0,

             record.TypeOfTransaction == "Online" ? 1 : 0,
             record.TypeOfTransaction == "POS" ? 1 : 0,

             record.CountryOfTransaction == "India" ? 1 : 0,
             record.CountryOfTransaction == "Russia" ? 1 : 0,
             record.CountryOfTransaction == "USA" ? 1 : 0,
             record.CountryOfTransaction == "United Kingdom" ? 1 : 0,

             record.ShippingAddress == "India" ? 1 : 0,
             record.ShippingAddress == "Russia" ? 1 : 0,
             record.ShippingAddress == "USA" ? 1 : 0,
             record.ShippingAddress == "United Kingdom" ? 1 : 0,

             record.Bank == "HSBC" ? 1 : 0,
             record.Bank == "Halifax" ? 1 : 0,
             record.Bank == "Lloyds" ? 1 : 0,
             record.Bank == "Metro" ? 1 : 0,
             record.Bank == "Monzo" ? 1 : 0,
             record.Bank == "RBS" ? 1 : 0,

             record.TypeOfCard == "Visa" ? 1 : 0
         };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
            var inputs = new List<NamedOnnxValue>
         {
             NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
         };
            string predictionResult;
            using (var results = _session.Run(inputs))
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                predictionResult = prediction != null && prediction.Length > 0 ? fraudTypeDict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
            }
            predictions.Add(new FraudPrediction { Orders = record, Prediction = predictionResult }); // Adds the fraud info & prediction for that animal to FraudPrediction viewmodel
        }
        return View(predictions);
    }

}