using UnityEngine;
using UnityEngine.UI;
using Foundation.Notifications;

public class HpBar : MonoBehaviour, INotificationReceiver
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
        NotificationCenter.Instance.AddObserver(this, R.Id.OnHpChanged);
    }

    public void HandleNotification(Notification notification)
    {
        if (notification.Name == R.Id.OnHpChanged)
        {
            OnHpChanged(notification.IntExtra);
        }
    }

    public void OnHpChanged(int hp)
    {
        _slider.value = (hp > 0) ? ((float)hp / _gameConfig.MaxHp) : 0f;
    }

    private void OnDestroy()
    {
        NotificationCenter.Instance.RemoveObserver(this, R.Id.OnHpChanged);
    }
}
