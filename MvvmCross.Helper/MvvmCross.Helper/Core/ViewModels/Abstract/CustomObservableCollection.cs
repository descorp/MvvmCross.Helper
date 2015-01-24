namespace MvvmCross.Helper.Core.ViewModels.Abstract
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    #endregion

    /// <summary>
    /// The custom observable collection.
    /// </summary>
    /// <typeparam name="T">
    /// Type for collection
    /// </typeparam>
    public class CustomObservableCollection<T> : IList<T>, IList, INotifyCollectionChanged
    {
        #region Fields

        private readonly List<T> dataRepository = new List<T>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        public CustomObservableCollection(IEnumerable<T> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    this.dataRepository.Add(item);
                }

                this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public CustomObservableCollection(List<T> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    this.dataRepository.Add(item);
                }

                this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public CustomObservableCollection()
        {
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The collection changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.dataRepository.Count();
            }
        }

        /// <summary>
        /// Gets a value indicating whether is fixed size.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets a value indicating whether is synchronized.
        /// </summary>
        public bool IsSynchronized { get; set; }

        /// <summary>
        /// Gets the sync root.
        /// </summary>
        public object SyncRoot { get; set; }

        #endregion

        #region Public Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        /// <returns>
        /// The <see cref="T" />.
        /// </returns>
        public T this[int index]
        {
            get
            {
                return this.dataRepository[index];
            }

            set
            {
                this.dataRepository[index] = value;
            }
        }

        #endregion

        #region Explicit Interface Indexers

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this.dataRepository[index] = (T)value;
                this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(T item)
        {
            this.dataRepository.Add(item);
            this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.dataRepository.Count() - 1));
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int Add(object value)
        {
            var item = (T)value;
            this.dataRepository.Add(item);
            var index = this.dataRepository.Count() - 1;
            this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            return index;
        }

        /// <summary>
        /// The clear.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Clear()
        {
            this.dataRepository.Clear();
            this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Contains(object value)
        {
            return this.dataRepository.Contains((T)value);
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Contains(T item)
        {
            return this.dataRepository.Contains(item);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CopyTo(Array array, int index)
        {
            this.dataRepository.CopyTo((T[])array, index);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.dataRepository.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.dataRepository.GetEnumerator();
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int IndexOf(object value)
        {
            if (value is T)
            {
                return this.dataRepository.IndexOf((T)value);
            }

            return this.Count - 1;
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int IndexOf(T item)
        {
            return this.dataRepository.IndexOf(item);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Insert(int index, object value)
        {
            try
            {
                var item = (T)value;
                this.dataRepository.Insert(index, item);
                this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
            catch (Exception e)
            {
                throw new ArgumentException("Value is not a " + typeof(T).FullName, e);
            }
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Insert(int index, T item)
        {
            this.dataRepository.Insert(index, item);
            this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Remove(object value)
        {
            try
            {
                var item = (T)value;
                this.Remove(item);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Value is not a " + typeof(T).FullName, e);
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Remove(T item)
        {
            var index = this.dataRepository.IndexOf(item);
            var remove = this.dataRepository.Remove(item);
            this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            return remove;
        }

        /// <summary>
        /// The remove at.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public void RemoveAt(int index)
        {
            var item = this.dataRepository[index];
            this.dataRepository.RemoveAt(index);
            this.RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Methods

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = this.CollectionChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, args);
        }

        #endregion
    }
}