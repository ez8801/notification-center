using UnityEngine;
using Foundation.Notifications;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameConfig _gameConfig;

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = Mathf.Clamp(value, 0, _gameConfig.MaxHp);
            NotificationCenter.Post(R.Id.OnHpChanged, _hp);
        }
    }
    private int _hp;

    public int Mp
    {
        get
        {
            return _mp;
        }
        set
        {
            _mp = Mathf.Clamp(value, 0, _gameConfig.MaxMp);
            NotificationCenter.Post(R.Id.OnMpChanged, _mp);
        }
    }
    private int _mp;

    private float _dTime;

    // Start is called before the first frame update
    void Start()
    {
        _hp = _gameConfig.InitialHp;
        _mp = _gameConfig.InitialMp;
        _dTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _dTime += Time.deltaTime;
        if (_dTime > 1f)
        {
            _dTime -= 1f;

            Hp -= 5;
            Mp += 5;

            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void UesAbility()
    {
        if (Mp >= _gameConfig.AbilityMpCost)
        {
            Hp += _gameConfig.AbilityValue;
            Mp -= _gameConfig.AbilityMpCost;
        }
        else
        {
            // Not Enough Mana.
        }
    }
}
