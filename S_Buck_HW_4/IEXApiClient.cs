using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using S_Buck_HW_4.Models.API;

namespace S_Buck_HW_4
{
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
                result = JsonConvert.DeserializeObject<T>(resultString);
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
    }
}
