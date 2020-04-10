using MyApp.WPF.Models.ProgressExtensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WPF.Models
{
    public class OneOfRequirement
    {
        protected struct TimeConsumingStep
        {
            public double ProgressPercentage { get; }
            public Func<Task<string>> Process { get; }

            public TimeConsumingStep(double progressPercentage, Func<Task<string>> process)
            {
                ProgressPercentage = progressPercentage;
                Process = process;
            }

            public Task<string> InvokeAsync()
            {
                return Process.Invoke();
            }
        }

        protected List<TimeConsumingStep> Steps { get; }

        public OneOfRequirement()
        {
            Steps = new List<TimeConsumingStep>()
            {
                new TimeConsumingStep(20, KneadThePizzaDoughProcess),
                new TimeConsumingStep(40, CutTheIngredientsProcess),
                new TimeConsumingStep(60, ToppingPizzaProcess),
                new TimeConsumingStep(80, BakingPizzaProcess),
                new TimeConsumingStep(100, PackagingPizzaProcess),
            };
        }

        public IObservable<ProgressWithPercentage<string>> TimeConsumingTask(OneOfRequest request)
        {
            return ProgressWithPercentage<string>.CreateObservableFromAsync(TimeConsumingProcessAsync, request);
        }

        protected virtual async Task TimeConsumingProcessAsync(ProgressObserver<ProgressWithPercentage<string>> progress, OneOfRequest request)
        {
            foreach (var step in Steps)
            {
                await progress.ExitIfCanceled();

                progress.Report(step.InvokeAsync().Result, step.ProgressPercentage);
            }

            //await progress.ThrowException(new Exception("Your pizza has been burnt!!"));

            await progress.ReportComplete("Your pizza is ready!");
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
