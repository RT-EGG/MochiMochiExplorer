using System;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public enum FileInformationViewColumnType
    {
        None = 0,
        FileName = 1 << 0,
        Extension = 1 << 1,
        Filepath = 1 << 2,
        FileSize = 1 << 3,
        CreationTime = 1 << 4,
        LastUpdateTime = 1 << 5,
        LastAccessTime = 1 << 6,
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
