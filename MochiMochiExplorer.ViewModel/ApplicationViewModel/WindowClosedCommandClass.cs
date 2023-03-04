using System.IO;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    public partial class ApplicationViewModel
    {
        public void SaveProject()
        {
            FileInformationList.SaveFile(
                Path.Join(FileInformationListDirectoryPath, $"{FileInformationList.Name}.json")
            );
        }

        class WindowClosedCommandClass : CommandBase<ApplicationViewModel>
        {
            public WindowClosedCommandClass(ApplicationViewModel inViewModel) : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => true;

            public override void Execute(object? parameter)
                => ViewModel.SaveProject();
        }
    }
}
