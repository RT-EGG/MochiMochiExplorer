using System;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public enum FileInformationViewColumnType
    {
        None = 0,
        FileName,
        Extension,
        Filepath,
        FileSize,
        CreationTime,
        LastUpdateTime,
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
