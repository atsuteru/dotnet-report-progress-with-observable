using MyApp.WPF.Models.ProgressExtensions;
using System;

namespace MyApp.WPF.Models
{
    public interface IOneOfRequirement
    {
        IObservable<string> TimeConsumingTask(IProgress<WithPercentage<string>> progress);
    }
}
