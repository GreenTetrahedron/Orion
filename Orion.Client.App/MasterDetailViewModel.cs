using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace Orion.Client.App
{
    public abstract partial class MasterDetailViewModel<T> : ObservableObject
    {
        private readonly ObservableCollection<T> items = [];

        // Too bad these don't work (yet?).
        // [ObservableProperty]
        // [AlsoNotifyChangeFor(nameof(HasCurrent))]
        private T selected;

        public event Action<T> OnSelectedChanged = delegate { };

        public ObservableCollection<T> Items => items;

        public T Selected
        {
            get => selected;
            set
            {
                OnSelectedChanged?.Invoke(value);
                SetProperty(ref selected, value);
                OnPropertyChanged(nameof(HasSelected));
            }
        }

        public bool HasSelected => selected is not null;

        public virtual T AddItem(T item)
        {
            items.Add(item);
            OnPropertyChanged(nameof(Items));

            return item;
        }

        public virtual T UpdateItem(T item, T original)
        {
            var hasCurrent = HasSelected;

            var i = items.IndexOf(original);
            items[i] = item; // Raises CollectionChanged.

            OnPropertyChanged(nameof(Items));

            if (hasCurrent && !HasSelected)
            {
                // Restore Current.
                Selected = item;
            }

            return item;
        }

        public virtual void DeleteItem(T item)
        {
            items.Remove(item);

            OnPropertyChanged(nameof(Items));
        }
    }
}
