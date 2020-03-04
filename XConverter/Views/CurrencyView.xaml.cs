using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XConverter.Api;
using XConverter.Api.Models;

namespace XConverter.Views
{
    /// <summary>
    /// Interaction logic for NewCurrencyView.xaml
    /// </summary>
    public partial class CurrencyView : UserControl
    {
        public CurrencyView()
        {
            InitializeComponent();
        }

        private CurrencyExchange currencyExchange;
        private async Task LoadData()
        {
            //currencyExchange = await ExchangeRateProcessor.LoadData("GBP");
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //await LoadData();

        }
 
    }
}
