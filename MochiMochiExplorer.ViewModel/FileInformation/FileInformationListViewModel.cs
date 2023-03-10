using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Utility.Wpf;
using CsUtility = Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    using ModelClass = Model.FileInformationList;

    public partial class FileInformationListViewModel : ViewModelBase<ModelClass>
    {
        public FileInformationListViewModel()
        {
            FileNameColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.FileName, 300.0);
            ExtensionColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.Extension, 50.0);
            FilepathColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.Filepath, 500.0, false);
            FileSizeColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.FileSize, 80.0);
            CreationTimeColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.CreationTime, 120.0);
            LastUpdateTimeColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.LastUpdateTime, 120.0);
            LastAccessTimeColumnViewModel = new FileInformationListColumnViewModel(this, FileInformationViewColumnType.LastAccessTime, 120.0);

            _collectionView = new CollectionViewSource()
            {
                IsLiveSortingRequested = true,
            };
            _collectionView.Source = _items;
            ChangeSort("FileName", ListSortDirection.Ascending);

            OpenFileCommand = new OpenFileCommandClass(this);
            CellDoubleClickCommand = new OpenFileCommandClass(this);
            ListKeyDownCommand = new ListKeyDownCommandClass(this);
            SortingCommand = new SortingCommandClass(this);

            BindingOperations.EnableCollectionSynchronization(_items, new object());
        }

        public string Name
        {
            get => Model!.Name.Value;
            set => Model!.Name.Value = value;
        }

        public ICollectionView CollectionView { get => _collectionView.View; }
        public IList SelectedItems { get; set; } = new List<FileInformationViewModel>();

        public FileInformationListColumnViewModel FileNameColumnViewModel { get; }
        public FileInformationListColumnViewModel ExtensionColumnViewModel { get; }
        public FileInformationListColumnViewModel FilepathColumnViewModel { get; }
        public FileInformationListColumnViewModel FileSizeColumnViewModel { get; }
        public FileInformationListColumnViewModel CreationTimeColumnViewModel { get; }
        public FileInformationListColumnViewModel LastUpdateTimeColumnViewModel { get; }
        public FileInformationListColumnViewModel LastAccessTimeColumnViewModel { get; }
        public IEnumerable<FileInformationListColumnViewModel> ColumnViewModels
        {
            get
            {
                yield return FileNameColumnViewModel;
                yield return ExtensionColumnViewModel;
                yield return FilepathColumnViewModel;
                yield return FileSizeColumnViewModel;
                yield return CreationTimeColumnViewModel;
                yield return LastUpdateTimeColumnViewModel;
                yield return LastAccessTimeColumnViewModel;
            }
        }

        // command
        public ICommand OpenFileCommand { get; }
        public ICommand CellDoubleClickCommand { get; }
        public ICommand ListKeyDownCommand { get; }
        public ICommand SortingCommand { get; }

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
                            if (!_reservedApplyAndFilter)
                            {
                                _reservedApplyAndFilter = true;
                                BackgroundTaskQueue.Instance.AddTask(new BackgroundTask()
                                {
                                    Async = false,
                                    Method = () =>
                                    {
                                        ApplySort();
                                        _reservedApplyAndFilter = false;
                                    }
                                });
                            }
                        }
                    }
            ));
            RegisterPropertyNotification(inModel.Name, nameof(Name));
        }

        private IEnumerable<T> GetSelectedItems<T>()
            => SelectedItems.OfType<T>();

        private CsUtility.ReactiveCollection<FileInformationViewModel> _items = new CsUtility.ReactiveCollection<FileInformationViewModel>();
        private CollectionViewSource _collectionView;
        private SortDescription? _sortDescription;
        private IComparer? _customSortComparer = null;
        bool _reservedApplyAndFilter = false;
    }
}
