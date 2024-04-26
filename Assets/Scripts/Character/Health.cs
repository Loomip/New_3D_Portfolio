using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // ĳ������ �ɷ�ġ
    [SerializeField] private CharacterState state;
    public CharacterState State { get => state; set => state = value; }

    [SerializeField] private GameObject healthBarPrefab;

    // ü���� ���� ����
    public int hp
    {
        get => State.GetStat(e_StatType.Hp);
        set => State.SetStat(e_StatType.Hp, value);
    }

    public int mp
    {
        get => State.GetStat(e_StatType.Mp);
        set => State.SetStat(e_StatType.Mp, value);
    }

    // ���� ���
    private int def
    {
        get => State.GetStat(e_StatType.Def);
        set => State.SetStat(e_StatType.Def, value);
    }

    // ���� �ð� 
    [SerializeField] private float damageCooldown;

    // �¾Ҵ��� 
    private bool canTakeDamage = true;

    public bool CanTakeDamage { get => canTakeDamage; set => canTakeDamage = value; }

    private void Start()
    {
        if (gameObject.tag == "Enemy")
        {
            GameObject healthBarObject = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            Slider healthBar = healthBarObject.GetComponent<Slider>();

            UIManager.instance.RegisterEnemyHealthBar(this, healthBar);
        }
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
        UIManager.instance.RefreshHp(gameObject.tag, this);

        // UImanager���� ü�� ��������

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }
}
