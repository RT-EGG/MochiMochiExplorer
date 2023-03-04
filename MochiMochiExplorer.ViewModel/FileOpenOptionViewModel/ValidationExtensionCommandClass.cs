using System.Linq;
using Utility.Wpf.CustomElement;

namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenOptionViewModel
    {
        private string GetChangeExtensionError(FileOpenProgramViewModel inTarget, string inNewExtension)
        {
            if (_fileOpenPrograms.Where(item => item != inTarget).Any(item => item.Extension == inNewExtension))
                return $"拡張子 {inNewExtension} の設定はすでに存在します。";

            return string.Empty;
        }

        class ValidationExtensionCommandClass : CommandBase<FileOpenOptionViewModel>
        {
            public ValidationExtensionCommandClass(FileOpenOptionViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => parameter is CustomDataGridTextColumn.ValidationCommandArgs;

            public override void Execute(object? parameter)
            {
                var args = (parameter as CustomDataGridTextColumn.ValidationCommandArgs)!;
                args.Error = ViewModel.GetChangeExtensionError((args.DataContext as FileOpenProgramViewModel)!, args.Text);
            }
        }
    }
}
