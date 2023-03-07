using MochiMochiExplorer.ViewModel.Wpf.Json;
using Reactive.Bindings;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public class FileInformationListColumnViewModel : ViewModelBase
    {
        public FileInformationListColumnViewModel(FileInformationListViewModel inParent, FileInformationViewColumnType inType, double inWidth=100.0, bool inIsVisible=true)
        {
            Parent = inParent;
            ColumnType = inType;
            _width = new ReactiveProperty<DataGridLength>(inWidth);
            _visibility = new ReactiveProperty<Visibility>(inIsVisible ? Visibility.Visible : Visibility.Hidden);
            _sorting = new ReactiveProperty<ListSortDirection?>();

            Parent.SortChanged += (sender, args)
                => _sorting.Value = (args.Column == ColumnType) && (args.Direction is not null) ? args.Direction : null;

            RegisterPropertyNotification(_width, nameof(Width));
            RegisterPropertyNotification(_visibility, nameof(Visibility), nameof(IsVisible));
            RegisterPropertyNotification(_sorting, nameof(Sorting));

            ToggleVisibilityCommand = new ToggleVisibilityCommandClass(this);
        }

        public ICommand ToggleVisibilityCommand { get; }

        public readonly FileInformationViewColumnType ColumnType;

        public string HeaderText => ColumnType.ToDisplayText();

        public DataGridLength Width
        {
            get => _width.Value;
            set => _width.Value = value;
        }

        public Visibility Visibility
        {
            get => _visibility.Value;
            set => _visibility.Value = value;
        }

        public ListSortDirection? Sorting => _sorting.Value;

        public bool IsVisible => Visibility == Visibility.Visible;

        private readonly FileInformationListViewModel Parent;
        private readonly ReactiveProperty<DataGridLength> _width;
        private readonly ReactiveProperty<Visibility> _visibility;
        private readonly ReactiveProperty<ListSortDirection?> _sorting;

        private void ToggleVisibility()
        {
            switch (Visibility)
            {
                case Visibility.Visible:
                    Visibility = Visibility.Hidden;
                    break;
                case Visibility.Collapsed:
                case Visibility.Hidden:
                    Visibility = Visibility.Visible;
                    break;
            }
        }

        internal void Import(JsonFileInformationListViewColumn inJson)
        {
            Width = inJson.Width;
            Visibility = inJson.Visible ? Visibility.Visible : Visibility.Hidden;
        }

        internal JsonFileInformationListViewColumn Export()
        {
            return new JsonFileInformationListViewColumn
            {
                Width = Width.Value,
                Visible = IsVisible,
            };
        }

        class ToggleVisibilityCommandClass : CommandBase<FileInformationListColumnViewModel>
        {
            public ToggleVisibilityCommandClass(FileInformationListColumnViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => true;

            public override void Execute(object? parameter)
                => ViewModel.ToggleVisibility();
        }
    }
}
