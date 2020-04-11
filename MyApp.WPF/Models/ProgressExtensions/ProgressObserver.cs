using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public class ProgressObserver<TProgressReporter> where TProgressReporter : IProgressReporter
    {
        protected IObserver<TProgressReporter> Observer { get;  }

        protected CancellationToken CancellationToken { get; }

        protected TProgressReporter PreviewReport { get; set; }

        public ProgressObserver(IObserver<TProgressReporter> observer, CancellationToken ct)
        {
            Observer = observer;
            CancellationToken = ct;
        }

        public async Task Report(TProgressReporter reporter)
        {
            if (reporter.IsProgressChanged(PreviewReport))
            {
                await Task.Delay(1);
                CancellationToken.ThrowIfCancellationRequested();
                PreviewReport = reporter;
            }
            Observer.OnNext(reporter);
        }
        public Task ReportComplete(TProgressReporter reporter)
        {
            Observer.OnNext(reporter);
            return Task.CompletedTask;
        }

        public Task ThrowException(Exception exception)
        {
            return Task.FromException(exception);
        }

        public async Task ExitIfCanceled()
        {
            await Task.Delay(1);
            if (CancellationToken.IsCancellationRequested)
            {
                await Task.FromCanceled(CancellationToken);
            }
        }
    }
}
