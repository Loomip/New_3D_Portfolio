using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // ĳ������ �ɷ�ġ
    [SerializeField] private CharacterState hpState;

    // ü���� ���� ����
    public int hp
    {
        get => hpState.GetStat(e_StatType.Hp);
        set => hpState.SetStat(e_StatType.Hp, value);
    }

    // ���� ���
    private int def
    {
        get => hpState.GetStat(e_StatType.Def);
        set => hpState.SetStat(e_StatType.Def, value);
    }

    // ���� �ð� 
    [SerializeField] private float damageCooldown;

    // �¾Ҵ��� 
    private bool canTakeDamage = true;

    public bool CanTakeDamage { get => canTakeDamage; set => canTakeDamage = value; }

    private void Start()
    {
        hpState = GetComponent<CharacterState>();
    }

    public void Hit(int damage)
    {
        if (hp > 0 && CanTakeDamage)
        {
            // ����� ȿ��
            StartCoroutine(IsHitCoroutine(damage));
        }
        else if (hp <= 0)
        {
            // ����
        }
    }

    IEnumerator IsHitCoroutine(int damage)
    {
        CanTakeDamage = false;

        // ����� ����Ʈ ȿ��

        // ������� ���� ��ŭ ü���� ����
        int realDamage = Mathf.Max(0, damage - def);
        hp -= realDamage;
        Debug.Log("���� ü�� : " + hp);

        // UImanager���� ü�� ��������

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }
}
