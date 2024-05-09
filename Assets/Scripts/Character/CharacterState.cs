using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [SerializeField] private string characterName; // 캐릭터 이름
    public string CharacterName { get => characterName; set => characterName = value; }

    private Dictionary<e_StatType, int> characterStats; // 캐릭터의 스탯을 저장

    void Start()
    {
        InitCharacterStats();
    }

    // 스탯 데이터를 사전에 저장하는 메소드
    void InitCharacterStats()
    {
        // DataManager를 사용하여 이 캐릭터의 스탯 데이터를 가져옴
        Data_Character.Param stats = DataManager.instance.GetCharacterData(CharacterName);

        // 스탯 데이터를 사전에 저장
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
        characterStats[statType] += value;
        if (statType == e_StatType.Hp && statType == e_StatType.Mp)
        {
            int max = (statType == e_StatType.Hp) ? GetStat(e_StatType.MaxHp) : GetStat(e_StatType.MaxMp);
            characterStats[statType] = Mathf.Clamp(characterStats[statType], 0, max);
        }

        // 체력과 마나가 최대치를 넘으면 최대치에 맞게 조정
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

        // 체력과 마나가 최대치를 넘으면 최대치에 맞게 조정
        characterStats[e_StatType.Hp] = Mathf.Min(characterStats[e_StatType.Hp], GetStat(e_StatType.MaxHp));
        characterStats[e_StatType.Mp] = Mathf.Min(characterStats[e_StatType.Mp], GetStat(e_StatType.MaxMp));
    }
}
