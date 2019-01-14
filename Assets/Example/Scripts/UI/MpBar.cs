using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour, IObserver
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
        switch (notification.id)
        {
            case R.Id.OnMpChanged:
                OnMpChanged(notification.intExtra);
                break;
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
