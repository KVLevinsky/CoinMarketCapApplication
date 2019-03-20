using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CoinMarketCapApplication.Services {
    public class CoinMarketCapService : IHostedService, IDisposable {
        Timer _timer;
        int queryIntervalMinutes = 5;
        int factor = 1;
        private string _apiKey = "ec07675a-6b10-4caa-b533-0eeb6367875e";
        private ICoinMarketCapCache _cache;

        public CoinMarketCapService(ICoinMarketCapCache cache) {
            if (cache == null) {
                throw new ArgumentNullException(nameof(cache));
            } else {
                _cache = cache;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(_serviceProcess, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(queryIntervalMinutes));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        void _serviceProcess(object state) {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "30";
            queryString["convert"] = "USD";
            queryString["sort"] = "market_cap";
            queryString["sort_dir"] = "desc";

            string response = _queryCoinMarketCapAPI("pro-api.coinmarketcap.com", "/v1/cryptocurrency/listings/latest", queryString.ToString());
            dynamic parsedResponse = JsonConvert.DeserializeObject(response);
            if (parsedResponse.status.error_code == 0) {
                factor = 1;
                _cache.AddData(response);

                List<string> ids = _cache.GetEmptyLogoIds();
                if (ids?.Count > 0) {
                    queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString["id"] = string.Join(',', ids);

                    string auxResponse = _queryCoinMarketCapAPI("pro-api.coinmarketcap.com", "/v1/cryptocurrency/info", queryString.ToString());
                    dynamic parsedAuxResponse = JsonConvert.DeserializeObject(auxResponse);
                    if (parsedAuxResponse.status.error_code == 0) {
                        _cache.AddAuxData(auxResponse);
                    }
                }
            } else {
                factor = factor > 16 ? 16 : factor * 2;
            }

            _timer.Change(TimeSpan.FromMinutes(queryIntervalMinutes), TimeSpan.FromMinutes(queryIntervalMinutes * factor));
        }

        string _queryCoinMarketCapAPI(string host, string path, string query) {
            var uri = new UriBuilder() { Scheme = "https", Host = host, Path = path, Query = query };

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", _apiKey);
            client.Headers.Add("Accepts", "application/json");
            return client.DownloadString(uri.ToString());
        }

        public void Dispose() {
            _timer?.Dispose();
        }
    }
}
