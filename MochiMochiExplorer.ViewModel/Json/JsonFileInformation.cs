using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MochiMochiExplorer.ViewModel.Wpf.Json
{
    internal enum ColumnSortDirection
    {
        [EnumMember(Value = "ascend")]
        Ascend,
        [EnumMember(Value = "descend")]
        Descend
    }

    internal static class ColumnSortDirectionExtensions
    {
        public static ListSortDirection ToListSortDirection(this ColumnSortDirection inValue)
            => inValue switch 
            {
                ColumnSortDirection.Ascend => ListSortDirection.Ascending,
                ColumnSortDirection.Descend => ListSortDirection.Descending,
                _ => throw new NotSupportedException(),
            };

        public static ColumnSortDirection ToColumnSortDirection(this ListSortDirection inValue)
            => inValue switch
            {
                ListSortDirection.Ascending => ColumnSortDirection.Ascend,
                ListSortDirection.Descending => ColumnSortDirection.Descend,
                _ => throw new NotSupportedException(),
            };
    }

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

    internal class JsonFileInformationViewSorting
    {
        [JsonProperty("column")]
        public string Column
        { get; set; } = string.Empty;
        [JsonProperty("direction")]
        public ColumnSortDirection Direction
        { get; set; } = ColumnSortDirection.Ascend;
    }

    internal class JsonFileInformationViewer
    {
        [JsonProperty("columns")]
        public Dictionary<FileInformationViewColumnType, JsonFileInformationListViewColumn> Columns
        { get; set; } = new Dictionary<FileInformationViewColumnType, JsonFileInformationListViewColumn>();

        [JsonProperty("sorting")]
        public JsonFileInformationViewSorting? Sorting
        { get; set; } = null;
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
