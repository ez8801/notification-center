using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
	void HandleNotification(Notification notification);
}

/// <summary>
/// NotificationCenter
/// This class cannot be inherited.
/// </summary>
/// <seealso cref="Notification"/>
public sealed class NotificationCenter : MonoBehaviour
{
    #region Singleton

    private static NotificationCenter _instance = null;
    private static readonly object _lockObject = new object();

    public static NotificationCenter Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public static NotificationCenter GetInstance()
    {
        lock (_lockObject)
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NotificationCenter>();

                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(NotificationCenter).ToString());
                    DontDestroyOnLoad(go);

                    _instance = go.AddComponent<NotificationCenter>();
                }
            }
        }
        return _instance;
    }

    public static bool IsExists()
    {
        return (_instance != null);
    }

    #endregion Singleton


    private Dictionary<R.Id, List<WeakReference>> _observers
		= new Dictionary<R.Id, List<WeakReference>>();

    private Dictionary<R.Id, List<WeakReference>> _disposableObservers
        = new Dictionary<R.Id, List<WeakReference>>();

	EqualityComparer<IObserver> _comparer 
		= EqualityComparer<IObserver>.Default;
    
    private bool IsContains(List<WeakReference> references, IObserver observer)
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

	private bool IsEquals(WeakReference weakReference, IObserver observer)
	{
		if (IsExpiredObserver(weakReference))
			return false;

		IObserver x = weakReference.Target as IObserver;
		return _comparer.Equals(x, observer);
	}

    /// <summary>
    /// Add a disposable observer for the specific message.
    /// </summary>
    public void AddDisposableObserver(IObserver observer, R.Id message)
    {
        InteralAddObserver(ref _disposableObservers, observer, message);
    }

	/// <summary>
	/// Add an observer for the specific message.
	/// </summary>
	public void AddObserver(IObserver observer, R.Id message)
	{
        InteralAddObserver(ref _observers, observer, message);
	}

    private void InteralAddObserver(ref Dictionary<R.Id, List<WeakReference>> container
        , IObserver observer, R.Id message)
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
    public void RemoveObserver(IObserver observer, R.Id message)
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

    private void RemoveObserver(ref Dictionary<R.Id, List<WeakReference>> container
        , R.Id message)
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
    public void RemoveObserver(IObserver observer)
	{
		if (observer == null || _observers.Count == 0)
			return;
        
        var enumerator = _observers.GetEnumerator();
		while (enumerator.MoveNext())
		{
            R.Id message = enumerator.Current.Key;
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
		if (_observers.Count > 0)
		{
			_observers.Clear();
		}

        if (_disposableObservers.Count > 0)
        {
            _disposableObservers.Clear();
        }
	}

	public bool IsExpiredObserver(WeakReference match)
	{
		return (match == null || match.IsAlive == false || match.Target == null);
	}
    
    public static void PostDelayed(R.Id id)
    {
        PostDelayed(Notification.Create(id));
    }

    public static void PostDelayed(Notification msg)
    {
        Instance.StartCoroutine(Instance.PerformPostDelayed(msg));
    }
    
    public static void PostDelayed(Notification msg, float delayTime)
    {
        Instance.StartCoroutine(Instance.PerformPostDelayed(msg, delayTime));
    }

    public static void PostDelayed(Notification msg, int skipFrameCount)
    {
        Instance.StartCoroutine(Instance.PerformPostDelayed(msg, skipFrameCount));
    }

    private IEnumerator PerformPostDelayed(Notification msg)
    {
        yield return new WaitForEndOfFrame();
        InternalPost(msg);
    }

    private IEnumerator PerformPostDelayed(Notification msg, float delayTime)
    {
        yield return new UnityEngine.WaitForSeconds(delayTime);
        InternalPost(msg);
    }
    
    public IEnumerator PerformPostDelayed(Notification msg, int skipFrameCount)
    {
        for (int i = 0; i < skipFrameCount; i++)
        {
            yield return null;
        }
        InternalPost(msg);
    }
    
    private void InternalPost(Notification msg)
	{
        InternalPost(ref _observers, msg);
        InternalPost(ref _disposableObservers, msg);
        RemoveObserver(ref _disposableObservers, msg.id);
    }

    private void InternalPost(ref Dictionary<R.Id, List<WeakReference>> container
        , Notification msg)
    {
        if (container.ContainsKey(msg.id))
        {
            List<WeakReference> observers = container[msg.id];
            if ((observers == null || observers.Count == 0) == false)
            {
                // Remove Expired Observers.
                observers.RemoveAll(IsExpiredObserver);

                for (int i = 0; i < observers.Count; i++)
                {
                    IObserver observer = observers[i].Target as IObserver;
                    observer.HandleNotification(msg);
                }
            }
        }
    }

    public static void Post(Notification message)
    {
        Instance.InternalPost(message);
    }
    
	public static void Post(R.Id id)
	{
		Instance.InternalPost(Notification.Create(id));
	}
    
	public static void Post(R.Id id, int data)
	{
        Instance.InternalPost(Notification.Create(id, data));
	}
	
	public static void Post(R.Id id, float data)
	{
		Instance.InternalPost(Notification.Create(id, data));
	}

    public static void Post(R.Id id, long data)
    {
        Instance.InternalPost(Notification.Create(id, data));
    }

    public static void Post(R.Id id, string data)
	{
		Instance.InternalPost(Notification.Create(id, data));
	}

	public static void Post(R.Id id, bool data)
	{
        Instance.InternalPost(Notification.Create(id, data));
	}

    public static void Post(R.Id id, object data)
    {
        Instance.InternalPost(Notification.Create(id, data));
    }    
}
