using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MochiMochiExplorer.ViewModel.Wpf.Json
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

    internal class JsonFileInformationListViewColumn
    {
        [JsonProperty("visible")]
        public bool Visible
        { get; set; } = true;

        [JsonProperty("width")]
        public double Width
        { get; set; } = 100.0;
    }

    internal class JsonFileInformationViewer
    {
        [JsonProperty("columns")]
        public Dictionary<FileInformationViewColumnType, JsonFileInformationListViewColumn> Columns
        { get; set; } = new Dictionary<FileInformationViewColumnType, JsonFileInformationListViewColumn>();
    }

    internal class JsonFileInformationList
    {
        [JsonProperty("name")]
        public string Name
        { get; set; } = string.Empty;

        [JsonProperty("viewer")]
        public JsonFileInformationViewer Viewer
        { get; set; } = new JsonFileInformationViewer();

        [JsonProperty("items")]
        public JsonFileInformation[] Items
        { get; set; } = new JsonFileInformation[0];
    }
}
