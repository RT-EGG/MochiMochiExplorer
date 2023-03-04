using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    public partial class ApplicationViewModel
    {
        private async Task OnLoad()
        {
            if (Directory.Exists(FileInformationListDirectoryPath))
            {
                foreach (var filepath in Directory.EnumerateFiles(FileInformationListDirectoryPath, "*.json"))
                {
                    var newList = new FileInformationListViewModel();
                    newList.BindModel(Model.FileInformationList.Value);

                    await newList.LoadFile(filepath);

                    FileInformationList = newList;
                    FirePropertyChanged(nameof(FileInformationList));

                    break;
                }
            }

            ContentRenderedCommand = new NullCommandClass<ApplicationViewModel>(this);
            FirePropertyChanged(nameof(ContentRenderedCommand));
        }

        public static string ApplicationDataDirectoryPath
            => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RT-EGG", "MochiMochiExplorer");
        private static string FileInformationListDirectoryPath
            => Path.Join(ApplicationDataDirectoryPath, "FileInformations");

        class OnLoadCommandClass : AsyncCommandBase<ApplicationViewModel>
        {
            public OnLoadCommandClass(ApplicationViewModel inViewModel) : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => true;

            protected override async Task ExecuteAsync(object? parameter)
            {

            }
                //=> await ViewModel.OnLoad();
                
        }
    }
}
