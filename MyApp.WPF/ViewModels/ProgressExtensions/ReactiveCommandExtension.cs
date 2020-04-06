using MyApp.WPF.Models.ProgressExtensions;
using ReactiveUI;
using System;
using System.Reactive;

namespace MyApp.WPF.ViewModels.ProgressExtensions
{
    public static class ReactiveCommandExtension
    {
        public static IDisposable SubscribeProgress<TNotification>(this ReactiveCommand<Unit, ProgressWithPercentage<TNotification>> command, Action<ProgressWithPercentage<TNotification>> onProgress)
        {
            return command.Subscribe(new ProgressReceiver<ProgressWithPercentage<TNotification>>(onProgress));
        }

        private class ProgressReceiver<TNotification> : IObserver<TNotification>
        {
            protected Action<TNotification> ReceivedAction { get; }

            internal ProgressReceiver(Action<TNotification> onProgressReceived)
            {
                ReceivedAction = onProgressReceived;
            }

            public void OnNext(TNotification value)
            {
                ReceivedAction.Invoke(value);
            }

            public void OnError(Exception error)
            {
            }
            public void OnCompleted()
            {
            }
        }
    }
}
