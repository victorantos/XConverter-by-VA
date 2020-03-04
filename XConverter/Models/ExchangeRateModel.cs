using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XConverter.Models
{
    public class ExchangeRateModel : INotifyPropertyChanged
    {
        private string _from;

        public string From
        {
            get { return _from; }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _to;

        public string To
        {
            get { return _to; }
            set
            {
                if (_to != value)
                {
                    _to = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double _rate;
        public double Rate
        {
            get { return _rate; }
            set
            {
                if (_rate != value)
                {
                    _rate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _fromAmount;

        public string FromAmount
        {
            get
            {
                if (decimal.TryParse(_fromAmount, out var amount))
                    return amount.ToString("0.##");

                return string.Empty;
            }
            set
            {
                if (_fromAmount != value)
                {
                    _fromAmount = value;
                    NotifyPropertyChanged();
                    if (Rate > 0)
                    {
                        if (decimal.TryParse(_fromAmount, out var amount))
                            ToAmount = (amount * (decimal)Rate).ToString();
                    }
                    
                }
            }
        }

        private string _toAmount;

        public string ToAmount
        {
            get {
                if (decimal.TryParse(_toAmount, out var amount))
                    return amount.ToString(".##");
                
                return string.Empty;
            }
            set
            {
                if (_toAmount != value)
                {
                    _toAmount = value;

                    NotifyPropertyChanged();
                    
                    if (Rate > 0)
                    {
                        if (decimal.TryParse(_toAmount, out var amount))
                            FromAmount = (amount / (decimal)Rate).ToString();
                    }
                  
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
