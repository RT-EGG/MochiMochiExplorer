using MochiMochiExplorer.Model;
using MochiMochiExplorer.ViewModel.Wpf.Json;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    public partial class ApplicationViewModel
    {
        public void LoadProject()
        {
            if (File.Exists(FileOpenOptionFilepath))
            {
                using (var reader = new StreamReader(new FileStream(FileOpenOptionFilepath, FileMode.Open, FileAccess.Read)))
                {
                    var source = reader.ReadToEnd();
                    var fileOpenOptionJsonObject = JsonConvert.DeserializeObject<JsonFileOpenOption>(source)!;

                    fileOpenOptionJsonObject.ImportTo(FileOpenOption.Instance);
                }
            }
            
        }

        public void SaveProject()
        {
            FileInformationList.SaveFile(
                Path.Join(FileInformationListDirectoryPath, $"{FileInformationList.Name}.json")
            );
        }

        public static string ApplicationDataDirectoryPath
            => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RT-EGG", "MochiMochiExplorer");
        private static string FileInformationListDirectoryPath
            => Path.Join(ApplicationDataDirectoryPath, "FileInformations");
        public static string FileOpenOptionFilepath
            => Path.Join(ApplicationDataDirectoryPath, "file_open_option.json");
    }
}
