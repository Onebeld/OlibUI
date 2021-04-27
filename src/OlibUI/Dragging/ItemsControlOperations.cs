using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OlibUI.Dragging
{
    public static class ItemsControlOperations
    {
        public static void MoveItemOnDrop<TItemsControl, TControlItem>(object sender, DragEventArgs e, Action<TItemsControl,TControlItem ,TControlItem> ToDo)
            where TItemsControl : ItemsControl
            where TControlItem : Control
        {
            TControlItem src = (TControlItem)sender;
            TControlItem target = (TControlItem)e.Data.Get(nameof(Control));

            if (src.Parent != target.Parent) return;

            if (!target.Equals(src))
            {
                TItemsControl parent = src.GetSelfAndLogicalAncestors().OfType<TItemsControl>().FirstOrDefault<TItemsControl>();

                Contract.Requires<NullReferenceException>(parent != null);

                int s_i = ((IList)parent.Items).IndexOf(src);
                int t_i = ((IList)parent.Items).IndexOf(target);

                if (parent is SelectingItemsControl s)
                {
                    s.SelectedItem = null;
                    s.SelectedIndex = -1;
                }
                OperateItemsIndex((IList<object>)parent.Items, s_i, t_i);

                ToDo.Invoke(parent, src, target);
            }
        }

        private static void OperateItemsIndex(IList<object> items, int srcIndex, int targetIndex)
        {
            if (items is IAvaloniaList<object> list) list.Move(targetIndex, srcIndex);
            else throw new NullReferenceException($"The items collection is not {nameof(IAvaloniaList<object>)}");
        }
    }
}
