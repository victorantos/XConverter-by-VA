using System;
using System.Collections.Generic;
using System.Text;

namespace XConverter.Api.Models
{
    public class CurrencyExchange
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}
