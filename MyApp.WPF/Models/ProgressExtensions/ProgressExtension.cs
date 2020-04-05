using System;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public static class ProgressExtension
    {
        public static void ReportWithPercentage<TNotification>(this IProgress<WithPercentage<TNotification>> progress, TNotification notification, double percentage)
        {
            progress.Report(new WithPercentage<TNotification>(notification, percentage));
        }
    }
}
