using System.Threading.Tasks;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public static class ProgressObserverExtnsion
    {
        public static void Report<TNotification>(this ProgressObserver<ProgressWithPercentage<TNotification>> observer, TNotification notification, double progressPercentage)
        {
            observer.Report(new ProgressWithPercentage<TNotification>(notification, progressPercentage));
        }

        public static Task ReportComplete<TNotification>(this ProgressObserver<ProgressWithPercentage<TNotification>> observer, TNotification notification)
        {
            return observer.ReportComplete(new ProgressWithPercentage<TNotification>(notification, double.MaxValue, true));
        }

    }
}
