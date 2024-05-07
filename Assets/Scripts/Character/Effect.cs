using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // 스킬 공격력을 전달받을 계수
    private int atk;
    public int Atk { get => atk; set => atk = value; }

    // 효과음을 나눌 Enum 타입
    [SerializeField] e_SkillSound skillSound;

    // 스킬 지속시간
    [SerializeField] private float duration;

    private void Start()
    {
        if (skillSound == e_SkillSound.melee)
        {
            SoundManager.instance.PlaySfx(e_Sfx.SwordSkill);
        }
        else if (skillSound == e_SkillSound.Ranged)
        {
            SoundManager.instance.PlaySfx(e_Sfx.BulletSkill);
        }

        // 일정시간뒤 시라짐
        StartCoroutine(DestroyAfterTime(duration));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<MonsterFSMController>().Hit();
            other.GetComponent<Health>().Hit(Atk);
        }
        else if (other.tag == "Boss")
        {
            other.GetComponent<BossFSMController>().Hit();
            other.GetComponent<Health>().Hit(Atk);
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
