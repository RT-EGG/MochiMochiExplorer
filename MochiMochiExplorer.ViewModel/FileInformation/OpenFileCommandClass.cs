using System;
using System.Collections;
using System.Linq;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public partial class FileInformationListViewModel
    {
        class OpenFileCommandClass : CommandBase<FileInformationListViewModel>
        {
            public OpenFileCommandClass(FileInformationListViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => parameter is IEnumerable items
                && items.OfType<object>().All(item => item is FileInformationViewModel);

            public override void Execute(object? parameter)
            {
                if (!(parameter is IEnumerable items))
                    throw new InvalidProgramException();

                items.OfType<FileInformationViewModel>().ForEach(item => item.OpenFile());
            }
        }
    }
}
