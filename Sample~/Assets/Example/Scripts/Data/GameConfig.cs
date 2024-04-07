using UnityEngine;

[CreateAssetMenu(menuName="Assets/Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField]
    private int _maxHp;

    [SerializeField]
    private int _initialHp;

    [SerializeField]
    private int _maxMp;

    [SerializeField]
    private int _initialMp;

    [SerializeField]
    private int _abilityMpCost;

    [SerializeField]
    private int _abilityValue;

    public int MaxHp { get { return _maxHp; } }

    public int InitialHp { get { return _initialHp; } }

    public int MaxMp { get { return _maxMp; } }

    public int InitialMp { get { return _initialMp; } }

    public int AbilityMpCost { get { return _abilityMpCost; } }

    public int AbilityValue { get { return _abilityValue; } }
}
