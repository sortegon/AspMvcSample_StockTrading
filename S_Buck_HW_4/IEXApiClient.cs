using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using S_Buck_HW_4.Models.API;

namespace S_Buck_HW_4
{
    /// <summary>
    /// Class to factor out calls to the IEX api
    /// </summary>
    public class IEXApiClient
    {
        //Base URL for the IEXTrading API. Method specific URLs are appended to this base URL.
        const string BASE_URL = "https://api.iextrading.com/1.0/";
        readonly HttpClient httpClient;

        public IEXApiClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Wrapper function for calling a GET on the API and deserializing the result
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON result to</typeparam>
        /// <param name="apiPath">The full API URL to call GET on</param>
        /// <returns>An object of type T created from the API response</returns>
        private T Get<T>(string apiPath)
        {
            string resultString = "";
            T result = default(T); // null for class types, 0 for int, false for bool, etc.

            HttpResponseMessage response = httpClient.GetAsync(apiPath).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                resultString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!resultString.Equals(""))
            {
                // IEX api can return invalid values for their type.
                // Ignore deserialization errors and leave those properties with default values (usually null)
                var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
                result = JsonConvert.DeserializeObject<T>(resultString, settings);
            }

            return result;
        }

        public List<StockSymbol> GetAllSymbols()
        {
            string apiPath = BASE_URL + "ref-data/symbols";
            return Get<List<StockSymbol>>(apiPath);
        }

        public StockDetails GetStockDetails(string symbol)
        {
            string apiPath = BASE_URL + "stock/" + symbol + "/stats";
            return Get<StockDetails>(apiPath);
        }

        public StockQuote GetStockQuote(string symbol)
        {
            string apiPath = BASE_URL + "stock/" + symbol + "/quote";
            return Get<StockQuote>(apiPath);
        }

        /// <summary>
        /// Batch method to get Quotes for multiple symbols with one api call
        /// </summary>
        /// <param name="symbols">The multiple symbols to get quotes for</param>
        /// <returns>A dictionary mapping symbols to quotes</returns>
        public IDictionary<string, StockQuote> GetStockQuotes(ICollection<string> symbols)
        {
            // If there are no symbols, avoid an api call and just return an empty dictionary
            if (!symbols.Any()) return new Dictionary<string, StockQuote>();

            var symbolList = String.Join(",", symbols);
            string apiPath = BASE_URL + $"stock/market/batch?types=quote&symbols={symbolList}";

            /*  batch endpoint returns an extra level of nesting i.e.
                    { "AAPL": { "quote": { ... } } }
                so deserialize as a dictionary-of-dictionaries and then get out the "quote" value
             */
            var result = Get<IDictionary<string,IDictionary<string,StockQuote>>>(apiPath);
            return result.ToDictionary(x => x.Key, x =>
            {
                var exists = x.Value.TryGetValue("quote", out var value);
                return exists ? value : null;
            });
        }
    }
}
