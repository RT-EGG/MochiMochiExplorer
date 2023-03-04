using System.Windows;

namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenOptionViewModel
    {
        class CloseWindowCommandClass : CommandBase<FileOpenOptionViewModel>
        {
            public CloseWindowCommandClass(FileOpenOptionViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => parameter is Window
                && ViewModel.Closable;

            public override void Execute(object? parameter)
                => (parameter as Window)!.Close();
        }
    }
}
