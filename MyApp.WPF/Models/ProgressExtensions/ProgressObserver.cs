using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public class ProgressObserver<TProgressReporter>
    {
        protected IObserver<TProgressReporter> Observer { get;  }

        protected CancellationToken CancellationToken { get; }

        public ProgressObserver(IObserver<TProgressReporter> observer, CancellationToken ct)
        {
            Observer = observer;
            CancellationToken = ct;
        }

        public void Report(TProgressReporter reporter)
        {
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
