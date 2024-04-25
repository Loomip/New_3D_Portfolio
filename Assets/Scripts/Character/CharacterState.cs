using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public string characterName; // 캐릭터 이름
    private Dictionary<e_StatType, int> characterStats; // 캐릭터의 스탯을 저장

    void Start()
    {
        InitCharacterStats();
    }

    // 스탯 데이터를 사전에 저장하는 메소드
    void InitCharacterStats()
    {
        // DataManager를 사용하여 이 캐릭터의 스탯 데이터를 가져옴
        Data_Character.Param stats = DataManager.instance.GetCharacterData(characterName);

        // 스탯 데이터를 사전에 저장
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

    // 원하는 스탯을 가져오는 메소드
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

        // MaxHP 또는 MaxMP가 증가하면 HP 또는 MP도 증가
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
