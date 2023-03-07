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
            LoadProject();

            if (Directory.Exists(FileInformationListDirectoryPath))
            {
                foreach (var filepath in Directory.EnumerateFiles(FileInformationListDirectoryPath, "*.json"))
                {
                    var newList = new FileInformationListViewModel();
                    newList.BindModel(Model!.FileInformationList.Value);

                    FileInformationList = newList;
                    FirePropertyChanged(nameof(FileInformationList));

                    await newList.LoadFile(filepath);

                    break;
                }
            }
        }
    }
}
