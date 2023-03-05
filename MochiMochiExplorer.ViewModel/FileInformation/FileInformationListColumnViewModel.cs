using Reactive.Bindings;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public class FileInformationListColumnViewModel : ViewModelBase
    {
        public FileInformationListColumnViewModel(FileInformationViewColumnType inType, double inWidth=100.0, bool inIsVisible=true)
        {
            ColumnType = inType;
            _width = new ReactiveProperty<DataGridLength>(inWidth);
            _visibility = new ReactiveProperty<Visibility>(inIsVisible ? Visibility.Visible : Visibility.Hidden);

            RegisterPropertyNotification(_width, nameof(Width));
            RegisterPropertyNotification(_visibility, nameof(Visibility), nameof(IsVisible));

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

        public bool IsVisible => Visibility == Visibility.Visible;

        private readonly ReactiveProperty<DataGridLength> _width;
        private readonly ReactiveProperty<Visibility> _visibility;

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

        internal void Import(Json.JsonFileInformationListViewColumn inJson)
        {
            Width = inJson.Width;
            Visibility = inJson.Visible ? Visibility.Visible : Visibility.Hidden;
        }

        internal Json.JsonFileInformationListViewColumn Export()
        {
            return new Json.JsonFileInformationListViewColumn
            {
                Width = Width.Value,
                Visible = IsVisible
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
