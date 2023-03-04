using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MochiMochiExplorer.Converter;
using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using Utility;

namespace MochiMochiExplorer.View
{
    /// <summary>
    /// FileInformationListView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileInformationListView : UserControl
    {
        public FileInformationListView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            Loaded += (_, _) => TargetApplication.Instance.AddFrameworkElement(this);
            Unloaded += (_, _) => TargetApplication.Instance.RemoveFrameworkElement(this);
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var subComponent = FindVisualParentAsDataGridSubComponent((e.OriginalSource as DependencyObject)!);
            if (subComponent is DataGridColumnHeader)
            {
                ListContextMenu.Items.OfType<MenuItem>().ForEach(item =>
                {
                    item.Visibility = (item is FileInformationListColumnVisibilityMenuItem) ? Visibility.Visible : Visibility.Collapsed;
                });
            }
            else
            {
                ListContextMenu.Items.OfType<MenuItem>().ForEach(item =>
                {
                    item.Visibility = (item is FileInformationListColumnVisibilityMenuItem) ? Visibility.Collapsed : Visibility.Visible;
                });
            }            
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DataGridCell cell))
                return;
            if (!(cell.DataContext is FileInformationViewModel file))
                return;

            if (!(DataContext is FileInformationListViewModel list))
                return;

            var parameter = new object[] { file };
            if (list.OpenFileCommand.CanExecute(parameter))
                list.OpenFileCommand.Execute(parameter);
            return;
        }

        private static DependencyObject? FindVisualParentAsDataGridSubComponent(DependencyObject inSource)
        {
            // refered
            // https://stackoverflow.com/questions/59423764/context-menu-for-specific-dynamic-column-for-datagrid-wpf
            while (inSource is not null
                && inSource is not DataGridCell
                && inSource is not DataGridColumnHeader
                && inSource is not DataGridRow)
            {
                inSource = (inSource is Visual || inSource is Visual3D)
                    ? VisualTreeHelper.GetParent(inSource)
                    : LogicalTreeHelper.GetParent(inSource);
            }

            return inSource;
        }
    }
}
