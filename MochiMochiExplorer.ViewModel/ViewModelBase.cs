using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Utility;

namespace MochiMochiExplorer.ViewModel.Wpf
{
    public interface IViewModel : INotifyPropertyChanged
    { }

    public abstract class ViewModelBase<M> : IViewModel, IDisposable where M : class
    {
        public ViewModelBase()
        {
            if (TargetApplicationBinder.Instance == null)
                return;

            TargetApplicationBinder.Instance.Application!.OnBeforeFrameworkElementRemove += element =>
            {
                if ((_view is not null) && (_view == element))
                    _view = null;
            };
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void BindModel(M? inModel)
        {
            ModelSubscriptions.DisposeItems();
            ModelSubscriptions.Clear();

            _model.Value = inModel;
            if (Model != null)
                BindModelProperties(Model);
        }

        public IDisposable RegisterAfterModelChanged(Action<M?> onChange)
            => _model.Subscribe(m => onChange(m));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected M? Model => _model.Value;
        private ReactiveProperty<M?> _model = new ReactiveProperty<M?>();
        public bool ModelBinded => Model != null;

        protected FrameworkElement? View
        {
            get
            {
                if (_view == null)
                    _view = TargetApplicationBinder.Instance?.FindViewFor<FrameworkElement>(this);
                return _view;
            }
        }
        private FrameworkElement? _view;

        internal virtual Dispatcher? Dispatcher => TargetApplicationBinder.Instance.Application?.UiDispatcher;
            //_view?.Dispatcher;

        protected virtual void BindModelProperties(M inModel)
        { }

        protected void AddModelSubscription(IDisposable inSubscription)
            => ModelSubscriptions.Add(inSubscription);
        protected void RegisterPropertyNotification<T>(IReadOnlyReactiveProperty<T> inProperty, params string[] inPropertyNames)
            => AddModelSubscription(inProperty.ObserveOnUIDispatcher().Subscribe(_ =>
                inPropertyNames.ForEach(name => FirePropertyChanged(name))
            ));

        private List<IDisposable> ModelSubscriptions = new List<IDisposable>();
        private bool _isDiposed;

        protected void FirePropertyChanged(params string[] inPropertyNames)
            => inPropertyNames.ForEach(name => FirePropertyChanged(name));

        protected void FirePropertyChanged(string inPropertyName)
            => FirePropertyChanged(new PropertyChangedEventArgs(inPropertyName));

        protected void FirePropertyChanged(PropertyChangedEventArgs inArgs)
            => TargetApplicationBinder.Instance.Application!.UiDispatcher.BeginInvoke(() => PropertyChanged?.Invoke(this, inArgs));

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDiposed)
            {
                if (disposing)
                {
                    ModelSubscriptions.DisposeItems();
                    ModelSubscriptions.Clear();
                }

                _isDiposed = true;
            }
        }
    }
}
