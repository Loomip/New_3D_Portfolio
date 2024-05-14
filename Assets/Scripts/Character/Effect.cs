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
            var monsterController = other.GetComponent<MonsterFSMController>();
            var health = other.GetComponent<Health>();
            if (monsterController != null && health != null)
            {
                monsterController.Hit();
                health.Hit(Atk);
            }
        }

        if (other.tag == "Boss")
        {
            var bossController = other.GetComponent<BossFSMController>();
            var health = other.GetComponent<Health>();
            if (bossController != null && health != null)
            {
                bossController.Hit();
                health.Hit(Atk);
            }
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
