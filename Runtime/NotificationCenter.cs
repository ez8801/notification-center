using System;
using System.Collections.Generic;

namespace Foundation.Notifications
{
    public interface INotificationReceiver
    {
        void HandleNotification(Notification notification);
    }

    /// <summary>
    /// NotificationCenter
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Notification"/>
    public sealed class NotificationCenter
    {
        private static NotificationCenter s_instance = null;
        public static NotificationCenter Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new NotificationCenter();
                return s_instance;
            }
        }

        private List<WeakReference> _universalObservers;
        private Dictionary<NotificationName, List<WeakReference>> _observers;
        private Dictionary<NotificationName, List<WeakReference>> _disposableObservers;
        EqualityComparer<INotificationReceiver> _comparer;

        public NotificationCenter()
        {
            _universalObservers = new List<WeakReference>();
            _observers = new Dictionary<NotificationName, List<WeakReference>>();
            _disposableObservers = new Dictionary<NotificationName, List<WeakReference>>();
            _comparer = EqualityComparer<INotificationReceiver>.Default;
        }

        private bool IsContains(List<WeakReference> references, INotificationReceiver observer)
        {
            if (references == null || references.Count == 0)
                return false;

            for (int i = 0; i < references.Count; i++)
            {
                WeakReference weakReference = references[i];
                if (IsEquals(weakReference, observer))
                    return true;
            }

            return false;
        }

        private bool IsEquals(WeakReference weakReference, INotificationReceiver observer)
        {
            if (IsExpiredObserver(weakReference))
                return false;

            INotificationReceiver x = weakReference.Target as INotificationReceiver;
            return _comparer.Equals(x, observer);
        }

        /// <summary>
        /// Add a disposable observer for the specific message.
        /// </summary>
        public void AddDisposableObserver(INotificationReceiver observer, NotificationName message)
        {
            InteralAddObserver(ref _disposableObservers, observer, message);
        }

        /// <summary>
        /// Add an universal observer
        /// </summary>
        public void AddUniversalObserver(INotificationReceiver observer)
        {
            if (observer == null)
                return;

            bool isContains = IsContains(_universalObservers, observer);
            if (isContains == false)
                _universalObservers.Add(new WeakReference(observer));
        }

        /// <summary>
        /// Add an observer for the specific message.
        /// </summary>
        public void AddObserver(INotificationReceiver observer, NotificationName message)
        {
            InteralAddObserver(ref _observers, observer, message);
        }

        private void InteralAddObserver(ref Dictionary<NotificationName, List<WeakReference>> container
            , INotificationReceiver observer, NotificationName message)
        {
            if (!container.ContainsKey(message))
                container[message] = new List<WeakReference>();

            List<WeakReference> observers = container[message];
            bool isContains = IsContains(observers, observer);
            if (isContains == false)
                container[message].Add(new WeakReference(observer));
        }

        /// <summary>
        /// Remove the specified observer.
        /// </summary>
        public void RemoveObserver(INotificationReceiver observer, NotificationName message)
        {
            if (observer == null || !_observers.ContainsKey(message))
                return;

            List<WeakReference> observers = _observers[message];
            if ((observers == null || observers.Count == 0) == false)
            {
                for (int i = 0; i < observers.Count; i++)
                {
                    WeakReference weakReference = observers[i];
                    if (IsEquals(weakReference, observer))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                if (observers.Count == 0)
                    _observers.Remove(message);
            }
        }

        private void RemoveObserver(ref Dictionary<NotificationName, List<WeakReference>> container
            , NotificationName message)
        {
            if (container == null || !container.ContainsKey(message))
                return;

            List<WeakReference> observers = container[message];
            if ((observers == null || observers.Count == 0) == false)
            {
                observers.Clear();
                observers = null;
            }

            container.Remove(message);
        }

        /// <summary>
        /// Remove the specified observer.
        /// </summary>
        public void RemoveObserver(INotificationReceiver observer)
        {
            if (_universalObservers.Count > 0)
            {
                for (int i = 0; i < _universalObservers.Count; i++)
                {
                    WeakReference weakReference = _universalObservers[i];
                    if (IsEquals(weakReference, observer))
                    {
                        _universalObservers.RemoveAt(i);
                        break;
                    }
                }
            }

            if (observer == null || _observers.Count == 0)
                return;

            var enumerator = _observers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                NotificationName message = enumerator.Current.Key;
                List<WeakReference> observers = _observers[message];
                if ((observers == null || observers.Count == 0) == false)
                {
                    observers.RemoveAll(IsExpiredObserver);

                    for (int i = 0; i < observers.Count; i++)
                    {
                        WeakReference weakReference = observers[i];
                        if (IsEquals(weakReference, observer))
                        {
                            observers.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void RemoveAll()
        {
            _universalObservers.Clear();
            _observers.Clear();
            _disposableObservers.Clear();
        }

        public bool IsExpiredObserver(WeakReference match)
        {
            return (match == null || match.IsAlive == false || match.Target == null);
        }

        private void InternalPost(Notification msg)
        {
            if (_universalObservers.Count > 0)
            {
                _universalObservers.RemoveAll(IsExpiredObserver);

                for (int i = 0; i < _universalObservers.Count; i++)
                {
                    var observer = _universalObservers[i].Target as INotificationReceiver;
                    observer.HandleNotification(msg);
                }
            }

            InternalPost(ref _observers, msg);
            InternalPost(ref _disposableObservers, msg);
            RemoveObserver(ref _disposableObservers, msg.Name);
        }

        private void InternalPost(ref Dictionary<NotificationName, List<WeakReference>> container
            , Notification msg)
        {
            if (container.ContainsKey(msg.Name))
            {
                List<WeakReference> observers = container[msg.Name];
                if ((observers == null || observers.Count == 0) == false)
                {
                    // Remove Expired Observers.
                    observers.RemoveAll(IsExpiredObserver);

                    for (int i = 0; i < observers.Count; i++)
                    {
                        INotificationReceiver observer = observers[i].Target as INotificationReceiver;
                        observer.HandleNotification(msg);
                    }
                }
            }
        }

        public static void Post(Notification message)
        {
            Instance.InternalPost(message);
        }

        public static void Post(NotificationName name)
        {
            Instance.InternalPost(Notification.Create(name));
        }

        public static void Post(NotificationName name, int data)
        {
            Instance.InternalPost(Notification.Create(name, data));
        }

        public static void Post(NotificationName name, float data)
        {
            Instance.InternalPost(Notification.Create(name, data));
        }

        public static void Post(NotificationName name, long data)
        {
            Instance.InternalPost(Notification.Create(name, data));
        }

        public static void Post(NotificationName name, string data)
        {
            Instance.InternalPost(Notification.Create(name, data));
        }

        public static void Post(NotificationName name, bool data)
        {
            Instance.InternalPost(Notification.Create(name, data));
        }

        public static void Post(NotificationName name, object data)
        {
            Instance.InternalPost(Notification.Create(name, data));
        }
    }
}