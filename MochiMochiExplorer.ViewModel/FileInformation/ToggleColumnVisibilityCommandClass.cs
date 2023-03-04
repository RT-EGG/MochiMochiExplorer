using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public partial class FileInformationListViewModel
    {
        private void ToggleComulnVisibility(FileInformationViewColumnType inType)
        {
            if (VisibleColumns.HasFlag(inType))
                VisibleColumns &= ~inType;
            else
                VisibleColumns |= inType;
        }

        class ToggleColumnVisibilityCommandClass : CommandBase<FileInformationListViewModel>
        {
            public ToggleColumnVisibilityCommandClass(FileInformationListViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => parameter is FileInformationViewColumnType;

            public override void Execute(object? parameter)
                => ViewModel.ToggleComulnVisibility((FileInformationViewColumnType)parameter!);
        }
    }
}
