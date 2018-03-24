using System;

namespace OUILookup.Data.Models
{
    public class OUI
    {
        public string OUIAddress { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }

        public OUI(string ouiAddress, string symbol, string companyName = null)
        {
            OUIAddress = ouiAddress;
            Symbol = symbol;
            CompanyName = companyName;
        }
    }
}
