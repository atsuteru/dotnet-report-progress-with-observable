using System;
using System.Threading;

namespace MyApp.WPF.Models.ProgressExtensions
{
    public class CancelableProgressWithPercentage<TNotification> : Progress<WithPercentage<TNotification>>
    {
        public CancellationToken CancellationToken { get; protected set; }

        public CancelableProgressWithPercentage(Action<WithPercentage<TNotification>> handler) : base(handler)
        {
        }

        public CancelableProgressWithPercentage<TNotification> With(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }
    }
}
