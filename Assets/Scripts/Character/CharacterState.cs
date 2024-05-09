using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [SerializeField] private string characterName; // ĳ���� �̸�
    public string CharacterName { get => characterName; set => characterName = value; }

    private Dictionary<e_StatType, int> characterStats; // ĳ������ ������ ����

    void Start()
    {
        InitCharacterStats();
    }

    // ���� �����͸� ������ �����ϴ� �޼ҵ�
    void InitCharacterStats()
    {
        // DataManager�� ����Ͽ� �� ĳ������ ���� �����͸� ������
        Data_Character.Param stats = DataManager.instance.GetCharacterData(CharacterName);

        // ���� �����͸� ������ ����
        characterStats = new Dictionary<e_StatType, int>
        {
            { e_StatType.Atk, stats.Atk },
            { e_StatType.Def, stats.Def },
            { e_StatType.Spd, stats.Spd },
            { e_StatType.Hp, stats.Hp },
            { e_StatType.MaxHp, stats.MaxHp },
            { e_StatType.Mp, stats.Mp },
            { e_StatType.MaxMp, stats.MaxMp },
            { e_StatType.Exhaustion, 0 },
            { e_StatType.Cooldown, 0 }
        };
    }

    // ���ϴ� ������ �������� �޼ҵ�
    public int GetStat(e_StatType statType)
    {
        return characterStats[statType];
    }

    public void SetStat(e_StatType statType, int value)
    {
        int max = (statType == e_StatType.Hp) ? GetStat(e_StatType.MaxHp) :
               (statType == e_StatType.Mp) ? GetStat(e_StatType.MaxMp) :
               Consts.MAX_STAT;
        characterStats[statType] = Mathf.Clamp((int)value, 0, max);
    }

    public void AddStat(e_StatType statType, int value)
    {
        characterStats[statType] += value;
        if (statType == e_StatType.Hp && statType == e_StatType.Mp)
        {
            int max = (statType == e_StatType.Hp) ? GetStat(e_StatType.MaxHp) : GetStat(e_StatType.MaxMp);
            characterStats[statType] = Mathf.Clamp(characterStats[statType], 0, max);
        }

        // ü�°� ������ �ִ�ġ�� ������ �ִ�ġ�� �°� ����
        characterStats[e_StatType.Hp] = Mathf.Min(characterStats[e_StatType.Hp], GetStat(e_StatType.MaxHp));
        characterStats[e_StatType.Mp] = Mathf.Min(characterStats[e_StatType.Mp], GetStat(e_StatType.MaxMp));
    }

    public void RemoveStat(e_StatType statType, int value)
    {
        characterStats[statType] -= value;
        if (statType == e_StatType.Hp && statType == e_StatType.Mp)
        {
            int max = (statType == e_StatType.Hp) ? GetStat(e_StatType.MaxHp) : GetStat(e_StatType.MaxMp);
            characterStats[statType] = Mathf.Clamp(characterStats[statType], 0, max);
        }

        // ü�°� ������ �ִ�ġ�� ������ �ִ�ġ�� �°� ����
        characterStats[e_StatType.Hp] = Mathf.Min(characterStats[e_StatType.Hp], GetStat(e_StatType.MaxHp));
        characterStats[e_StatType.Mp] = Mathf.Min(characterStats[e_StatType.Mp], GetStat(e_StatType.MaxMp));
    }
}
