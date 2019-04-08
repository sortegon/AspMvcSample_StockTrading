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

        public IActionResult Index(string beginsWith = "A")
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
                    where s.IsEnabled  // skip symbols that aren't enabled on IEX
                    select new StockCompany {Symbol = s.Symbol, CompanyName = s.Name};
                stocks.AddRange(companies);
                _dbContext.SaveChanges();
            }

            var filteredStocks = stocks.Where(s => s.Symbol.StartsWith(beginsWith));

            ViewBag.BeginsWith = beginsWith;  // pass to the view so the selected letter can be bolded instead of linked

            return View();
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
                Shares = 1  // We don't allow shares to be 0, so start at 1
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
            if (String.IsNullOrEmpty(symbol) || symbol != trade.Symbol || trade.Shares == 0) return BadRequest();

            if (!ModelState.IsValid) return View(trade);

            // We need to use a RepeatableRead transaction to ensure that the UserStock cannot be modified
            // while we are updating it and saving this trade
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

                trade.TradeDateTime = DateTime.Now;

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
                Shares = 1  // We don't allow shares to be 0, so start at 1
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
            if (String.IsNullOrEmpty(symbol) || symbol != trade.Symbol || trade.Shares == 0
                || !userId.HasValue || userId <= 0 || userId != trade.UserID) return BadRequest();

            if (!ModelState.IsValid) return View(trade);

            // We need to use a RepeatableRead transaction to ensure that the UserStock cannot be modified
            // while we are updating it and saving this trade
            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                var userStock = _dbContext.UserStocks.Find(trade.UserID, trade.Symbol);
                if (userStock == null || userStock.Shares < trade.Shares) return BadRequest();

                trade.TradeDateTime = DateTime.Now;
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

        [Route("[controller]/{symbol}/[action]")]
        public IActionResult AddFavorite(string symbol, int? userId)
        {
            if (String.IsNullOrEmpty(symbol)) return BadRequest();

            symbol = symbol.ToUpper();

            var company = _dbContext.StockCompanies.Find(symbol);
            if (company == null) return NotFound();

            var users =
                from user in _dbContext.Users
                orderby user.LastName
                select new
                {
                    ID = user.UserID,
                    FullName = user.FirstName + " " + user.LastName
                };

            var favorite = new UserStockFavorite()
            {
                Symbol = symbol
            };

            if (userId.HasValue && users.Select(u => u.ID).Contains(userId.Value))
            {
                favorite.UserID = userId.Value;
            }

            ViewBag.CompanyName = company.CompanyName;
            ViewBag.Users = new SelectList(users, "ID", "FullName");

            return View(favorite);
        }

        [HttpPost, Route("[controller]/{symbol}/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult AddFavorite(string symbol, [Bind("Symbol,UserID")] UserStockFavorite favorite)
        {
            if (String.IsNullOrEmpty(symbol) || symbol != favorite.Symbol) return BadRequest();

            if (!ModelState.IsValid) return View(favorite);

                var userFavorite = _dbContext.UserStockFavorites.Find(favorite.UserID, favorite.Symbol);
                // if the stock is already in the User's favorites, we just do nothing
                if (userFavorite == null)
                {
                    _dbContext.UserStockFavorites.Add(favorite);
                    _dbContext.SaveChanges();
                }

            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/{symbol}/[action]")]
        public IActionResult RemoveFavorite(string symbol, int? userId)
        {
            if (String.IsNullOrEmpty(symbol) || !userId.HasValue || userId <= 0) return BadRequest();

            symbol = symbol.ToUpper();

            var favorite = _dbContext.UserStockFavorites
                .Include(us => us.StockCompany)
                .FirstOrDefault(us => us.UserID == userId & us.Symbol == symbol);
            if (favorite == null) return NotFound();

            var company = favorite.StockCompany;

            var users =
                from user in _dbContext.Users
                orderby user.LastName
                select new
                {
                    ID = user.UserID,
                    FullName = user.FirstName + " " + user.LastName
                };
            ViewBag.CompanyName = company.CompanyName;
            ViewBag.Users = new SelectList(users, "ID", "FullName");

            return View(favorite);
        }

        [HttpPost, Route("[controller]/{symbol}/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFavorite(string symbol, int? userId, [Bind("Symbol,UserID")] UserStockFavorite favorite)
        {
            if (String.IsNullOrEmpty(symbol) || symbol != favorite.Symbol
                || !userId.HasValue || userId <= 0 || userId != favorite.UserID) return BadRequest();

            if (!ModelState.IsValid) return View(favorite);

            var userFavorite = _dbContext.UserStocks.Find(favorite.UserID, favorite.Symbol);
            // if the stock is not in the User's favorites, we just do nothing
            if (userFavorite != null)
            {
                _dbContext.UserStocks.Remove(userFavorite);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Details", "Users", new {id = userId});
        }
    }
}