using Reactive.Bindings;

namespace MochiMochiExplorer.Model
{
    public class Application
    {
        //public async Task Initialize()
        //{
        //    if (Directory.Exists(FileInformationListDirectoryPath))
        //    {
        //        var newList = new FileInformationList();
        //        FileInformationList.Value = newList;
        //        await Task.Run(() =>
        //        {
        //            foreach (var filepath in Directory.EnumerateFiles(FileInformationListDirectoryPath, "*.json"))
        //            {
        //                if (!File.Exists(filepath))
        //                    continue;

        //                try
        //                {
        //                    newList.LoadFile(filepath);
        //                }
        //                catch (FileLoadException)
        //                {
        //                    continue;
        //                }

        //                FileInformationList.Value = newList;
        //                break;
        //            }
        //        });
        //    }
        //}

        public static string ApplicationDataDirectoryPath
            => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RT-EGG", "MochiMochiExplorer");
        private static string FileInformationListDirectoryPath
            => Path.Join(ApplicationDataDirectoryPath, "FileInformations");

        public ReactiveProperty<FileInformationList> FileInformationList 
            { get; } = new ReactiveProperty<FileInformationList>(new Model.FileInformationList("List"));
    }
}
