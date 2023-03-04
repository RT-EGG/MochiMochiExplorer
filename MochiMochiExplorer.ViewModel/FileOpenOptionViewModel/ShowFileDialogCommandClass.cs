using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;

namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenProgramViewModel
    {
        class ShowFileDialogCommandClass : CommandBase<FileOpenProgramViewModel>
        {
            public ShowFileDialogCommandClass(FileOpenProgramViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => parameter is Window;

            public override void Execute(object? parameter)
            {
                var dialog = new CommonOpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filters.Add(new CommonFileDialogFilter("すべてのファイル", "*.*"));

                if (dialog.ShowDialog((parameter as Window)!) == CommonFileDialogResult.Ok)
                    ViewModel.ProgramFilepath = dialog.FileName;
            }
        }
    }
}
