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

            var SetupHeaders = (DataGridColumn inColumn, MenuItem inMenuItem) =>
            {
                if (!(inMenuItem.Tag is FileInformationViewColumnType tag))
                    return;

                var text = tag.ToDisplayText();
                inColumn.Header = text;
                inMenuItem.Header = text;
            };

            SetupHeaders(DataGridColumn_FileName, MenuItemVisibility_FileName);
            SetupHeaders(DataGridColumn_Extension, MenuItemVisibility_Extension);
            SetupHeaders(DataGridColumn_Filepath, MenuItemVisibility_Filepath);
            SetupHeaders(DataGridColumn_FileSize, MenuItemVisibility_FileSize);
            SetupHeaders(DataGridColumn_CreationTime, MenuItemVisibility_CreationTime);
            SetupHeaders(DataGridColumn_LastUpdateTime, MenuItemVisibility_LastUpdateTime);
            SetupHeaders(DataGridColumn_LastAccessTime, MenuItemVisibility_LastAccessTime);

            MenuItemsVisibility = new MenuItem[]
            {
                MenuItemVisibility_FileName,
                MenuItemVisibility_Extension,
                MenuItemVisibility_Filepath,
                MenuItemVisibility_FileSize,
                MenuItemVisibility_CreationTime,
                MenuItemVisibility_LastUpdateTime,
                MenuItemVisibility_LastAccessTime,
            };
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var subComponent = FindVisualParentAsDataGridSubComponent((e.OriginalSource as DependencyObject)!);
            if (subComponent is DataGridColumnHeader)
            {
                MenuItemsVisibility.ForEach(item => item.Visibility = Visibility.Visible);
            }
            else
            {
                MenuItemsVisibility.ForEach(item => item.Visibility = Visibility.Collapsed);
            }
            
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is FileInformationListViewModel newContext))
                throw new InvalidOperationException();

            if (DataGridColumn_FileName is null)
                return;

            var visibilityConverter = new FileInformationViewColumnTypeToVisibilityConverter();
            var checkedConverter = new FileInformationViewColumnTypeToBoolConverter();
            var SetupDataGridColumnAndMenuItem = (DataGridColumn inColumn, MenuItem inMenuItem) =>
            {
                if (!(inMenuItem.Tag is FileInformationViewColumnType type))
                    return;

                var binding = new Binding("VisibleColumns") { Source = newContext, Converter = visibilityConverter, ConverterParameter = type };
                BindingOperations.SetBinding(inColumn, DataGridColumn.VisibilityProperty, binding);

                binding = new Binding("VisibleColumns") { Source = newContext, Converter = checkedConverter, ConverterParameter = type };
                BindingOperations.SetBinding(inMenuItem, MenuItem.IsCheckedProperty, binding);

                binding = new Binding("ToggleColumnVisibilityCommand") { Source = newContext };
                BindingOperations.SetBinding(inMenuItem, MenuItem.CommandProperty, binding);

                inMenuItem.CommandParameter = type;
            };

            SetupDataGridColumnAndMenuItem(DataGridColumn_FileName, MenuItemVisibility_FileName);
            SetupDataGridColumnAndMenuItem(DataGridColumn_Extension, MenuItemVisibility_Extension);
            SetupDataGridColumnAndMenuItem(DataGridColumn_Filepath, MenuItemVisibility_Filepath);
            SetupDataGridColumnAndMenuItem(DataGridColumn_FileSize, MenuItemVisibility_FileSize);
            SetupDataGridColumnAndMenuItem(DataGridColumn_CreationTime, MenuItemVisibility_CreationTime);
            SetupDataGridColumnAndMenuItem(DataGridColumn_LastUpdateTime, MenuItemVisibility_LastUpdateTime);
            SetupDataGridColumnAndMenuItem(DataGridColumn_LastAccessTime, MenuItemVisibility_LastAccessTime);
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

        private MenuItem[] MenuItemsVisibility { get; } = new MenuItem[0];
    }
}
