using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Health : MonoBehaviour
{
    // ĳ������ �ɷ�ġ
    [SerializeField] private CharacterState state;

    public CharacterState State { get => state; set => state = value; }

    // ü���� ���� ����
    public int hp
    {
        get => State.Hp;
        set => State.Hp = value;
    }

    public int mp
    {
        get => State.Mp;
        set => State.Mp = value;
    }

    // ���� ���
    private int def
    {
        get => State.Def; 
        set => State.Def = value;
    }

    // ������ ��ð� 
    [SerializeField] private float damageCooldown;

    // �¾Ҵ��� 
    private bool canTakeDamage = true;

    public bool CanTakeDamage { get => canTakeDamage; set => canTakeDamage = value; }

    public abstract void Hit(int damage);

    public IEnumerator IsHitCoroutine(int damage)
    {
        CanTakeDamage = false;

        // ������� ���� ��ŭ ü���� ����
        int realDamage = Mathf.Max(0, damage - def);
        hp -= realDamage;

        // UImanager���� ü�� ��������
        UIManager.instance.RefreshHp(gameObject.tag, this);

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }
}
