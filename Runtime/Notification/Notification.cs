#define NOTIFICATION_USE_POOL
using System.Collections.Generic;

namespace Foundation.Notifications
{
    public struct NotificationName
    {
        readonly string _name;

        public NotificationName(string name) => _name = name;
        public static implicit operator NotificationName(string name) => new NotificationName(name);
        public static implicit operator string(NotificationName name) => name._name;
        public override string ToString() => _name;
    }

    /// <summary>
    /// Notification
    /// </summary>
    /// <seealso cref="NotificationCenter"/>
    public class Notification
    {
#if NOTIFICATION_USE_POOL
        const int MAX_POOL_SIZE = 512;
        private static Queue<Notification> _Queue = new Queue<Notification>();

        private bool _isPooled = false;
#endif

        public NotificationName Name;

        public int IntExtra
        {
            get => (int)LongExtra;
            set => LongExtra = value;
        }
        public float FloatExtra;
        public string StringExtra;
        public bool BoolExtra;
        public long LongExtra;
        public object DataExtra;

        public static Notification Create()
        {
#if NOTIFICATION_USE_POOL
            Notification result = null;
            while (result == null && _Queue.Count > 0)
            {
                result = _Queue.Dequeue();
            }

            if (result != null)
            {
                result.Clear();
                result._isPooled = false;
                return result;
            }
#endif
            return new Notification();
        }

        public static Notification Create(NotificationName name)
        {
            Notification notification = Create();
            notification.Name = name;
            return notification;
        }

        public static Notification Create(NotificationName name, int data)
        {
            Notification notification = Create();
            notification.Name = name;
            notification.IntExtra = data;
            return notification;
        }

        public static Notification Create(NotificationName name, float data)
        {
            Notification notification = Create();
            notification.Name = name;
            notification.FloatExtra = data;
            return notification;
        }

        public static Notification Create(NotificationName name, string data)
        {
            Notification notification = Create();
            notification.Name = name;
            notification.StringExtra = data;
            return notification;
        }

        public static Notification Create(NotificationName name, long data)
        {
            Notification notification = Create();
            notification.Name = name;
            notification.LongExtra = data;
            return notification;
        }

        public static Notification Create(NotificationName name, bool data)
        {
            Notification notification = Create();
            notification.Name = name;
            notification.BoolExtra = data;
            return notification;
        }

        public static Notification Create(NotificationName name, object data)
        {
            Notification notification = Create();
            notification.Name = name;
            notification.DataExtra = data;
            return notification;
        }

        public Notification()
        {
            Name = string.Empty;
            IntExtra = 0;
            FloatExtra = 0f;
            StringExtra = string.Empty;
            LongExtra = 0;
            BoolExtra = false;
            DataExtra = null;

            _isPooled = false;
        }

        public void Clear()
        {
            Name = string.Empty;
            IntExtra = 0;
            FloatExtra = 0f;
            StringExtra = string.Empty;
            LongExtra = 0;
            BoolExtra = false;
            DataExtra = null;
        }

        ~Notification()
        {
#if NOTIFICATION_USE_POOL
            if (_isPooled == false)
            {
                Clear();

                if (_Queue.Count < MAX_POOL_SIZE)
                {
                    _isPooled = true;
                    lock (_Queue)
                    {
                        _Queue.Enqueue(this);
                    }

                    System.GC.ReRegisterForFinalize(this);
                }
            }
#endif
        }
    }
}