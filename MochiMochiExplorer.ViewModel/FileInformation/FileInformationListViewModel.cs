using Reactive.Bindings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Utility;
using Utility.Wpf;
using CsUtility = Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    using ModelClass = Model.FileInformationList;

    public partial class FileInformationListViewModel : ViewModelBase<ModelClass>
    {
        public FileInformationListViewModel()
        {
            ToggleColumnVisibilityCommand = new ToggleColumnVisibilityCommandClass(this);
            OpenFileCommand = new OpenFileCommandClass(this);
            CellDoubleClickCommand = new OpenFileCommandClass(this);
            ListKeyDownCommand = new ListKeyDownCommandClass(this);

            BindingOperations.EnableCollectionSynchronization(_items, new object());
        }

        public string Name
        {
            get => Model!.Name.Value;
            set => Model!.Name.Value = value;
        }

        public FileInformationViewColumnType VisibleColumns
        {
            get => _visibleColumns.Value;
            set => _visibleColumns.Value = value;
        }
        public IEnumerable<FileInformationViewModel> Items => _items;
        public IList SelectedItems { get; set; } = new List<FileInformationViewModel>();

        // command
        public ICommand ToggleColumnVisibilityCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand CellDoubleClickCommand { get; }
        public ICommand ListKeyDownCommand { get; }

        public async Task AddFiles(IEnumerable<string> inFilepathes)
        {
            if (Model is null)
                return;

            var alreadyHas = Model.Select(item => item.Filepath).ToArray();
            var pathes = inFilepathes.Where(path => !alreadyHas.Any(already => already == path)).ToArray();

            await AddItems(pathes.Select(path => new Model.FileInformation(path)));
        }

        private async Task AddItems(IEnumerable<Model.FileInformation> inItems)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher is null || Model is null)
                throw new NullReferenceException();

            var items = await Task.Run(() => new Queue<Model.FileInformation>(inItems.ToArray()));

            while (items.Any())
            {
                await dispatcher.InvokeAsync(() =>
                {
                    var time0 = DateTime.Now;
                    while (items.Any())
                    {
                        var item = items.Dequeue();
                        Model.Add(item);

                        if ((DateTime.Now - time0) > TimeSpan.FromMilliseconds(100.0))
                            break;
                    }
                });

                await Task.Delay(100);
            }
        }

        protected override void BindModelProperties(ModelClass inModel)
        {
            base.BindModelProperties(inModel);
           
            AddModelSubscription(
                inModel.Synchronize(
                    _items,
                    items => items.Select(item => new FileInformationViewModel(item)),
                    new ObservableCollectionSynchronizeOption
                    {
                        DisposeOnRemove = true,
                        OnAfterSynced = args =>
                        {
                        }
                    }
            ));
            RegisterPropertyNotification(inModel.Name, nameof(Name));
            RegisterPropertyNotification(_visibleColumns, nameof(VisibleColumns));
        }

        private IEnumerable<T> GetSelectedItems<T>()
            => SelectedItems.OfType<T>();

        private ReactiveProperty<FileInformationViewColumnType> _visibleColumns = new ReactiveProperty<FileInformationViewColumnType>(
                FileInformationViewColumnType.FileName |
                FileInformationViewColumnType.Extension |
                FileInformationViewColumnType.FileSize |
                FileInformationViewColumnType.LastUpdateTime |
                FileInformationViewColumnType.LastAccessTime
            );
        private CsUtility.ReactiveCollection<FileInformationViewModel> _items = new CsUtility.ReactiveCollection<FileInformationViewModel>();
    }
}
