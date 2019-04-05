using System;
using System.Collections.Generic;
using System.Data;
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
        public IActionResult Buy(string symbol, int? userId)
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

            if (userId.HasValue && users.Select(u => u.ID).Contains(userId.Value))
            {
                trade.UserID = userId.Value;
            }

            ViewBag.CompanyName = company.CompanyName;
            ViewBag.Users = new SelectList(users, "ID", "FullName");

            return View(trade);
        }

        [HttpPost, Route("[controller]/{symbol}/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(string symbol, [Bind("Symbol,Price,UserID,Shares")] UserStockTrade trade)
        {
            if (String.IsNullOrEmpty(symbol) || symbol != trade.Symbol) return BadRequest();

            if (!ModelState.IsValid) return View(trade);

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                var userStock = _dbContext.UserStocks.Find(trade.UserID, trade.Symbol);
                if (userStock == null)
                {
                    userStock = new UserStock
                    {
                        UserID = trade.UserID,
                        Symbol = trade.Symbol,
                        Shares = 0,
                        Basis = 0
                    };

                    _dbContext.UserStocks.Add(userStock);
                    _dbContext.SaveChanges();
                }

                userStock.Shares += trade.Shares;
                userStock.Basis += trade.Shares * trade.Price;

                trade.TradeDateTime = DateTime.UtcNow;

                _dbContext.UserStockTrades.Add(trade);
                _dbContext.UserStocks.Update(userStock);

                _dbContext.SaveChanges();
                transaction.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/{symbol}/[action]")]
        public IActionResult Sell(string symbol, int? userId)
        {
            if (String.IsNullOrEmpty(symbol) || !userId.HasValue || userId <= 0) return BadRequest();

            symbol = symbol.ToUpper();

            var userStock = _dbContext.UserStocks
                .Include(us => us.StockCompany)
                .FirstOrDefault(us => us.UserID == userId & us.Symbol == symbol);
            if (userStock == null) return NotFound();

            var company = userStock.StockCompany;

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
                UserID = userId.Value,
                Symbol = symbol,
                Price = quote.LatestPrice.Value,
                Shares = 1
            };

            ViewBag.MaxShares = userStock.Shares;
            ViewBag.CompanyName = company.CompanyName;
            ViewBag.Users = new SelectList(users, "ID", "FullName");

            return View(trade);
        }

        [HttpPost, Route("[controller]/{symbol}/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Sell(string symbol, int? userId, [Bind("Symbol,Price,UserID,Shares")] UserStockTrade trade)
        {
            if (String.IsNullOrEmpty(symbol) || symbol != trade.Symbol
                || !userId.HasValue || userId <= 0 || userId != trade.UserID) return BadRequest();

            if (!ModelState.IsValid) return View(trade);

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                var userStock = _dbContext.UserStocks.Find(trade.UserID, trade.Symbol);
                if (userStock == null || userStock.Shares < trade.Shares) return BadRequest();

                trade.TradeDateTime = DateTime.UtcNow;
                trade.Shares = -trade.Shares; // switch to negative for selling

                userStock.Shares += trade.Shares;
                userStock.Basis += trade.Shares * trade.Price;

                _dbContext.UserStockTrades.Add(trade);
                _dbContext.UserStocks.Update(userStock);

                _dbContext.SaveChanges();
                transaction.Commit();
            }

            return RedirectToAction("Details", "Users", new {id = userId});
        }
    }
}