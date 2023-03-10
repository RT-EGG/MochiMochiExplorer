using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Utility.Wpf.Behavior
{
    // refered
    // https://chorusde.hatenablog.jp/entry/2013/02/28/064747

    public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
    {
        /// <summary>
        /// アタッチ時
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            DataGrid grid = (DataGrid)this.AssociatedObject;
            if (grid != null)
            {
                //DataGridの選択アイテム変更時のハンドラを登録
                grid.SelectionChanged += grid_SelectionChanged;
            }
        }

        /// <summary>
        /// デタッチ時
        /// </summary>
        protected override void OnDetaching()
        {
            DataGrid grid = (DataGrid)this.AssociatedObject;
            if (grid != null)
            {
                //DataGridの選択アイテム変更時のハンドラの登録を解除
                grid.SelectionChanged -= grid_SelectionChanged;
            }

            base.OnDetaching();
        }

        #region 選択されたアイテムリストの依存プロパティの登録
        public static DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(DataGridSelectedItemsBehavior), new PropertyMetadata(null));

        /// <summary>
        /// 選択されたアイテムリスト
        /// </summary>
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        #endregion

        /// <summary>
        /// 選択されたアイテムリストを選択されたアイテムの変更に応じて更新
        /// </summary>
        void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //新たに選択されたアイテムをリストに追加
            foreach (var addedItem in e.AddedItems)
            {
                SelectedItems.Add(addedItem);
            }

            //選択解除されたアイテムをリストから削除
            foreach (var removedItem in e.RemovedItems)
            {
                SelectedItems.Remove(removedItem);
            }
        }
    }
}
