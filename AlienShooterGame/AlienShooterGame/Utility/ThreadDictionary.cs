using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AlienShooterGame
{

    /// <summary>
    /// The threaded dictionary class allows quick lookup of items via their key, and provides a foreach functionality
    /// that is thread-safe, allowing items to be added and removed from the dictionary while foreach loops are occuring.
    /// </summary>
    /// <typeparam name="KeyType">The type used for the dictionary key.</typeparam>
    /// <typeparam name="ValueType">The item type to be stored in the dictionary.</typeparam>
    public class ThreadDictionary<KeyType, ValueType>
    {
        /// <summary>
        /// This event is fired when all jobs in the New and Old entity lists have been synchronized into the main 
        /// dictionary.
        /// </summary>
        public event QueuesEmptiedEventhandler QueuesEmptied;
        public delegate void QueuesEmptiedEventhandler();
        
        /// <summary>
        /// This event is fired when an item is added to the dictionary.
        /// </summary>
        /// <param name="sender">The item being added.</param>
        public event ItemAddedEventHandler ItemAdded;
        public delegate void ItemAddedEventHandler(ValueType sender);

        /// <summary>
        /// This event is fired when an item is removed from the dictionary.
        /// </summary>
        /// <param name="sender">The item being removed.</param>
        public event ItemRemovedEventHandler ItemRemoved;
        public delegate void ItemRemovedEventHandler(ValueType sender);

        /// <summary>
        /// This is the dictionary datastructure that this threaded dictionary is wrapped around.
        /// </summary>
        protected Dictionary<KeyType, ValueType> _Dictionary;

        /// <summary>
        /// IsCycling is greater than 0 when a thread is using the foreach iterator of the dictionary. When using foreach it 
        /// is important that no items are added or removed from the dictionary. Instead they are queued to be added/removed 
        /// and this will take place when all threads have completed the foreach loop.
        /// IsCycling records the number of threads currently using the foreach iterator.
        /// </summary>
        protected UInt16 _IsCycling = 0;

        /// <summary>
        /// When the dictionary is to be updated by deleting old items and adding new items, the foreach iterations
        /// are blocked while this process is underway.
        /// </summary>
        protected bool _IterationBlocked = false;

        /// <summary>
        /// Items that are queued to be added to the entity manager are stored in here. Items are queued if the dictionary
        /// is busy being used by another thread.
        /// </summary>
        protected List<KeyValuePair<KeyType, ValueType>> _NewItems;

        /// <summary>
        /// Items that are queued to be removed are stored here. Items are queued if the dictionary is busy being used
        /// by another thread.
        /// </summary>
        protected List<KeyType> _OldItems;

        /// <summary>
        /// The default initial capacity for the dictionary.
        /// </summary>
        private const int DefaultHashSize = 1024;

        /// <summary>
        /// Returns the number of items currently in the dictionary.
        /// </summary>
        public int Count { get { return _Dictionary.Count; } }

        /// <summary>
        /// True if the dictionary is about to be wiped empty.
        /// </summary>
        protected bool _Clearing = false;


        /// <summary>
        /// Creates a thread-safe dictionary with default initial capacity.
        /// </summary>
        public ThreadDictionary()
        {
            _Dictionary = new Dictionary<KeyType, ValueType>(DefaultHashSize);
            _NewItems = new List<KeyValuePair<KeyType,ValueType>>();
            _OldItems = new List<KeyType>();

        }

        /// <summary>
        /// This method looks up the specified key in the dictionary.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <param name="value">A holder for the output value.</param>
        /// <returns>True if the key was found in the dictionary, false otherwise.</returns>
        public bool TryGetValue(KeyType key, out ValueType value)
        {
            lock (_Dictionary)
            {
                return _Dictionary.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// Adds the specified item, key combo to the dictionary.
        /// </summary>
        /// <param name="key">The key for the item.</param>
        /// <param name="item">The item to be added.</param>
        public void Add(KeyType key, ValueType item)
        {
            lock (_Dictionary)
            {
                if (_IsCycling > 0)
                {
                    _NewItems.Add(new KeyValuePair<KeyType, ValueType>(key, item));
                    //_IterationBlocked = true;
                }
                else
                {
                    ValueType val;
                    if (!_Dictionary.TryGetValue(key, out val))
                        _Dictionary.Add(key, item);
                }
            }
            OnItemAdded(item);
        }

        /// <summary>
        /// This function removes a dictionary item.
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <returns>True if the item was found and removed, false otherwise.</returns>
        public bool Remove(KeyType key)
        {
            ValueType item = default(ValueType);
            bool result = TryGetValue(key, out item);
            if (!result) return false;
            lock (_Dictionary)
            {
                if (_IsCycling > 0)
                {
                    _OldItems.Add(key);
                    //_IterationBlocked = true;
                }
                else
                    _Dictionary.Remove(key);
            }
            OnItemRemoved(item);
            return true;
        }

        /// <summary>
        /// This function allows the user to perform a foreach loop for each item in the dictionary without worrying about
        /// threading issues. Multiple threads can perform this foreach loop simultaneously and items can be added and removed
        /// while these loops occur. The function to be performed must be passed to the ForEach function.
        /// </summary>
        /// <param name="function">The function to be performed on each item.</param>
        public object ForEach(Func<ValueType, object, object, object, object> function, object p1, object p2, object p3) 
        {
            object result = null;
            lock (_Dictionary)
            {
                while (_IterationBlocked)
                    Monitor.Wait(_Dictionary);
                _IsCycling++;
                
            }
            foreach (KeyValuePair<KeyType, ValueType> pair in _Dictionary)
            {
                result = function(pair.Value, p1, p2, p3);
            }
            lock (_Dictionary)
            {
                _IsCycling--;
                if (_IsCycling == 0)
                    EmptyQueues();
            }
            return result;
        }

        /// <summary>
        /// Clears the dictionary of all values.
        /// </summary>
        public virtual bool Clear()
        {
            lock (_Dictionary)
            {
                if (_IsCycling > 0)
                {
                    _Clearing = true;
                    return false;
                }
                else
                {
                    _Dictionary.Clear();
                    return true;
                }
            }
        }

        /// <summary>
        /// This function updates the dictionary when all threads have stopped using the foreach iterator. It adds the new
        /// items to the dictionary and removes the old items from the dictionary then wakes up all threads that are
        /// waiting for this task.
        /// </summary>
        protected void EmptyQueues()
        {
            if (_Clearing)
            {
                _Dictionary.Clear();
                _Clearing = false;
            }

            if (_OldItems.Count == 0 && _NewItems.Count == 0)
                return;
            foreach (KeyType key in _OldItems)
            {
                try { _Dictionary.Remove(key); }
                catch (Exception) { }
            }
            foreach (KeyValuePair<KeyType, ValueType> pair in _NewItems)
            {
                try { _Dictionary.Add(pair.Key, pair.Value); }
                catch (Exception) { }
            }
            _OldItems.Clear();
            _NewItems.Clear();
            _IterationBlocked = false;
            Monitor.Pulse(_Dictionary);
            if (QueuesEmptied != null)
                QueuesEmptied();
        }


        /// <summary>
        /// Fires the ItemAdded event.
        /// </summary>
        /// <param name="e">The entity that was added.</param>
        protected virtual void OnItemAdded(ValueType item)
        {
            if (ItemAdded != null)
                ItemAdded(item);
        }

        /// <summary>
        /// Fires the ItemRemoved event.
        /// </summary>
        /// <param name="e">The entity being removed.</param>
        protected virtual void OnItemRemoved(ValueType item)
        {
            if (ItemRemoved != null)
                ItemRemoved(item);
        }
    }
}
