using System;
using System.Runtime.Serialization;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public enum FileInformationViewColumnType
    {
        [EnumMember(Value = "none")]
        None = 0,
        [EnumMember(Value = "filename")]
        FileName,
        [EnumMember(Value = "extension")]
        Extension,
        [EnumMember(Value = "filepath")]
        Filepath,
        [EnumMember(Value = "filesize")]
        FileSize,
        [EnumMember(Value = "creation_time")]
        CreationTime,
        [EnumMember(Value = "last_update_time")]
        LastUpdateTime,
        [EnumMember(Value = "last_access_time")]
        LastAccessTime,
    }

    public static class FileInformationViewColumnTypeExtensions
    {
        public static string ToDisplayText(this FileInformationViewColumnType inValue)
            => inValue.ToDisplayText(Internationalizer.Instance!);

        public static string ToDisplayText(this FileInformationViewColumnType inValue, IInternationalizer inInternationalizer)
        {
            return inInternationalizer.Internationalize(inValue switch
            {
                FileInformationViewColumnType.None => "",
                FileInformationViewColumnType.FileName => "ファイル名",
                FileInformationViewColumnType.Extension => "拡張子",
                FileInformationViewColumnType.Filepath => "完全パス",
                FileInformationViewColumnType.FileSize => "サイズ",
                FileInformationViewColumnType.CreationTime => "作成日時",
                FileInformationViewColumnType.LastUpdateTime => "最終更新日時",
                FileInformationViewColumnType.LastAccessTime => "最終アクセス日時",
                _ => throw new ArgumentException()
            });
        }
    }
}
