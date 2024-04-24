using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // 캐릭터의 능력치
    [SerializeField] private CharacterState hpState;

    // 체력을 담을 변수
    public int hp
    {
        get => hpState.GetStat(e_StatType.Hp);
        set => hpState.SetStat(e_StatType.Hp, value);
    }

    // 방어력 계산
    private int def
    {
        get => hpState.GetStat(e_StatType.Def);
        set => hpState.SetStat(e_StatType.Def, value);
    }

    // 무적 시간 
    [SerializeField] private float damageCooldown;

    // 맞았는지 
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
        Debug.Log("현재 체력 : " + hp);

        // UImanager에서 체력 리프레쉬

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }
}
