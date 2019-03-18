using CoinMarketCapApplication.Models;
using System.Collections.Generic;

namespace CoinMarketCapApplication.Services {
    public interface ICoinMarketCapCache {
        void AddData(string data);
        void AddAuxData(string data);
        List<string> GetEmptyLogoIds();
        dynamic LastValuableData { get; }
        List<CoinDataModel> Data { get; }
    }
}