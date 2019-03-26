using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using S_Buck_HW_4.Models;

namespace S_Buck_HW_4.Controllers
{
    public class SymbolsController : Controller
    {
        //Base URL for the IEXTrading API. Method specific URLs are appended to this base URL.
        string BASE_URL = "https://api.iextrading.com/1.0/";
        HttpClient httpClient;
        public SymbolsController(/*ApplicationDbContext context*/)
        {
            //dbContext = context;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            string IEXTrading_API_PATH = BASE_URL + "ref-data/symbols";
            string resultString = "";
            List<StockSymbol> symbols = null;
            
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                resultString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!resultString.Equals(""))
            {
                symbols = JsonConvert.DeserializeObject<List<StockSymbol>>(resultString);
                symbols = symbols.GetRange(0, 50);
            }

            return View(symbols);
        }

        public IActionResult Details(string symbol)
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/"+ symbol +"/stats";
            string resultString = "";
            StockDetails stockDetails = null;

            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                resultString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!resultString.Equals(""))
            {
                stockDetails = JsonConvert.DeserializeObject<StockDetails>(resultString);
            }

            return View(stockDetails);
        }
    }
}