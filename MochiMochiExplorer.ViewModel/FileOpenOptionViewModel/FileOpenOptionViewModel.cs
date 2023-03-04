using MochiMochiExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility;
using Utility.Wpf;

namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenOptionViewModel : ViewModelBase<FileOpenOption>
    {
        public FileOpenOptionViewModel()
        {
            AddNewOpenProgramItemCommand = new AddNewOpenProgramItemCommandClass(this);
            ValidationExtensionCommand = new ValidationExtensionCommandClass(this);
            WindowClosingCommand = new WindowClosingCommandClass(this);
            CloseWindowCommand = new CloseWindowCommandClass(this);
        }

        public ICommand AddNewOpenProgramItemCommand { get; } 
        public ICommand ValidationExtensionCommand { get; }
        public ICommand WindowClosingCommand { get; }
        public ICommand CloseWindowCommand { get; }

        public IEnumerable<FileOpenProgramViewModel> FileOpenPrograms => _fileOpenPrograms;
        public bool CanSave => _fileOpenPrograms.All(item => !item.HasErrors);
        public bool Closable => CanSave;
        public string NonClosableReason
            => string.Join(
                Environment.NewLine,
                _fileOpenPrograms
                    .Select(item => item.GetErrors())
                    .SelectMany(item => item)
                    .Where(item => !string.IsNullOrWhiteSpace(item))
            );

        public void Save()
            => Model?.SaveFile();

        protected override void BindModelProperties(FileOpenOption inModel)
        {
            base.BindModelProperties(inModel);

            inModel.FileOpenPrograms.Synchronize(
                _fileOpenPrograms, 
                items => items.Select(item =>
                {
                    var newItem = new FileOpenProgramViewModel(_fileOpenPrograms, item);
                    newItem.ErrorsChanged += (sender, args)
                        => FirePropertyChanged(
                            nameof(CanSave),
                            nameof(Closable),
                            nameof(NonClosableReason)
                        );
                    return newItem;
                }),
                new ObservableCollectionSynchronizeOption
                {
                    DisposeOnRemove = true,
                    OnAfterSynced = _ => FirePropertyChanged(nameof(FileOpenPrograms)),
                }
            );
        }

        private ReactiveCollection<FileOpenProgramViewModel> _fileOpenPrograms = new ReactiveCollection<FileOpenProgramViewModel>();
    }
}
