using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public partial class FileInformationListViewModel
    {
        public class SortChangedEventArgs : EventArgs
        {
           public SortChangedEventArgs(FileInformationViewColumnType inColumn, ListSortDirection? inDirection)
            {
                Column = inColumn;
                Direction = inDirection;
            }

            public readonly FileInformationViewColumnType Column;
            public readonly ListSortDirection? Direction;
        }

        private void ChangeSort(string inPropertyName, ListSortDirection? inDirection)
        {
            _sortDescription = inDirection is null ? null : new SortDescription(inPropertyName, inDirection.Value);
            ApplySort();
        }

        internal event EventHandler<SortChangedEventArgs>? SortChanged;

        private void ApplySort()
        {
            _collectionView.SortDescriptions.Clear();
            if (_sortDescription is not null)
                _collectionView.SortDescriptions.Add(_sortDescription.Value);
            CollectionView.Refresh();

            TargetApplicationBinder.Instance?.Application?.UiDispatcher.Invoke(() => {
                View?.InvalidateVisual();

                var args = (_sortDescription is not null) && Enum.TryParse<FileInformationViewColumnType>(_sortDescription.Value.PropertyName, out var column)
                    ? new SortChangedEventArgs(column, _sortDescription.Value.Direction)
                    : new SortChangedEventArgs(FileInformationViewColumnType.FileName, null);
                SortChanged?.Invoke(this, args);
            });
        }

        class SortingCommandClass : ReactiveCommandBase<FileInformationListViewModel, DataGridSortingEventArgs>
        {
            public SortingCommandClass(FileInformationListViewModel inViewModel) 
                : base(inViewModel)
            {
                this.Subscribe(args => 
                    ViewModel.ChangeSort(args.Column.SortMemberPath, args.Column.SortDirection)
                );
            }
        }
    }
}
