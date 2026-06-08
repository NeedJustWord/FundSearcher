using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using Fund.Core.Helpers;
using Fund.DataBase;
using FundSearcher.Controls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;

namespace FundSearcher
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        private Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new Mutex(true, "FundSearcher_Mutex", out var createdNew);
            if (createdNew)
            {
                base.OnStartup(e);
            }
            else
            {
                MessageBoxEx.ShowError("程序已在运行，请检查任务栏");
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.Dispose();
            base.OnExit(e);
        }

        protected override Window CreateShell()
        {
            Config.ConfigPath = $"{Assembly.GetEntryAssembly().GetName().Name}.exe.config";
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<FundDataBase>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Module>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.Assembly.FullName;
                var viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
                return Type.GetType(viewModelName);
            });
        }
    }
}
