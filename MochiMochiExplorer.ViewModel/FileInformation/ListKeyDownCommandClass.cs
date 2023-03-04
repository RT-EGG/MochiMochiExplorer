using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileInformation
{
    public partial class FileInformationListViewModel
    {
        private void RemoveItems()
        {
            if (Model is not null)
                GetSelectedItems<FileInformationViewModel>()
                    .ToArray()
                    .ForEach(item => item.RemoveFrom(Model));
        }

        class ListKeyDownCommandClass : ReactiveCommandBase<FileInformationListViewModel, KeyEventArgs>
        {
            public ListKeyDownCommandClass(FileInformationListViewModel inViewModel)
                : base(inViewModel)
            {
                this.Subscribe(args => OnKeyDown(args));
            }

            private void OnKeyDown(KeyEventArgs inArgs)
            {
                switch (inArgs.Key)
                {
                    case Key.Delete:
                        ViewModel.RemoveItems();
                        break;
                }
            }
        }
    }
}
