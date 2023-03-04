using Newtonsoft.Json;
using System;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation.Json
{
    internal class JsonFileInformation
    {
        [JsonProperty("filepath")]
        public string Filepath
        { get; set; } = string.Empty;

        [JsonProperty("last_access_time")]
        public DateTime LastAccessTime
        { get; set; } = new DateTime();
    }

    internal class JsonFileInformationList
    {
        [JsonProperty("name")]
        public string Name
        { get; set; } = string.Empty;

        [JsonProperty("items")]
        public JsonFileInformation[] Items
        { get; set; } = new JsonFileInformation[0];
    }
}
