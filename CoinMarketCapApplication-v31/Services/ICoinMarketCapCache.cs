using CoinMarketCapApplication_v31.Models;
using System.Collections.Generic;

namespace CoinMarketCapApplication_v31.Services {
    public interface ICoinMarketCapCache {
        void AddData(string data);
        void AddAuxData(string data);
        List<string> GetEmptyLogoIds();
        dynamic LastValuableData { get; }
        List<CoinDataModel> Data { get; }
    }
}