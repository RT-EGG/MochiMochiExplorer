using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    public partial class ApplicationViewModel
    {
        private async Task OnFileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var pathes = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (pathes == null)
                return;

            var filepathes = pathes.Where(path => File.Exists(path));
            //await Model!.AddFiles(filepathes);
            await FileInformationList.AddFiles(filepathes);
        }

        class FileDropCommandClass : AsyncCommandBase<ApplicationViewModel>
        {
            public FileDropCommandClass(ApplicationViewModel inViewModel) 
                : base(inViewModel) 
            { }

            public override bool CanExecute(object? parameter)
                => parameter is DragEventArgs;

            protected override async Task ExecuteAsync(object? parameter)
                => await ViewModel.OnFileDrop((parameter as DragEventArgs)!);
        }
    }
}
