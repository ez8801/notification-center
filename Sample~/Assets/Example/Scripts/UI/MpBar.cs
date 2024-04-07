using UnityEngine;
using UnityEngine.UI;
using Foundation.Notifications;

public class MpBar : MonoBehaviour, INotificationReceiver
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private GameConfig _gameConfig;

    private void Awake()
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();

        AddObserver();
    }

    private void AddObserver()
    {
        NotificationCenter.Instance.AddObserver(this, R.Id.OnMpChanged);
    }

    public void HandleNotification(Notification notification)
    {
        if (notification.Name == R.Id.OnMpChanged)
        {
            OnMpChanged(notification.IntExtra);
        }
    }

    public void OnMpChanged(int mp)
    {
        _slider.value = (mp > 0) ? ((float)mp / _gameConfig.MaxHp) : 0f;
    }

    private void OnDestroy()
    {
        NotificationCenter.Instance.RemoveObserver(this, R.Id.OnMpChanged);
    }
}
