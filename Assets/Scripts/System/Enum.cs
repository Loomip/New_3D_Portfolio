using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_StatType
{
    Atk,
    Def,
    Spd,
    Hp,
    MaxHp,
    Mp,
    MaxMp,
    Exhaustion,
    Cooldown
}

public enum e_MenuType
{
    None,
    Status,
    ItemSlot,
    Upgrade,
    Settings,
    Length
}

public enum e_EquipType
{
    Weapon,     //���⽽��
    Length
}

enum e_Weapon
{
    None,
    Weapon,
    Spell
}

public enum e_MonsterState
{
    Idle,       // ��û���
    Run,        // �ɾ�ٴϴ� ����
    Attack,     // ���� ����
    Hit,        // ��Ʈ ����
    Die         // ���� ����
}

public enum e_BossState
{
    Idle,       // ��û���
    Run,        // �ɾ�ٴϴ� ����
    Attack,     // ���� ����
    Skill1,     // ����1
    Skill2,     // ����2
    Hit,        // ��Ʈ ����
    Die         // ���� ����
}

