using System.Collections.Generic;

/// <summary>
/// Notification
/// </summary>
/// <seealso cref="NotificationCenter"/>
public class Notification
{
    const int MAX_POOL_SIZE = 512;
    private static Queue<Notification> _Queue = new Queue<Notification>();
    
    // Use enum or int.
    public R.Id id;

	public int intExtra;
	public float floatExtra;
	public string stringExtra;
	public bool boolExtra;
    public long longExtra;
    public object dataExtra;

    private bool isPooled = false;

    public static Notification Create()
    {
        Notification result = null;
        while (result == null && _Queue.Count > 0)
        {
            result = _Queue.Dequeue();
        }

        if (result != null)
        {
            result.Clear();
            result.isPooled = false;
        }
        return new Notification();
    }

    public static Notification Create(R.Id id)
    {
        Notification notification = Create();
        notification.id = id;
        return notification;
    }

    public static Notification Create(R.Id id, int data)
    {
        Notification notification = Create();
        notification.id = id;
        notification.intExtra = data;
        return notification;
    }

    public static Notification Create(R.Id id, float data)
    {
        Notification notification = Create();
        notification.id = id;
        notification.floatExtra = data;
        return notification;
    }

    public static Notification Create(R.Id id, string data)
    {
        Notification notification = Create();
        notification.id = id;
        notification.stringExtra = data;
        return notification;
    }

    public static Notification Create(R.Id id, long data)
    {
        Notification notification = Create();
        notification.id = id;
        notification.longExtra = data;
        return notification;
    }

    public static Notification Create(R.Id id, bool data)
    {
        Notification notification = Create();
        notification.id = id;
        notification.boolExtra = data;
        return notification;
    }

    public static Notification Create(R.Id id, object data)
    {
        Notification notification = Create();
        notification.id = id;
        notification.dataExtra = data;
        return notification;
    }

    public Notification()
    {
        this.id = R.Id.None;
        intExtra = 0;
        floatExtra = 0f;
        stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = false;
        dataExtra = null;
    }

    public Notification(R.Id id)
	{
        this.id = id;
		intExtra = 0;
		floatExtra = 0f;
		stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = false;
        dataExtra = null;
    }

	public Notification(R.Id id, int data)
	{
		this.id = id;
		intExtra = data;
		floatExtra = 0f;
		stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = false;
        dataExtra = null;
    }

    public Notification(R.Id id, float data)
	{
		this.id = id;
		intExtra = 0;
		floatExtra = data;
		stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = false;
        dataExtra = null;
    }

    public Notification(R.Id id, string data)
	{
		this.id = id;
		intExtra = 0;
		floatExtra = 0f;
		stringExtra = data;
        longExtra = 0;
        boolExtra = false;
        dataExtra = null;
    }

    public Notification(R.Id id, long data)
    {
        this.id = id;
        intExtra = 0;
        floatExtra = 0f;
        stringExtra = string.Empty;
        longExtra = data;
        boolExtra = false;
        dataExtra = null;
    }

    public Notification(R.Id id, bool data)
	{
		this.id = id;
		intExtra = 0;
		floatExtra = 0f;
		stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = data;
        dataExtra = null;
    }

    public Notification(R.Id id, object data)
    {
        this.id = id;
        intExtra = 0;
        floatExtra = 0f;
        stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = false;
        dataExtra = data;
    }

    public void Clear()
    {
        id = R.Id.None;
        intExtra = 0;
        floatExtra = 0f;
        stringExtra = string.Empty;
        longExtra = 0;
        boolExtra = false;
        dataExtra = null;
    }

    ~Notification()
    {
        if (isPooled == false)
        {
            Clear();

            if (_Queue.Count < MAX_POOL_SIZE)
            {
                isPooled = true;
                lock (_Queue)
                {
                    _Queue.Enqueue(this);
                }

                System.GC.ReRegisterForFinalize(this);
            }
        }
    }
}
