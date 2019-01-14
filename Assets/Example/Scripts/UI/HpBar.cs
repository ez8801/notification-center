using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour, IObserver
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
        switch (notification.id)
        {
            case R.Id.OnHpChanged:
                OnHpChanged(notification.intExtra);
                break;
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
