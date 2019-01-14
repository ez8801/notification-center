
/// <summary>
/// Notification
/// </summary>
/// <seealso cref="NotificationCenter"/>
public struct Notification
{
    // Use enum or int.
    public R.Id id;

	public int intExtra;
	public float floatExtra;
	public string stringExtra;
	public bool boolExtra;
    public long longExtra;
    public object dataExtra;

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
}
