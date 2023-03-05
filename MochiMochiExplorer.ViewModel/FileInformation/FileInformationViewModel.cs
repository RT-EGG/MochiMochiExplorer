using MochiMochiExplorer.ViewModel.Wpf.Utility;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Windows.Media.Imaging;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    using ModelClass = Model.FileInformation;

    public class FileInformationViewModel : ViewModelBase<ModelClass>
    {
        public FileInformationViewModel(ModelClass inModel)
        {
            BindModel(inModel);

            UpdateFileInfo();
        }

        public BitmapSource? Icon => _icon.Value;
        public string FileName => Path.GetFileName(Filepath);
        public string Extension => Path.GetExtension(Filepath).ToLower();
        public string Filepath => Model!.Filepath;
        public string FileSize => FileSizeTextConverter.GetFileSizeText(_fileSize.Value);
        public string CreationTime => DateToString(_creationTime.Value);
        public string LastUpdateTime => DateToString(_lastUpdateTime.Value);
        // このアプリを使って最後にアクセスした日時
        public string LastAccessTime => DateToString(Model!.LastAccessTime.Value);

        public void OpenFile()
        {
            FileOpener.Instance.OpenFile(Filepath);
            Model!.LastAccessTime.Value = DateTime.Now;
        }

        public void RemoveFrom(IList<Model.FileInformation> inItems)
        {
            if (Model is not null)
                inItems.Remove(Model);
        }

        public void UpdateFileInfo()
        {
            BackgroundTaskQueue.Instance.AddTask(new BackgroundTask
            {
                Method = async () =>
                {
                    var info = new FileInfo(Model!.Filepath);
                    _fileSize.Value = info.Length;
                    _creationTime.Value = info.CreationTime;
                    _lastUpdateTime.Value = info.LastWriteTime;

                    _icon.Value = await FileIcons.Instance.GetAssociatedIcon(Model!.Filepath, FileIcons.IconSize.Small);
                },
                Async = true,
            });
        }

        protected override void BindModelProperties(ModelClass inModel)
        {
            base.BindModelProperties(inModel);

            RegisterPropertyNotification(_fileSize, nameof(FileSize));
            RegisterPropertyNotification(_creationTime, nameof(CreationTime));
            RegisterPropertyNotification(_lastUpdateTime, nameof(LastUpdateTime));
            RegisterPropertyNotification(_icon, nameof(Icon));

            RegisterPropertyNotification(inModel.LastAccessTime, nameof(LastAccessTime));
        }

        private static string DateToString(DateTime inValue)
            => inValue.ToString("yyyy/MM/dd HH:mm");

        private ReactiveProperty<BitmapSource?> _icon = new ReactiveProperty<BitmapSource?>();
        private ReactiveProperty<long> _fileSize = new ReactiveProperty<long>(0);
        private ReactiveProperty<DateTime> _creationTime = new ReactiveProperty<DateTime>(Scheduler.Immediate);
        private ReactiveProperty<DateTime> _lastUpdateTime = new ReactiveProperty<DateTime>();

        private class FileSizeTextConverter
        {
            public static string GetFileSizeText(long inFileSize)
            {
                int unit = 1024;
                float size = inFileSize;
                int index = 0;

                while (size >= unit)
                {
                    size /= unit;
                    index++;
                }

                return $"{size.ToString("#,##0.##")}{_suffix[index]}";
            }

            private static readonly string[] _suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        }
    }
}
