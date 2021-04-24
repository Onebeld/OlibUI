using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Media;
using OlibUI.Generators;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace OlibUI.Instruments.NavigationView
{
    [PseudoClasses(":normal")]
    public partial class NavigationView : TreeView, IItemsPresenterHost, IContentPresenterHost, IHeadered
    {
        public readonly static StyledProperty<object> HeaderProperty =
            AvaloniaProperty.Register<NavigationView, object>(nameof(Header), "Header");
        public readonly static StyledProperty<Geometry> IconProperty =
            AvaloniaProperty.Register<NavigationView, Geometry>(
                nameof(Icon));
        public readonly static DirectProperty<NavigationView, object> TitleProperty =
            AvaloniaProperty.RegisterDirect<NavigationView, object>(
                nameof(Title),
                o => o.Title);
        public readonly static DirectProperty<NavigationView, object> SelectedContentProperty =
            AvaloniaProperty.RegisterDirect<NavigationView, object>(
                nameof(SelectedContent),
                o => o.SelectedContent);
        public readonly static StyledProperty<double> CompactPaneLengthProperty =
            AvaloniaProperty.Register<NavigationView, double>(nameof(CompactPaneLength), 50);
        public readonly static StyledProperty<double> OpenPaneLengthProperty =
            AvaloniaProperty.Register<NavigationView, double>(nameof(OpenPaneLength), 200);
        public readonly static StyledProperty<bool> IsOpenProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(IsOpen), true);
        public readonly static StyledProperty<bool> HideTitleProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(HideTitle), false);
        public static readonly StyledProperty<SplitViewDisplayMode> DisplayModeProperty =
            AvaloniaProperty.Register<NavigationView, SplitViewDisplayMode>(nameof(DisplayMode), SplitViewDisplayMode.CompactInline);
        public readonly static StyledProperty<bool> AlwaysOpenProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(AlwaysOpen), false);
        public readonly static StyledProperty<bool> AutoCompleteBoxIsVisibleProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(AutoCompleteBoxIsVisible), true);
        public readonly static DirectProperty<NavigationView, IEnumerable<string>> ItemsAsStringsProperty =
            AvaloniaProperty.RegisterDirect<NavigationView, IEnumerable<string>>(nameof(ItemsAsStrings), o => o.ItemsAsStrings);

        private object _title;
        private object _selectedcontent;
        private IEnumerable<string> _itemsasstrings;
        private NavigationViewItemBase _headerItem;
        private AutoCompleteBox _completeBox;

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public Geometry Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }


        public object Title
        {
            get => _title;
            private set => SetAndRaise(TitleProperty, ref _title, value);
        }


        public object SelectedContent
        {
            get => _selectedcontent;
            private set => SetAndRaise(SelectedContentProperty, ref _selectedcontent, value);
        }


        public double CompactPaneLength
        {
            get => GetValue(CompactPaneLengthProperty);
            set => SetValue(CompactPaneLengthProperty, value);
        }


        public double OpenPaneLength
        {
            get => GetValue(OpenPaneLengthProperty);
            set => SetValue(OpenPaneLengthProperty, value);
        }


        public bool IsOpen
        {
            get => GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }


        public bool HideTitle
        {
            get => GetValue(HideTitleProperty);
            set => SetValue(HideTitleProperty, value);
        }

        public bool AlwaysOpen
        {
            get => GetValue(AlwaysOpenProperty);
            set => SetValue(AlwaysOpenProperty, value);
        }


        public SplitViewDisplayMode DisplayMode
        {
            get => GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }


        public bool AutoCompleteBoxIsVisible
        {
            get => GetValue(AutoCompleteBoxIsVisibleProperty);
            set => SetValue(AutoCompleteBoxIsVisibleProperty, value);
        }


        public IEnumerable<string> ItemsAsStrings
        {
            get => _itemsasstrings;
            private set => SetAndRaise(ItemsAsStringsProperty, ref _itemsasstrings, value);
        }

        static NavigationView()
        {
            SelectionModeProperty.OverrideDefaultValue<NavigationView>(SelectionMode.Single);
            SelectedItemProperty.Changed.AddClassHandler<NavigationView>((x, e) => x.OnSelectedItemChanged(x, e));
            FocusableProperty.OverrideDefaultValue<NavigationView>(true);
            IsOpenProperty.Changed.AddClassHandler<NavigationView>((x, e) => x.OnIsOpenChanged(x, e));
        }

        public NavigationView()
        {
            PseudoClasses.Add(":normal");
        }

        protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ItemsCollectionChanged(sender, e);

            ProcessString();
        }

        protected virtual IEnumerable<string> ProcessString()
        {
            AvaloniaList<string> l = new AvaloniaList<string>();
            IEnumerable<NavigationViewItem> items = this.GetLogicalDescendants().OfType<NavigationViewItem>();

            foreach (NavigationViewItem item in items) 
                if (item.Header != null) l.Add(item.Header.ToString());

            ItemsAsStrings = l;
            return ItemsAsStrings;
        }

        internal void SelectSingleItemCore(object item)
        {
            if (SelectedItem != item)
            {
                PseudoClasses.Remove(":normal");
                PseudoClasses.Add(":normal");
            }

            if (!(SelectedItem is null)) ((ISelectable)SelectedItem).IsSelected = false;

            ((ISelectable)item).IsSelected = true;

            SelectedItems.Clear();
            SelectedItems.Add(item);

            IEnumerable<NavigationViewItem> item_parents = ((ILogical)item).GetLogicalAncestors().OfType<NavigationViewItem>();

            if (IsOpen) foreach (NavigationViewItem n in item_parents) n.IsExpanded = true;

            SelectedItem = item;
        }

        internal void SelectSingleItem(object item)
        {
            SelectSingleItemCore(item);
        }

        protected void OnSelectedItemChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            UpdateTitleAndSelectedContent();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _headerItem = e.NameScope.Find<NavigationViewItemBase>("PART_HeaderItem");
            _completeBox = e.NameScope.Find<AutoCompleteBox>("PART_AutoCompleteBox");
            _completeBox.SelectionChanged += CompleteBoxItemSelected;

            _headerItem.PointerPressed += (s, e_) =>
            {
                bool ea = IsOpen;
                bool a = AlwaysOpen;
                if (a != true)
                {
                    switch (ea)
                    {
                        case true:
                            IsOpen = false;
                            break;

                        case false:
                            IsOpen = true;
                            break;
                    }
                }
                else if (a == true)
                {
                    IsOpen = true;
                }
            };

            UpdateTitleAndSelectedContent();
            ProcessString();
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);

            if (Items is IList l && l.Count >= 1 && l[0] is ISelectable s) SelectSingleItem(s);
        }

        private void CompleteBoxItemSelected(object sender, SelectionChangedEventArgs e)
        {
            object n = ((AutoCompleteBox)sender).SelectedItem; //gets the header string
            IEnumerable<NavigationViewItem> val = this.GetLogicalDescendants()
                            .OfType<NavigationViewItem>().Where(x => x.Header == n); //select the nav-item by type and header

            NavigationViewItem val_c = val.FirstOrDefault(); // converts to NavigationViewItem

            if (val_c != null) SelectSingleItem(val_c);
        }

        ///<inheritdoc/>
        IAvaloniaList<ILogical> IContentPresenterHost.LogicalChildren => LogicalChildren;

        private IContentPresenter ContentPart { get; set; }

        bool IContentPresenterHost.RegisterContentPresenter(IContentPresenter presenter) => RegisterContentPresenter(presenter);

        protected override IItemContainerGenerator CreateItemContainerGenerator()
            => new NavigationViewContainerGenerator(this, NavigationViewItem.ContentProperty, NavigationViewItem.ItemsProperty, NavigationViewItem.HeaderProperty, NavigationViewItem.TitleProperty, NavigationViewItem.IsExpandedProperty);

        ///<inheritdoc/>
        protected virtual bool RegisterContentPresenter(IContentPresenter presenter)
        {
            if (presenter.Name == "PART_SelectedContentPresenter")
            {
                ContentPart = presenter;
                return true;
            }
            return false;
        }

        ///<inheritdoc/>
        protected override void OnContainersMaterialized(ItemContainerEventArgs e)
        {
            base.OnContainersMaterialized(e);
            UpdateTitleAndSelectedContent();
        }

        ///<inheritdoc/>
        protected override void OnContainersDematerialized(ItemContainerEventArgs e)
        {
            base.OnContainersDematerialized(e);
            UpdateTitleAndSelectedContent();
        }

        protected virtual void OnIsOpenChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
        }

        protected virtual void UpdateTitleAndSelectedContent()
        {
            if (SelectedItem is NavigationViewItemBase s)
            {
                SelectedContent = s.Content;
                Title = s.Title;
            }
        }
    }
}
