using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Threading;

namespace ClassLibraryHelper.Collection
{
    public class SafeObservable<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly IList<T> _collection = new List<T>();
        private readonly Dispatcher _dispatcher;
        private readonly ReaderWriterLock _lock = new ReaderWriterLock();

        public SafeObservable()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public void Add(T item)
        {
            if (_dispatcher.Thread == Thread.CurrentThread)
                DoAdd(item);
            else
                _dispatcher.BeginInvoke((Action)(() => DoAdd(item)));
        }

        private void DoAdd(T item)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            _collection.Add(item);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            _lock.ReleaseWriterLock();
        }

        public void Clear()
        {
            if (_dispatcher.Thread == Thread.CurrentThread)
                DoClear();
            else
                _dispatcher.BeginInvoke((Action)(DoClear));
        }

        private void DoClear()
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            _collection.Clear();
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _lock.ReleaseWriterLock();
        }

        public bool Contains(T item)
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            var result = _collection.Contains(item);
            _lock.ReleaseReaderLock();
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            _collection.CopyTo(array, arrayIndex);
            _lock.ReleaseWriterLock();
        }

        public bool Remove(T item)
        {
            if (Thread.CurrentThread == _dispatcher.Thread)
                return DoRemove(item);
            var op = _dispatcher.BeginInvoke(new Func<T, bool>(DoRemove), item);
            if (op.Result == null)
                return false;
            return (bool)op.Result;
        }

        private bool DoRemove(T item)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            var index = _collection.IndexOf(item);
            if (index == -1)
            {
                _lock.ReleaseWriterLock();
                return false;
            }
            var result = _collection.Remove(item);
            if (result && CollectionChanged != null)
                CollectionChanged(this, new
                    NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _lock.ReleaseWriterLock();
            return result;
        }

        public int Count
        {
            get
            {
                _lock.AcquireReaderLock(Timeout.Infinite);
                var result = _collection.Count;
                _lock.ReleaseReaderLock();
                return result;
            }
        }
        public bool IsReadOnly { get { return _collection.IsReadOnly; } }

        public int IndexOf(T item)
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            var result = _collection.IndexOf(item);
            _lock.ReleaseReaderLock();
            return result;
        }

        public void Insert(int index, T item)
        {
            if (Thread.CurrentThread == _dispatcher.Thread)
                DoInsert(index, item);
            else
                _dispatcher.BeginInvoke((Action)(() => DoInsert(index, item)));
        }
        private void DoInsert(int index, T item)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            _collection.Insert(index, item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            _lock.ReleaseWriterLock();
        }
        public void RemoveAt(int index)
        {
            if (Thread.CurrentThread == _dispatcher.Thread)
                DoRemoveAt(index);
            else
                _dispatcher.BeginInvoke((Action)(() => DoRemoveAt(index)));
        }
        private void DoRemoveAt(int index)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            if (_collection.Count == 0 || _collection.Count <= index)
            {
                _lock.ReleaseWriterLock();
                return;
            }
            _collection.RemoveAt(index);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _lock.ReleaseWriterLock();

        }

        public T this[int index]
        {
            get
            {
                _lock.AcquireReaderLock(Timeout.Infinite);
                var result = _collection[index];
                _lock.ReleaseReaderLock();
                return result;
            }
            set
            {
                _lock.AcquireWriterLock(Timeout.Infinite);
                if (_collection.Count == 0 || _collection.Count <= index)
                {
                    _lock.ReleaseWriterLock();
                    return;
                }
                _collection[index] = value;
                _lock.ReleaseWriterLock();
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
