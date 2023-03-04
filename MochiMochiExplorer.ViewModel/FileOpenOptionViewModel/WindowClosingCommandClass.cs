using System.ComponentModel;

namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenOptionViewModel
    {
        class WindowClosingCommandClass : CommandBase<FileOpenOptionViewModel>
        {
            public WindowClosingCommandClass(FileOpenOptionViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => parameter is CancelEventArgs;

            public override void Execute(object? parameter)
            {
                var args = (parameter as CancelEventArgs)!;

                if (!ViewModel.Closable)
                    args.Cancel = true;
                else
                    ViewModel.Save();
            }
        }
    }
}
