using System;

namespace MochiMochiExplorer.ViewModel.Wpf
{
    internal class NullCommandClass<T> : CommandBase<T> where T : IViewModel
    {
        public NullCommandClass(T inViewModel) : base(inViewModel)
        { }

        public override bool CanExecute(object? parameter)
            => false;

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
