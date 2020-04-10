using MyApp.WPF.Models;
using MyApp.WPF.Models.ProgressExtensions;
using MyApp.WPF.ViewModels.ProgressExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MyApp.WPF.ViewModels
{
    public class ReportProgressViewModel: ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        [Reactive]
        public double ProgressPercentage { get; set; }

        public ObservableCollection<LogItem> Logs { get; set; }

        public ReactiveCommand<Unit, ProgressWithPercentage<string>> StartCommand { get; protected set; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; protected set; }

        public ReportProgressViewModel()
        {
            Activator = new ViewModelActivator();

            Logs = new ObservableCollection<LogItem>();

            this.WhenActivated(d =>
            {
                HandleActivation(d);
            });
        }

        public IObserver<string> Receiver { get; set; }

        [Reactive]
        public bool IsCancel { get; set; }

        private void HandleActivation(CompositeDisposable d)
        {
            // Create command.
            StartCommand = ReactiveCommand
                .CreateFromObservable(() =>
                {
                    Logs.Clear();
                    return Locator.Current.GetService<IOneOfRequirement>()
                        .TimeConsumingTask(new OneOfRequest())
                        .TakeUntil(CancelCommand)
                        .SubscribeOn(ThreadPoolScheduler.Instance);
                });
            
            // Create cancellation command.
            CancelCommand = ReactiveCommand
                .Create(() => { }, StartCommand.IsExecuting)
                .DisposeWith(d);

            // Register the process that when the command progress received.
            StartCommand
                .SubscribeProgress(progress =>
                {
                    ProgressPercentage = progress.Percentage;
                    Logs.Add(new LogItem()
                    {
                        Time = DateTime.Now,
                        Message = progress.Notification
                    });
                    if (progress.IsCompleted)
                    {
                        Logs.Add(new LogItem()
                        {
                            Time = DateTime.Now,
                            Message = "Task completed."
                        });
                    }
                });

            // Register the process that when the command error received.
            StartCommand
                .ThrownExceptions
                .Subscribe(error =>
                {
                    ProgressPercentage = 0;
                    Logs.Add(new LogItem()
                    {
                        Time = DateTime.Now,
                        Message = error.Message
                    });
                });

            // Register the process that when the command was cancelled.
            CancelCommand
                .Subscribe(_ =>
                {
                    ProgressPercentage = 0;
                    Logs.Add(new LogItem()
                    {
                        Time = DateTime.Now,
                        Message = "Your pizza was cancelled."
                    });
                })
                .DisposeWith(d);
        }
    }
}
