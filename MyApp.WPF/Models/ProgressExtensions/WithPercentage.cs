namespace MyApp.WPF.Models.ProgressExtensions
{
    public class WithPercentage<TNotification>
    {
        public double Percentage { get; }

        public TNotification Notification { get; }

        public WithPercentage(TNotification notification, double percentage)
        {
            Percentage = percentage;
            Notification = notification;
        }
    }
}
