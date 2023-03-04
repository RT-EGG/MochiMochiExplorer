using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using System;
using System.Windows.Input;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    using ModelClass = Model.Application;

    public partial class ApplicationViewModel : ViewModelBase<ModelClass>
    {
        public ApplicationViewModel()
        {
            FileDropCommand = new FileDropCommandClass(this);
            ContentRenderedCommand = new OnLoadCommandClass(this);
            ShowFileOpenOptionsCommand = new ShowFileOpenOptionsCommandClass(this);
            ShowDataDirectoryInExplorerCommand = new ShowDataDirectoryInExplorerCommandClass(this);
            WindowClosedCommand = new WindowClosedCommandClass(this);

            BackgroundTaskQueue.Instance.Start();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                BackgroundTaskQueue.Instance.Stop();
            }
        }

        // commands
        public ICommand FileDropCommand { get; }
        public ICommand ContentRenderedCommand { get; set; }
        public ICommand ShowFileOpenOptionsCommand { get; }
        public ICommand ShowDataDirectoryInExplorerCommand { get; }
        public ICommand WindowClosedCommand { get; }

        // children
        public FileInformationListViewModel FileInformationList
        { get; private set; } = new FileInformationListViewModel();

        protected override void BindModelProperties(ModelClass inModel)
        {
            base.BindModelProperties(inModel);

            AddModelSubscription(inModel.FileInformationList.Subscribe(value =>
            {
                FileInformationList.BindModel(value);
                FirePropertyChanged(nameof(FileInformationList));
            }));

            View?.Dispatcher.BeginInvoke(async () =>
            {
                await OnLoad();
            });
        }
    }
}
