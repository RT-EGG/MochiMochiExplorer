using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;

namespace Utility.Wpf
{
    public class ObservableCollectionSynchronizeOption
    {
        public bool DisposeOnRemove { get; set; } = false;
        public Action<NotifyCollectionChangedEventArgs?>? OnAfterSynced { get; set; } = null;
    }

    public static class ObservableCollectionSynchronizeList
    {
        public static IDisposable Synchronize<T, U>(this IReadOnlyReactiveCollection<T> inOriginal, 
            List<U> inSynced, 
            Func<IEnumerable<T>, IEnumerable<U>> select, 
            ObservableCollectionSynchronizeOption? inOption = null)
            => inOriginal.Synchronize(inSynced,
                select,
                inSynced.InsertRange,
                inSynced.RemoveRange,
                inOption
            );

        public static IDisposable Synchronize<T, U>(this IReadOnlyReactiveCollection<T> inOriginal, 
            ReactiveCollection<U> inSynced, 
            Func<IEnumerable<T>, IEnumerable<U>> select, 
            ObservableCollectionSynchronizeOption? inOption = null)
        {
            var result = new DisposableCollection<IDisposable>();

            if (inOption?.OnAfterSynced != null)
            {
                var originalOnAfterSynced = inOption!.OnAfterSynced;
                inOption.OnAfterSynced = args =>
                {
                    if (args != null)
                    {
                        switch (args.Action)
                        {
                            case NotifyCollectionChangedAction.Reset:
                            case NotifyCollectionChangedAction.Move:
                            case NotifyCollectionChangedAction.Replace:
                                originalOnAfterSynced!.Invoke(args);
                                break;
                        }
                    }
                };

                result.Add(inSynced.Subscribe(args =>
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                        case NotifyCollectionChangedAction.Remove:
                            originalOnAfterSynced!.Invoke(args);
                            break;
                    }
                }));
            }

            result.Add(inOriginal.Synchronize(inSynced,
                select,
                (index, items) => {
                    //inSynced.InsertRange(index, items);
                    items.Indexed().ForEach(i => inSynced.Insert(index + i.Item1, i.Item2));
                    return;
                },
                (index, count) => inSynced.RemoveRange(index, count),
                inOption
            ));

            return result;
        }

        private static IDisposable Synchronize<T, U>(this IReadOnlyReactiveCollection<T> inOriginal, IList<U> inSynced,
            Func<IEnumerable<T>, IEnumerable<U>> select,
            Action<int, IEnumerable<U>> insertRange,
            Action<int, int> removeRange,
            ObservableCollectionSynchronizeOption? inOption = null)
        {
            if (inOption == null)
                inOption = new ObservableCollectionSynchronizeOption();

            var needToDispose = typeof(IDisposable).IsAssignableFrom(typeof(U)) && inOption.DisposeOnRemove;

            if (needToDispose)
                inSynced.OfType<IDisposable>().DisposeItems();
            inSynced.Clear();
            insertRange?.Invoke(0, select(inOriginal));
            inOption.OnAfterSynced?.Invoke(null);

            var action = () => { };
            return inOriginal.Subscribe(args =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (args.NewItems != null)
                        {

                            //var items = args.NewItems.OfType<T>().Select(item => select(item)).ToArray();
                            var items = select(args.NewItems.OfType<T>());
                            action = () =>
                            {
                                insertRange?.Invoke(args.NewStartingIndex, items);
                            };
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (args.OldItems != null)
                        {
                            if (needToDispose)
                                args.OldItems.OfType<IDisposable>().DisposeItems();

                            action = () => removeRange?.Invoke(args.OldStartingIndex, args.OldItems.Count);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        if (args.NewItems != null)
                        {
                            action = () =>
                            {
                                var newItems = select(args.NewItems.OfType<T>()).ToArray();
                                foreach (var (i, newItem) in newItems.Indexed())
                                    inSynced[args.NewStartingIndex + i] = newItem;
                            };
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        action = () =>
                        {
                            var item = inSynced[args.OldStartingIndex];
                            inSynced.RemoveAt(args.OldStartingIndex);
                            inSynced.Insert(args.NewStartingIndex, item);
                        };                        
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        action = () =>
                        {
                            if (needToDispose)
                                inSynced.OfType<IDisposable>().DisposeItems();
                            inSynced.Clear();
                        };
                        break;

                }

                action();
                inOption.OnAfterSynced?.Invoke(args);
            });
        }
    }
}
