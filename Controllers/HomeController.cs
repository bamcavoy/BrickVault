using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Elfie.Serialization;

namespace BrickVault.Controllers;

public class HomeController : Controller
{
    // onnx
    private readonly InferenceSession _session;

    private IConfiguration _configuration;
    private ILegoRepository _repo;
    public HomeController(IConfiguration configuration, ILegoRepository tmp)
    {
        _configuration = configuration;
        _repo = tmp;

        // onnx- initialize inference session
        _session = new InferenceSession("decision_tree_model.onnx");
    }
    
    public IActionResult Index()
    {
        ViewBag.Products = _repo.Products
            .OrderBy(x => x.Name)
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
    public IActionResult Products(int itemsPerPage = 10, int pageNum = 1)
    {
        var primaryColors = _repo.Products.Select(x => x.PrimaryColor);
        var secondaryColors = _repo.Products.Select(x => x.SecondaryColor);

        var allColors = primaryColors.Concat(secondaryColors).Distinct().ToList();
        
        var model = new ProductListViewModel
        {
            Products = _repo.Products
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * itemsPerPage)
                .Take(itemsPerPage),
        
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = itemsPerPage,
                TotalItems = _repo.Products.Count()
            },
            
            Categories = _repo.Categories
                .OrderBy(x => x.CategoryName),
            
            Colors = allColors
        };

        return View(model);
    }
    
    public IActionResult AboutUs()
    {
        return View();
    }
    
    public IActionResult ReviewOrders()
    {
        //var records = _repo.Orders.ToList(); // fetch allllll the records
        //var predictions = new List<FraudPrediction>();

        //// dictionary to interpet the prediction
        //var fraudTypeDict = new Dictionary<int, string>
        //{
        //    {0, "Not Fraud" },
        //    {1, "Fraud"}
        //};

        //foreach (var record in records)
        //{
        //    var input = new List<float>
        //    {
        //        (float)record.Age,
        //        (float)record.Time,
        //        (float)record.Amount,

        //        record.CountryOfResidence == "India" ? 1 : 0,
        //        record.CountryOfResidence == "Russia" ? 1 : 0,
        //        record.CountryOfResidence == "USA" ? 1 : 0,
        //        record.CountryOfResidence == "United Kingdom" ? 1 : 0,

        //        record.Gender == "M" ? 1 : 0,

        //        record.DayOfWeek == "Mon" ? 1 : 0,
        //        record.DayOfWeek == "Tue" ? 1 : 0,
        //        record.DayOfWeek == "Wed" ? 1 : 0,
        //        record.DayOfWeek == "Thu" ? 1 : 0,
        //        record.DayOfWeek == "Fri" ? 1 : 0,
        //        record.DayOfWeek == "Sat" ? 1 : 0,
        //        record.DayOfWeek == "Sun" ? 1 : 0,

        //        record.EntryMode == "PIN" ? 1 : 0,
        //        record.EntryMode == "Tap" ? 1 : 0,

        //        record.TypeOfTransaction == "Online" ? 1 : 0,
        //        record.TypeOfTransaction == "POS" ? 1 : 0,

        //        record.CountryOfTransaction == "India" ? 1 : 0,
        //        record.CountryOfTransaction == "Russia" ? 1 : 0,
        //        record.CountryOfTransaction == "USA" ? 1 : 0,
        //        record.CountryOfTransaction == "United Kingdom" ? 1 : 0,

        //        record.ShippingAddress == "India" ? 1 : 0,
        //        record.ShippingAddress == "Russia" ? 1 : 0,
        //        record.ShippingAddress == "USA" ? 1 : 0,
        //        record.ShippingAddress == "United Kingdom" ? 1 : 0,

        //        record.Bank == "HSBC" ? 1 : 0,
        //        record.Bank == "Halifax" ? 1 : 0,
        //        record.Bank == "Lloyds" ? 1 : 0,
        //        record.Bank == "Metro" ? 1 : 0,
        //        record.Bank == "Monzo" ? 1 : 0,
        //        record.Bank == "RBS" ? 1 : 0,

        //        record.TypeOfCard == "Visa" ? 1 : 0
        //    };
        //    var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

        //    var inputs = new List<NamedOnnxValue>
        //    {
        //        NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
        //    };

        //    string predictionResult;
        //    using (var results = _session.Run(inputs))
        //    {
        //        var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
        //        predictionResult = prediction != null && prediction.Length > 0 ? fraudTypeDict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
        //    }

        //    predictions.Add(new FraudPrediction { Orders = record, Prediction = predictionResult }); // Adds the fraud info & prediction for that animal to FraudPrediction viewmodel
        //    }

        //return View(predictions);
        return View();
    }
    
    public IActionResult Cart()
    {
        return View();
    }
}