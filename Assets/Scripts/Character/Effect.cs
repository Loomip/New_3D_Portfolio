using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // ��ų ���ݷ��� ���޹��� ���
    private int atk;
    public int Atk { get => atk; set => atk = value; }

    // ȿ������ ���� Enum Ÿ��
    [SerializeField] e_SkillSound skillSound;

    // ��ų ���ӽð�
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

        // �����ð��� �ö���
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
