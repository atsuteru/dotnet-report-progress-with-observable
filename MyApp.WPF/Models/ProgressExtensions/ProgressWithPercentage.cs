using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public class ProgressWithPercentage<TNotification> : IProgressReporter
    {

        public static IObservable<ProgressWithPercentage<TNotification>> CreateObservableFromAsync<TParam>(Func<ProgressObserver<ProgressWithPercentage<TNotification>>, TParam, Task> func, TParam param)
        {
            return Observable
                .Create<ProgressWithPercentage<TNotification>>((o, ct) => func.Invoke(new ProgressObserver<ProgressWithPercentage<TNotification>>(o, ct), param));
        }

        public double Percentage { get; }

        public TNotification Notification { get; }

        public bool IsCompleted { get; }

        public ProgressWithPercentage(TNotification notification, double percentage, bool isCompleted = false)
        {
            Percentage = percentage;
            Notification = notification;
            IsCompleted = isCompleted;
        }

        public bool IsProgressChanged(IProgressReporter previewReport)
        {
            if (!(previewReport is ProgressWithPercentage<TNotification> target))
            {
                return true;
            }
            return (Percentage - target.Percentage) > 1d;
        }
    }
}
