using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XConverter.ViewModels;

namespace XConverter
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container =
           new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
           // _container.Singleton<IEventAggregator, EventAggregator>();
          //  _container.RegisterPerRequest(typeof(ShellViewModel), null, typeof(ShellViewModel));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewFor<ShellViewModel>();
        }   
    }
}
