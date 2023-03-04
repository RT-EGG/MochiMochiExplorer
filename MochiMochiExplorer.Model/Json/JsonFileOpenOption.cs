using Newtonsoft.Json;

namespace MochiMochiExplorer.Model.Json
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
        [JsonProperty("file_open_programs")]
        public List<JsonFileOpenProgram> FileOpenPrograms { get; set; } = new List<JsonFileOpenProgram>();
    }
}
