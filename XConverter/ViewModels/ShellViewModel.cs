using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XConverter.Api;
using XConverter.Api.Models;
using XConverter.Helpers;
using XConverter.Models;

namespace XConverter.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<string>, INotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;
        private ICommand _closeCommand;
        private ICommand _openCommand;
        private ICommand _removeCurrencyCommand;
        private BindableCollection<ExchangeRateModel> _savedExchangeRates = new BindableCollection<ExchangeRateModel>();

        public ShellViewModel()
        {
            var data = FileHelper.LoadSavedCurrencies();
            if (data.Any())
            {
                _savedExchangeRates.AddRange(data);
            }

            _eventAggregator = new EventAggregator();
            _eventAggregator.Subscribe(this);

        }
         
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new CommandHandler((args) => ApplicationClosing(args), () => { return true; } ));
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new CommandHandler((args) => ApplicationOpening(args), () => { return true; }));
            }
        }

        public ICommand RemoveCurrencyCommand
        {
            get
            {
                return _removeCurrencyCommand ?? (_removeCurrencyCommand = new CommandHandler((args) => RemoveCurrencyFromList(args), () => { return true; } ));
            }
        }
        public BindableCollection<ExchangeRateModel> SavedExchangeRates
        {
            get { return _savedExchangeRates; }
            set
            {
                if (_savedExchangeRates == value)
                {
                    return;
                }
             
                _savedExchangeRates = value;
                NotifyOfPropertyChange(nameof(SavedExchangeRates));
            }
        }

        public async void LoadNewCurrencyPage()
        {
            var rates = await ExchangeRateProcessor.LoadData("GBP");
           
            ActivateItem(new CurrencyViewModel(rates, _eventAggregator));
        }
 
        public async void Handle(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            var msgs = message.Split(':');
            switch (msgs[0])
            {
                case "FROM":
                    AddOrUpdateFromCurrency(msgs[1]); 
                    break;
                case "TO":
                    AddOrUpdateToCurrency(msgs[1]);
                    break;
                case "RATE":
                    string from = msgs[1], to = msgs[2];
                    if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                    {
                        double rate = 1;
                        if (from != to)
                            rate = (await ExchangeRateProcessor.LoadData(from)).Rates[to];
                        UpdateCurrencyRate(from, to, rate);
                    }
                    break;
                default:
                    break;
            }
        }
         
        public void ApplicationClosing(object args)
        {
            try
            {
                FileHelper.SaveCurrencies(SavedExchangeRates.ToList());
            }
            catch (Exception ex)
            {
                // TODO log the error

                MessageBox.Show("An error has occured while trying to save currency list.");
            }
        }

        public async void ApplicationOpening(object args)
        {
            try
            {
                LoadNewCurrencyPage();
            }
            catch (Exception ex)
            {
                // TODO log the error

                MessageBox.Show("An error has occured while trying to load currencies. Please check your Internet connection or try later.");
            }
        }

        public void RemoveCurrencyFromList(object exchangeRate)
        {
            SavedExchangeRates.Remove((ExchangeRateModel)exchangeRate);
        }

        /// <summary>
        /// Update exchange rate for specific currency pairs from the main list
        /// </summary>
        /// <param name="from">from currency</param>
        /// <param name="to">to currency</param>
        /// <param name="rate">updated exchange rate</param>
        private void UpdateCurrencyRate(string from, string to, double rate)
        {
            foreach (var item in SavedExchangeRates)
            {
                if (item.From == from && item.To == to)
                    item.Rate = rate;
            }
        }

        private void AddOrUpdateFromCurrency(string curr)
        {
            if (string.IsNullOrEmpty(curr))
                return;

            var isNewCurrencyLine = true;
            foreach (var item in SavedExchangeRates)
            {
                if (string.IsNullOrEmpty(item.From) || string.IsNullOrEmpty(item.To))
                    isNewCurrencyLine = false;

                if (!isNewCurrencyLine)
                {
                    item.From = curr;

                    // Notify subscribers about a new currency pair that needs an exchange rate
                    _eventAggregator.PublishOnUIThread("RATE:" + item.From + ":" + item.To);
                    break;
                }
            }

            if (isNewCurrencyLine)
                AddNewCurrencyExchangeLine(curr, default);
        }

        private void AddNewCurrencyExchangeLine(string from, string to)
        {
            // Allow max 5 currency pairs
            const int maxPairs = 5;
            if (SavedExchangeRates.Count >= maxPairs)
                SavedExchangeRates.RemoveAt(maxPairs-1);

            SavedExchangeRates.Insert(0, new ExchangeRateModel { From = from, To = to });
        }

        private void AddOrUpdateToCurrency(string curr)
        {
            if (string.IsNullOrEmpty(curr))
                return;

            var isNewCurrencyLine = true;
            foreach (var item in SavedExchangeRates)
            {
                if (string.IsNullOrEmpty(item.From) || string.IsNullOrEmpty(item.To))
                    isNewCurrencyLine = false;

                if(!isNewCurrencyLine)
                {
                    item.To = curr;
                    _eventAggregator.PublishOnUIThread("RATE:" + item.From + ":" + item.To);
                    break;
                }
            }

            if (isNewCurrencyLine)
                AddNewCurrencyExchangeLine(default(string), curr);
        }
    }
}
