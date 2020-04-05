using MyApp.WPF.Models.ProgressExtensions;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WPF.Models
{
    public class OneOfRequirement : IOneOfRequirement
    {
        public IObservable<string> TimeConsumingTask(IProgress<WithPercentage<string>> progress)
        {
            return Observable
                .FromAsync(() => TimeConsumingProcessAsync(progress));
        }

        protected virtual Task<string> TimeConsumingProcessAsync(IProgress<WithPercentage<string>> progress)
        {
            Thread.Sleep(1000);
            progress.ReportWithPercentage("Knead the pizza dough process completed", 20);

            Thread.Sleep(1000);
            progress.ReportWithPercentage("Cut the ingredients process completed", 40);

            Thread.Sleep(1000);
            progress.ReportWithPercentage("Topping pizza process completed", 60);

            Thread.Sleep(1000);
            progress.ReportWithPercentage("baking pizza process completed", 80);

            Thread.Sleep(1000);
            progress.ReportWithPercentage("Packaging pizza was completed", 100);

            return Task.FromResult("Your pizza is ready!");
        }
    }
}
