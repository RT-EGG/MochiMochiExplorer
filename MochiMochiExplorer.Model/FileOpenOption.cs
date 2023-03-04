using MochiMochiExplorer.Model.Json;
using Newtonsoft.Json;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MochiMochiExplorer.Model
{
    public class FileOpenProgram
    {
        public FileOpenProgram(string inTargetExtension, string inProgramFilepath)
        {
            TargetExtension.Value = inTargetExtension;
            ProgramFilepath.Value = inProgramFilepath;
        }

        public ReactiveProperty<string> TargetExtension { get; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> ProgramFilepath { get; } = new ReactiveProperty<string>(string.Empty);
    }

    public class FileOpenOption
    {
        private FileOpenOption()
        {
            LoadFile();
        }

        public static FileOpenOption Instance { get; } = new FileOpenOption();

        public void LoadFile()
        {
            var jsonObject = new JsonFileOpenOption();
            if (File.Exists(SaveFilepath))
            {
                using (var reader = new StreamReader(new FileStream(SaveFilepath, FileMode.Open, FileAccess.Read)))
                {
                    var source = reader.ReadToEnd();
                    jsonObject = JsonConvert.DeserializeObject<JsonFileOpenOption>(source)!;
                }
            }
            Import(jsonObject);
        }

        public void SaveFile()
        {
            Directory.CreateDirectory(Application.ApplicationDataDirectoryPath);

            var jsonObject = Export();
            using (var writer = new StreamWriter(new FileStream(SaveFilepath, FileMode.Create, FileAccess.Write)))
            {
                writer.Write(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
            }
        }

        private void Import(JsonFileOpenOption inSource)
        {
            FileOpenPrograms.Clear();
            inSource.FileOpenPrograms.ForEach(src =>
                FileOpenPrograms.Add(new FileOpenProgram(src.TargetExtension, src.ProgramFilepath))
            );
        }

        private JsonFileOpenOption Export()
            => new JsonFileOpenOption
            {
                FileOpenPrograms = new List<JsonFileOpenProgram>(
                    FileOpenPrograms.Select(src => new JsonFileOpenProgram
                    {
                        TargetExtension = src.TargetExtension.Value,
                        ProgramFilepath = src.ProgramFilepath.Value
                    })
                )
            };

        public Utility.ReactiveCollection<FileOpenProgram> FileOpenPrograms { get; } = new Utility.ReactiveCollection<FileOpenProgram>();

        public static string SaveFilepath
            => Path.Join(Application.ApplicationDataDirectoryPath, "file_open_option.json");
    }
}
