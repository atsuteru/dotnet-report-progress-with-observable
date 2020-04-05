using MyApp.WPF.Models.ProgressExtensions;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WPF.Models
{
    public class OneOfRequirement : IOneOfRequirement
    {
        public Dictionary<double, Func<Task<string>>> Steps { get; }

        public OneOfRequirement()
        {
            Steps = new Dictionary<double, Func<Task<string>>>()
            {
                { 20, KneadThePizzaDoughProcess},
                { 40, CutTheIngredientsProcess},
                { 60, ToppingPizzaProcess},
                { 80, BakingPizzaProcess},
                { 100, PackagingPizzaProcess},
            };
        }

        public IObservable<string> TimeConsumingTask(CancelableProgressWithPercentage<string> progress)
        {
            return Observable
                .FromAsync(() => TimeConsumingProcessAsync(progress));
        }

        protected virtual Task<string> TimeConsumingProcessAsync(CancelableProgressWithPercentage<string> progress)
        {
            foreach (var step in Steps)
            {
                if (progress.CancellationToken.IsCancellationRequested)
                {
                    return Task.FromException<string>(new TaskCanceledException("Your pizza is canncelled"));
                }
                progress.ReportWithPercentage(step.Value.Invoke().Result, step.Key);
            }

            return Task.FromResult("Your pizza is ready!");
        }

        protected Task<string> KneadThePizzaDoughProcess()
        {
            Thread.Sleep(1000);
            return Task.FromResult("Knead the pizza dough process completed");
        }

        protected Task<string> CutTheIngredientsProcess()
        {
            Thread.Sleep(1000);
            return Task.FromResult("Cut the ingredients process completed");
        }

        protected Task<string> ToppingPizzaProcess()
        {
            Thread.Sleep(1000);
            return Task.FromResult("Topping pizza process completed");
        }

        protected Task<string> BakingPizzaProcess()
        {
            Thread.Sleep(1000);
            return Task.FromResult("baking pizza process completed");
        }

        protected Task<string> PackagingPizzaProcess()
        {
            Thread.Sleep(1000);
            return Task.FromResult("Packaging pizza process completed");
        }
    }
}
