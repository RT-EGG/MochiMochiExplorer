using MochiMochiExplorer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf.FileOpenOptionViewModel
{
    public partial class FileOpenProgramViewModel : ViewModelBase<FileOpenProgram>
    {
        public FileOpenProgramViewModel(IList<FileOpenProgramViewModel> inOwner, FileOpenProgram inModel)
        {
            _owner = inOwner;
            _validationErrors.ValidationErrorChanged += args =>
            {
                var a = new DataErrorsChangedEventArgs(args.Key);
                ErrorsChanged?.Invoke(this, a);
            };

            ShowFileDialogCommand = new ShowFileDialogCommandClass(this);

            BindModel(inModel);
        }

        public string Extension
        {
            get => Model!.TargetExtension.Value;
            set => Model!.TargetExtension.Value = value;
        }

        public string ProgramFilepath
        {
            get => Model!.ProgramFilepath.Value;
            set => Model!.ProgramFilepath.Value = value;
        }

        public ICommand ShowFileDialogCommand { get; }

        // INotifyDataErrorInfo
        public bool HasErrors => _validationErrors.HasAnyError;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public IEnumerable GetErrors(string? propertyName)
            => _validationErrors.Where(item => item.Key == propertyName);
        public IEnumerable<string> GetErrors()
            => _validationErrors.Select(item => item.Message);

        protected override void BindModelProperties(FileOpenProgram inModel)
        {
            base.BindModelProperties(inModel);

            RegisterPropertyNotification(inModel.TargetExtension, nameof(Extension));
            RegisterPropertyNotification(inModel.ProgramFilepath, nameof(ProgramFilepath));

            AddModelSubscription(inModel.TargetExtension.Subscribe(ext =>
            {
                ext = ext.ToLower();
                if (_owner.Any(item => item != this && item.Extension.ToLower() == ext))
                {
                    _validationErrors.AddOrSet(nameof(Extension), $"拡張子 {ext} の設定はすでに存在します");
                }
                else
                {
                    _validationErrors.AddOrSet(nameof(Extension), "");
                }
            }));
        }

        private IList<FileOpenProgramViewModel> _owner;
        private ValidationErrorList _validationErrors = new ValidationErrorList();
    }
}
