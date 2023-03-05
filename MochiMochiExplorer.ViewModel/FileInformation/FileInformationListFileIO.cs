using MochiMochiExplorer.ViewModel.Wpf.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public partial class FileInformationListViewModel
    {
        public async Task LoadFile(string inFilepath)
        {
            if (!File.Exists(inFilepath))
                throw new FileNotFoundException();

            using (var reader = new StreamReader(new FileStream(inFilepath, FileMode.Open, FileAccess.Read)))
            {
                var source = reader.ReadToEnd();
                var jsonObject = JsonConvert.DeserializeObject<JsonFileInformationList>(source);
                if (jsonObject is null)
                    throw new FileLoadException();

                await Import(jsonObject);
            }
        }

        public void SaveFile(string inFilepath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(inFilepath)!);

            var jsonObject = Export();
            using (var writer = new StreamWriter(new FileStream(inFilepath, FileMode.Create, FileAccess.Write)))
            {
                writer.Write(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
            }
        }

        private async Task Import(JsonFileInformationList inSource)
        {
            ColumnViewModels.ForEach(column =>
            {
                if (inSource.Viewer.Columns.TryGetValue(column.ColumnType, out var source))
                {
                    column.Import(source);
                }
            });

            if (Model is null)
                throw new NullReferenceException();

            Model.Name.Value = inSource.Name;
            Model.Clear();
            await AddItems(inSource.Items.Select(item =>
            {
                var result = new Model.FileInformation(item.Filepath);
                result.LastAccessTime.Value = item.LastAccessTime;

                return result;
            }));
        }

        private JsonFileInformationList Export()
        {
            if (Model is null)
                throw new NullReferenceException();

            return new JsonFileInformationList
            {
                Name = Model.Name.Value,
                Viewer = new JsonFileInformationViewer
                {
                    Columns = new Dictionary<FileInformationViewColumnType, JsonFileInformationListViewColumn>(
                        ColumnViewModels.Select(column => new KeyValuePair<FileInformationViewColumnType, JsonFileInformationListViewColumn>(
                            column.ColumnType, column.Export()
                        ))
                    ),
                },
                Items = Model.Select(item => new JsonFileInformation
                {
                    Filepath = item.Filepath,
                    LastAccessTime = item.LastAccessTime.Value,
                }).ToArray(),
            };
        }
    }
}
