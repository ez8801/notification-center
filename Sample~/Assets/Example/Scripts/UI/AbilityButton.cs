using UnityEngine;
using UnityEngine.UI;
using Foundation.Notifications;

public class AbilityButton : MonoBehaviour, INotificationReceiver
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameConfig _gameConfig;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

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
        _button.interactable = (mp >= _gameConfig.AbilityMpCost);
    }

    private void OnDestroy()
    {
        NotificationCenter.Instance.RemoveObserver(this, R.Id.OnMpChanged);
    }
}
