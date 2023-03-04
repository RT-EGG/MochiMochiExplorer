namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenOptionViewModel
    {
        private void AddNewOpenProgramItem()
        {
            var newItem = new MochiMochiExplorer.Model.FileOpenProgram("", "");
            Model!.FileOpenPrograms.Add(newItem);
        }

        class AddNewOpenProgramItemCommandClass : CommandBase<FileOpenOptionViewModel>
        {
            public AddNewOpenProgramItemCommandClass(FileOpenOptionViewModel inViewModel)
                : base(inViewModel)
            { }

            public override bool CanExecute(object? parameter)
                => true;

            public override void Execute(object? parameter)
                => ViewModel.AddNewOpenProgramItem();
        }
    }
}
