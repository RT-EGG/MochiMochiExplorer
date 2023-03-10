using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace MochiMochiExplorer.ViewModel.Wpf
{
    public delegate void FrameworkElementNotifyAction(FrameworkElement inValue);

    public interface ITargetApplicationEvents
    {
        event FrameworkElementNotifyAction? OnAfterFrameworkElementAdd;
        event FrameworkElementNotifyAction? OnBeforeFrameworkElementRemove;
    }

    public interface ITargetApplication : ITargetApplicationEvents
    {
        Dispatcher UiDispatcher { get; }
        IEnumerable<FrameworkElement> FrameworkElements { get; }

        void ShowFileOpenOptionView(FileOpenOptionViewModel.FileOpenOptionViewModel inViewModel);
        void QuitApplication();
    }

    public class TargetApplicationBinder
    {
        private TargetApplicationBinder(ITargetApplication inApplication)
        {
            Application = inApplication;
        }

        public static void InitializeForApplication(ITargetApplication inTargetApplication)
        {
            _instance = new TargetApplicationBinder(inTargetApplication);
        }

        public static TargetApplicationBinder? Instance => _instance;
        private static TargetApplicationBinder? _instance = null;

        public T? FindViewFor<T>(IViewModel inViewModel) where T : FrameworkElement
            => Application!.FrameworkElements.OfType<T>().FirstOrDefault(f => f.DataContext == inViewModel);

        public readonly ITargetApplication? Application = null;
    }
}
