using CoinMarketCapApplication_v31.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinMarketCapApplication_v31.Services {
    public class CoinMarketCapCache : ICoinMarketCapCache {
        private string _lastValuableData = string.Empty;
        private Dictionary<string, string> _logoUrls = new Dictionary<string, string>();

        public dynamic LastValuableData => JsonConvert.DeserializeObject(_lastValuableData);

        public List<CoinDataModel> Data {
            get {
                List<CoinDataModel> value = new List<CoinDataModel>();
                try {
                    foreach (var item in LastValuableData.data)
                        value.Add(new CoinDataModel() {
                            Id = item.id,
                            Name = item.name,
                            Symbol = item.symbol,
                            Price = item.quote.USD.price,
                            Change1H = item.quote.USD.percent_change_1h,
                            Change24h = item.quote.USD.percent_change_24h,
                            Capitalization = item.quote.USD.market_cap,
                            LastUpdated = item.quote.USD.last_updated,
                            LogoURL = _logoUrls.TryGetValue(item.id.ToString(), out string logo) ? logo : string.Empty
                        });
                } catch { }
                return value;
            }
        }

        public void AddData(string data) {
            dynamic value = JsonConvert.DeserializeObject(data);
            if (value.status.error_code == 0) {
                _lastValuableData = data;
            }
        }

        public void AddAuxData(string data) {
            dynamic value = JsonConvert.DeserializeObject(data);
            if (value.status.error_code == 0) {
                foreach (var item in value.data) {
                    _logoUrls[item.Name] = item.Value.logo.ToString();
                }
            }
        }

        public List<string> GetEmptyLogoIds() {
            List<string> value = Data.Where(x => x.LogoURL == string.Empty).Select(x => x.Id.ToString()).ToList();
            return value.Count > 0 ? value : null;
        }
    }
}
