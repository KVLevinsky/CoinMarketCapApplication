using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinMarketCapApplication_v31.Models {
    public class CoinDataModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public string LogoURL { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:N5}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        public decimal Capitalization { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4}%")]
        public float Change1H { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4}%")]
        public float Change24h { get; set; }

        [DisplayFormat(DataFormatString = "{0:D}", HtmlEncode = false)]
        public DateTime LastUpdated { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", HtmlEncode = false)]
        public DateTime LastUpdatedLocalTime {
            get {
                return LastUpdated.ToLocalTime();
            }
        }
    }
}
