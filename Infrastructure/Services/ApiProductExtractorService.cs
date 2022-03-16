using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ApiProductExtractorService : IApiProductExtractorService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ApiProductExtractorService> _logger;

        public ApiProductExtractorService(IConfiguration config, ILogger<ApiProductExtractorService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<List<Products>> GetProductsApiAsync()
        {
            List<Products> products = new List<Products>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = _config["Api:ApiUrl"]; ;
                    string apikey = _config["Api:ApiKey"];
                    client.DefaultRequestHeaders.Add("x-apikey", apikey);

                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new BadResponseApiException(String.Format("Error de Servidor. StatusCode: {0}",
                                response.StatusCode));

                        var jsonString = response.Content.ReadAsStringAsync();
                        RootObject rootOb = JsonConvert.DeserializeObject<RootObject>(jsonString.Result);

                        foreach (var item in rootOb.Products)
                        {
                            var producto = new Products()
                            {
                                Code = item.Code,
                                Sku = item.Sku,
                                Currency = item.Currency,
                                Stock = (int)item.Stock,
                                Price = item.Price,
                                Iva = item.Iva,
                                Ii = (int)item.Ii,
                            };
                            products.Add(producto);
                        }
                    }
                }
            }
            catch (BadResponseApiException e)
            {
                _logger.LogError(e.Message);
            }
            return products;
        }
    }
}
