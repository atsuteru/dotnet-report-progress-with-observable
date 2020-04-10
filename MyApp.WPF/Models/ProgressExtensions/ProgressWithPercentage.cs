using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public class ProgressWithPercentage<TNotification> : IProgressReporter<TNotification>
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
    }
}
