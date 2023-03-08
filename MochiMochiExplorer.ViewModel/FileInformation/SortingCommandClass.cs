using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

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
            _customSortComparer = null;
            if (_sortDescription is not null)
            {
                var column = Enum.Parse<FileInformationViewColumnType>(_sortDescription.Value.PropertyName);
                switch (column)
                {
                    case FileInformationViewColumnType.FileSize:
                        _customSortComparer = _sortDescription.Value.Direction switch
                        {
                            ListSortDirection.Ascending => Comparer<FileInformationViewModel>.Create((left, right) => (int)(left.FileSizeValue - right.FileSizeValue)),
                            ListSortDirection.Descending => Comparer<FileInformationViewModel>.Create((left, right) => (int)(right.FileSizeValue - left.FileSizeValue)),
                            _ => throw new InvalidEnumArgumentException()
                        };
                        break;
                    default:
                        break;
                }
            }
            ApplySort();
        }

        internal event EventHandler<SortChangedEventArgs>? SortChanged;

        private void ApplySort()
        {
            _collectionView.SortDescriptions.Clear();
            if (_collectionView.View is ListCollectionView view && _customSortComparer is not null)
                view.CustomSort = _customSortComparer;
            if (_sortDescription is not null && _customSortComparer is null)
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
