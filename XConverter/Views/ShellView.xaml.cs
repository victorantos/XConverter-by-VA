using System.Windows;
using XConverter.Api;

namespace XConverter.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {

        public ShellView()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         
        }
    }
}
