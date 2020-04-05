using MyApp.WPF.Models;
using MyApp.WPF.Models.ProgressExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WPF.ViewModels
{
    public class ReportProgressViewModel: ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        [Reactive]
        public double ProgressPercentage { get; set; }

        public ObservableCollection<LogItem> Logs { get; set; }

        public ReactiveCommand<Unit, string> StartCommand { get; protected set; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; protected set; }

        public CancelableProgressWithPercentage<string> ExecutionProgress { get; }

        public ReportProgressViewModel()
        {
            Activator = new ViewModelActivator();

            Logs = new ObservableCollection<LogItem>();

            ExecutionProgress = new CancelableProgressWithPercentage<string>(progress =>
            {
                ProgressPercentage = progress.Percentage;
                Logs.Add(new LogItem()
                {
                    Time = DateTime.Now,
                    Message = progress.Notification
                });
            });

            this.WhenActivated(d =>
            {
                HandleActivation(d);
            });
        }

        private void HandleActivation(CompositeDisposable d)
        {
            CancellationTokenSource canceller = null;

            // Create command.
            StartCommand = ReactiveCommand
                .CreateFromObservable(() =>
                {
                    canceller?.Dispose();
                    canceller = new CancellationTokenSource();
                    return Locator.Current.GetService<IOneOfRequirement>()
                        .TimeConsumingTask(ExecutionProgress.With(canceller.Token))
                        .SubscribeOn(ThreadPoolScheduler.Instance);
                });

            // Create cancellation command.
            CancelCommand = ReactiveCommand
                .Create(() =>
                {
                    canceller?.Cancel();
                },
                StartCommand.IsExecuting)
                .DisposeWith(d);

            // Register the process that when the command was completed.
            StartCommand
                .TakeUntil(CancelCommand)
                .Subscribe(response =>
                {
                    ProgressPercentage = 100;
                    Logs.Add(new LogItem()
                    {
                        Time = DateTime.Now,
                        Message = response
                    });
                })
                .DisposeWith(d);

            // Register the process that when the command was canceled.
            StartCommand
                .ThrownExceptions
                .Where(x => x is TaskCanceledException)
                .Subscribe(exception =>
                {
                    ProgressPercentage = 0;
                    Logs.Add(new LogItem()
                    {
                        Time = DateTime.Now,
                        Message = exception.Message
                    });
                })
                .DisposeWith(d);

            // Execute command
            StartCommand.Execute().Subscribe(
                onNext: response => { },
                onError: exception => { });
        }
    }
}
