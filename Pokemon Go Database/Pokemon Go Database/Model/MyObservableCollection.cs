using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Pokemon_Go_Database.Model
{
    /// <summary>
    /// An extension of ObservableCollection that features support for notifying when a member has a property changed.
    /// This allows the view to update when an object is changed, not just added or removed.
    /// </summary>
    /// <typeparam name="T">The type of objects contained in the collection. Must implement INotifyPropertyChanged</typeparam>
    public class MyObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public MyObservableCollection() : base()
        {
            //CollectionChanged += new NotifyCollectionChangedEventHandler(NewCollectionChanged);
        }

        /// <summary>
        /// Constructor for creating from an IEnumerable
        /// </summary>
        /// <param name="enumerable">The IEnumerable object to instantiate from</param>
        public MyObservableCollection(IEnumerable<T> enumerable) : base(enumerable)
        {

        }

        /// <summary>
        /// When an item is added to the collection, it is given the proper event handler.
        /// Event handler is removed from item when it is removed from the collection.
        /// </summary>
        /// <param name="sender">The MyObservableCollection instance</param>
        /// <param name="e">The arguments</param>
        void ModifyEventHandler(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= MemberPropertyChanged;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += MemberPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Overrides OnCollectionChanged to add the event handlers to members.
        /// </summary>
        /// <param name="e">The arguments</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            ModifyEventHandler(this, e);
            base.OnCollectionChanged(e);
        }

        //protected override void MoveItem(int oldIndex, int newIndex)
        //{
        //    _suppressNotification = true;
        //    base.MoveItem(oldIndex, newIndex);
        //    _suppressNotification = false;
        //    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move));
        //}

        /// <summary>
        /// Event to notify that a member of the collection has been modified.
        /// </summary>
        public event PropertyChangedEventHandler MemberChanged;

        /// <summary>
        /// Raises the MemberChangedEvent
        /// </summary>
        /// <param name="sender">The member that has been modified</param>
        /// <param name="e">The arguments</param>
        private void MemberPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = MemberChanged;
            if (handler != null)
            {
                //Debug.WriteLine(this);
                //Debug.WriteLine(e.PropertyName);
                MemberChanged(sender, e);
                //Debug.WriteLine("A collection member was changed: " + e.PropertyName);
            }
            //Debug.WriteLine("A collection member was changed: " + e.PropertyName);
        }

        public void InsertRange(IEnumerable<T> items)
        {
            this.CheckReentrancy();
            int index = this.Items.Count;
            foreach (var item in items)
            {
                this.Items.Add(item);
                item.PropertyChanged += MemberPropertyChanged;
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
