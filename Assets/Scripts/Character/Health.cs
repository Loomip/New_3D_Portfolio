using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // 캐릭터의 능력치
    [SerializeField] private CharacterState state;
    public CharacterState State { get => state; set => state = value; }

    [SerializeField] private GameObject healthBarPrefab;

    // 체력을 담을 변수
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

    // 방어력 계산
    private int def
    {
        get => State.GetStat(e_StatType.Def);
        set => State.SetStat(e_StatType.Def, value);
    }

    // 무적 시간 
    [SerializeField] private float damageCooldown;

    // 맞았는지 
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
            // 대미지 효과
            StartCoroutine(IsHitCoroutine(damage));
        }
        else if (hp <= 0)
        {
            // 죽음
        }
    }

    IEnumerator IsHitCoroutine(int damage)
    {
        CanTakeDamage = false;

        // 대미지 이팩트 효과

        // 대미지가 들어온 만큼 체력을 깍음
        int realDamage = Mathf.Max(0, damage - def);
        hp -= realDamage;
        UIManager.instance.RefreshHp(gameObject.tag, this);

        // UImanager에서 체력 리프레쉬

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }
}
