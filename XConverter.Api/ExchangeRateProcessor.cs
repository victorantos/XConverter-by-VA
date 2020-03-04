using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XConverter.Api.Models;

namespace XConverter.Api
{
    public class ExchangeRateProcessor
    {
        public static async Task<CurrencyExchange> LoadData(string baseRate)
        {
            string url = "https://api.exchangeratesapi.io/latest?base=" + baseRate;
          
            using(HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<CurrencyExchange>();
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
