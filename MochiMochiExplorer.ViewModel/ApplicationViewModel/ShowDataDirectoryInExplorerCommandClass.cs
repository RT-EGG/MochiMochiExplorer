using MochiMochiExplorer.Model;
using System.Diagnostics;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    public partial class ApplicationViewModel
    {
        class ShowDataDirectoryInExplorerCommandClass : CommandBase<ApplicationViewModel>
        {
            public ShowDataDirectoryInExplorerCommandClass(ApplicationViewModel inViewModel) 
                : base(inViewModel)
            {}

            public override bool CanExecute(object? parameter)
                => true;

            public override void Execute(object? parameter)
            {
                var info = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = Application.ApplicationDataDirectoryPath
                };

                Process.Start(info);
            }
        }
    }
}
