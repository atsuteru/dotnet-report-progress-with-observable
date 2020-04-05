using MyApp.WPF.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MyApp.WPF.Views
{
    public partial class ReportProgressView : ReactiveUserControl<ReportProgressViewModel>, IActivatableView
    {
        public ReportProgressView()
        {
            InitializeComponent();

            ViewModel = new ReportProgressViewModel();

            this.WhenActivated(d =>
            {
                HandleActivation(d);
            });
        }

        private void HandleActivation(CompositeDisposable d)
        {
            this.OneWayBind(ViewModel, vm => vm.ProgressPercentage, v => v.ProgressBar.Value).DisposeWith(d);
            this.OneWayBind(ViewModel, vm => vm.Logs, v => v.LogListBox.ItemsSource).DisposeWith(d);
            this.BindCommand(ViewModel, vm => vm.StartCommand, v => v.StartButton).DisposeWith(d);
            this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton).DisposeWith(d);
        }
    }
}
