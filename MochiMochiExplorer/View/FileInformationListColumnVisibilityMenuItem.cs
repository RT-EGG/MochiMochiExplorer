using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MochiMochiExplorer.View
{
    internal class FileInformationListColumnVisibilityMenuItem : MenuItem
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "Column")
            {
                ReBind();
            }
        }

        public FileInformationListColumnViewModel Column
        {
            get => (FileInformationListColumnViewModel)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }

        private void ReBind()
        {
            var column = Column;
            Header = column.HeaderText;

            BindingOperations.SetBinding(this, IsCheckedProperty, new Binding("IsVisible") {
                Source = column, Mode = BindingMode.OneWay,
            });
            BindingOperations.SetBinding(this, CommandProperty, new Binding("ToggleVisibilityCommand") {
                Source = column, Mode = BindingMode.OneWay, 
            });
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
                "Column",
                typeof(FileInformationListColumnViewModel),
                typeof(FileInformationListColumnVisibilityMenuItem)
        );
    }
}
