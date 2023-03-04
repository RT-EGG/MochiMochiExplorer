using MochiMochiExplorer.Model;

namespace MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel
{
    public partial class ApplicationViewModel
    {
        class ShowFileOpenOptionsCommandClass : CommandBase<ApplicationViewModel>
        {
            public ShowFileOpenOptionsCommandClass(ApplicationViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => true;

            public override void Execute(object? parameter)
            {
                var vm = new FileOpenOptionViewModel.FileOpenOptionViewModel();
                vm.BindModel(FileOpenOption.Instance);

                TargetApplicationBinder.Instance.Application!.ShowFileOpenOptionView(vm);
            }
        }
    }
}
