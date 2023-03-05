using MochiMochiExplorer.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MochiMochiExplorer.ViewModel.Wpf.Json
{
    internal class JsonFileOpenProgram
    {
        [JsonProperty("target_extension")]
        public string TargetExtension { get; set; } = string.Empty;
        [JsonProperty("program_filepath")]
        public string ProgramFilepath { get; set; } = string.Empty;
    }

    internal class JsonFileOpenOption
    {
        public JsonFileOpenOption()
        { }

        public JsonFileOpenOption(FileOpenOption inSource)
        {
            FileOpenPrograms = new List<JsonFileOpenProgram>(inSource.FileOpenPrograms.Select(p =>
                new JsonFileOpenProgram
                {
                    TargetExtension = p.TargetExtension.Value,
                    ProgramFilepath = p.ProgramFilepath.Value
                }
            ));
        }

        public void ImportTo(FileOpenOption inDst)
        {
            inDst.FileOpenPrograms.Clear();
            FileOpenPrograms.ForEach(src =>
                inDst.FileOpenPrograms.Add(new FileOpenProgram(src.TargetExtension, src.ProgramFilepath))
            );
        }

        [JsonProperty("file_open_programs")]
        public List<JsonFileOpenProgram> FileOpenPrograms { get; set; } = new List<JsonFileOpenProgram>();        
    }
}
