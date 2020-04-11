namespace MyApp.WPF.Models.ProgressExtensions
{
    public interface IProgressReporter
    {
        bool IsProgressChanged(IProgressReporter previewReport);
    }
}
