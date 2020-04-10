using MyApp.WPF.Models;
using Splat;
using System.Windows;

namespace MyApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Regist Models
            Locator.CurrentMutable.RegisterLazySingleton(() => new OneOfRequirement());

            base.OnStartup(e);
        }
    }
}
