using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using S_Buck_HW_4.Models;
using S_Buck_HW_4.Models.API;
using S_Buck_HW_4.Models.Database;

namespace S_Buck_HW_4.Controllers
{
    public class SymbolsController : Controller
    {
        private readonly IEXApiClient _apiClient;
        private readonly ApplicationDbContext _dbContext;

        public SymbolsController(IEXApiClient apiClient, ApplicationDbContext dbContext)
        {
            _apiClient = apiClient;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Since the API call for Stock Symbols can take a while, we cache the results into the database.
            // For this exercise, we will assume the API data does not change frequently, but in a production app
            // we would want to track the last time the API was called and make a refresh policy.

            var stocks = _dbContext.StockCompanies;
            if (!stocks.Any())
            {
                var symbols = _apiClient.GetAllSymbols();
                var companies =
                    from s in symbols
                    select new StockCompany {Symbol = s.Symbol, CompanyName = s.Name};
                stocks.AddRange(companies);
                _dbContext.SaveChanges();
            }

            return View(stocks.Take(50));
        }
        
        [Route("[controller]/{symbol}")]
        public IActionResult Details(string symbol)
        {
            var model = new SymbolDetailViewModel
            {
                Details = _apiClient.GetStockDetails(symbol),
                Quote = _apiClient.GetStockQuote(symbol)
            };

            return View(model);
        }
        [Route("[controller]/{symbol}/[action]")]
        public IActionResult Buy(string symbol)
        {
            if (String.IsNullOrEmpty(symbol)) return BadRequest();

            symbol = symbol.ToUpper();

            var company = _dbContext.StockCompanies.Find(symbol);
            if (company == null) return NotFound();

            var quote = _apiClient.GetStockQuote(symbol);
            if (quote?.LatestPrice == null) return NotFound();

            var users =
                from user in _dbContext.Users
                orderby user.LastName
                select new
                {
                    ID = user.UserID,
                    FullName = user.FirstName + " " + user.LastName
                };

            var trade = new UserStockTrade
            {
                Symbol = symbol,
                Price = quote.LatestPrice.Value,
                Shares = 1
            };

            ViewBag.CompanyName = company.CompanyName;
            ViewBag.Users = new SelectList(users, "ID", "FullName");

            return View(trade);
        }
    }
}