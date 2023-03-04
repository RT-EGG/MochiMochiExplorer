using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MochiMochiExplorer.View
{
    internal class FileInformationDataTextColumn : DataGridTextColumn
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "ViewModel")
            {
                ReBind();
            }
        }

        public FileInformationListColumnViewModel ViewModel
        {
            get => (FileInformationListColumnViewModel)GetValue(ViewModelProperty);
            set
            {
                SetValue(ViewModelProperty, value);
                ReBind();
            }
        }

        private void ReBind()
        {
            var vm = ViewModel;

            BindingOperations.SetBinding(this, HeaderProperty, new Binding("HeaderText") {
                Source = vm, Mode = BindingMode.OneWay
            });

            BindingOperations.SetBinding(this, WidthProperty, new Binding("Width") { 
                Source = vm, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged 
            });
            BindingOperations.SetBinding(this, VisibilityProperty, new Binding("Visibility") {
                Source = vm,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            IsReadOnly = true;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
                "ViewModel",
                typeof(FileInformationListColumnViewModel),
                typeof(FileInformationDataTextColumn)
        );
    }
}
