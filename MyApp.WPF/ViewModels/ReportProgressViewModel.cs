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

namespace MyApp.WPF.ViewModels
{
    public class ReportProgressViewModel: ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        [Reactive]
        public double ProgressPercentage { get; set; }

        public ObservableCollection<LogItem> Logs { get; set; }

        public ReactiveCommand<Unit, string> StartCommand { get; protected set; }

        public ReportProgressViewModel()
        {
            Activator = new ViewModelActivator();

            Logs = new ObservableCollection<LogItem>();

            this.WhenActivated(d =>
            {
                HandleActivation(d);
            });
        }

        private void HandleActivation(CompositeDisposable d)
        {
            // Create command.
            StartCommand = ReactiveCommand
                .CreateFromObservable(() =>
                {
                    return Locator.Current.GetService<IOneOfRequirement>()
                        .TimeConsumingTask(new Progress<WithPercentage<string>>(progress =>
                        {
                            ProgressPercentage = progress.Percentage;
                            Logs.Add(new LogItem() 
                            { 
                                Time = DateTime.Now,
                                Message = progress.Notification
                            });
                        }))
                        .SubscribeOn(ThreadPoolScheduler.Instance);
                });

            // Register the process that when the command was completed.
            StartCommand
                .Subscribe(response =>
                {
                    Logs.Add(new LogItem()
                    {
                        Time = DateTime.Now,
                        Message = response
                    });
                })
                .DisposeWith(d);

            // Execute command
            StartCommand.Execute().Subscribe();
        }
    }
}
