using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public string characterName; // ĳ���� �̸�
    private Dictionary<e_StatType, int> characterStats; // ĳ������ ������ ����

    void Start()
    {
        InitCharacterStats();
    }

    // ���� �����͸� ������ �����ϴ� �޼ҵ�
    void InitCharacterStats()
    {
        // DataManager�� ����Ͽ� �� ĳ������ ���� �����͸� ������
        Data_Character.Param stats = DataManager.instance.GetCharacterData(characterName);

        // ���� �����͸� ������ ����
        characterStats = new Dictionary<e_StatType, int>
        {
            { e_StatType.Atk, stats.Atk },
            { e_StatType.Def, stats.Def },
            { e_StatType.Spd, stats.Spd },
            { e_StatType.Hp, stats.Hp },
            { e_StatType.MaxHp, stats.MaxHp },
            { e_StatType.Mp, stats.Mp },
            { e_StatType.MaxMp, stats.MaxMp }
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
        int max = (statType == e_StatType.Hp) ? GetStat(e_StatType.MaxHp) :
            (statType == e_StatType.Mp) ? GetStat(e_StatType.MaxMp) :
            Consts.MAX_STAT;
        characterStats[statType] += value;
        characterStats[statType] = Mathf.Clamp(characterStats[statType], 0, max);

        // MaxHP �Ǵ� MaxMP�� �����ϸ� HP �Ǵ� MP�� ����
        if (statType == e_StatType.MaxHp || statType == e_StatType.MaxMp)
        {
            e_StatType currentStatType = (statType == e_StatType.MaxHp) ? e_StatType.Hp : e_StatType.Mp;
            characterStats[currentStatType] += value;
        }
    }

    public void RemoveStat(e_StatType statType, int value)
    {
        characterStats[statType] -= value;
        if (statType == e_StatType.MaxHp)
        {
            characterStats[e_StatType.Hp] = Mathf.Min(characterStats[e_StatType.Hp], characterStats[e_StatType.MaxHp]);
        }
        else
        {
            int max = (statType == e_StatType.Hp) ? GetStat(e_StatType.MaxHp) :
                    (statType == e_StatType.Mp) ? GetStat(e_StatType.MaxMp) :
                    Consts.MAX_STAT;
            characterStats[statType] = Mathf.Clamp(characterStats[statType], 0, max);
        }
    }

}
