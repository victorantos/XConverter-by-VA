using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XConverter.Api;
using XConverter.Api.Models;
using XConverter.Helpers;

namespace XConverter.ViewModels
{
    public class CurrencyViewModel : Screen
    {
        private ICommand _clickLeftSideCommand;
        public ICommand ClickLeftSideCommand
        {
            get
            {
                return _clickLeftSideCommand ?? (_clickLeftSideCommand = new CommandHandler((arg) => LeftSideClick(arg), () => CanExecute));
            }
        }

        private ICommand _clickRightSideCommand;
        public ICommand ClickRightSideCommand
        {
            get
            {
                return _clickRightSideCommand ?? (_clickRightSideCommand = new CommandHandler((arg) => RightSideClick(arg), () => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }
  
        public List<string> Currencies { get; set; }

        private IEventAggregator _eventAggregator;

        public CurrencyViewModel()
        {

        }
        public CurrencyViewModel(CurrencyExchange currencyExchange, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Currencies = currencyExchange.Rates.Keys.OrderBy(k => k).ToList();
        }
        private void LeftSideClick(object param)
        {
            _eventAggregator.PublishOnUIThread("FROM:" + param);
        }
        private void RightSideClick(object param)
        {
            _eventAggregator.PublishOnUIThread("TO:" + param);
        }

    }
}
