using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitState : BossAttackableState
{
    // �ǰ� ��ƼŬ
    [SerializeField] protected ParticleSystem hitParticle;

    IEnumerator DamagerCoolDoun()
    {
        Material[] materialsCopy = meshs.materials;

        // �� ��Ƽ������ ������ ����
        for (int i = 0; i < materialsCopy.Length; i++)
        {
            materialsCopy[i].color = Color.red;
        }

        meshs.materials = materialsCopy;

        // �´� ����
        //SoundManager.instance.PlaySfx(e_Sfx.Hit);

        // �ǰ�ȿ�� ���
        hitParticle.Play();

        yield return new WaitForSeconds(0.2f);

        materialsCopy = meshs.materials;

        // �� ��Ƽ������ ������ ����
        for (int i = 0; i < materialsCopy.Length; i++)
        {
            materialsCopy[i].color = Color.white;
        }

        meshs.materials = materialsCopy;
    }

    public override void EnterState(e_BossState monsterState)
    {
        if (monsterHp.CanTakeDamage)
        {
            StartCoroutine(DamagerCoolDoun());
        }

        if (monsterHp.hp <= 0)
        {
            // ��� ���·� ��ȯ
            controller.TransactionToState(e_BossState.Die);
            return;
        }
    }

    public override void UpdateState()
    {
        if (state.GetStat(e_StatType.Hp) <= 0)
        {
            controller.TransactionToState(e_BossState.Die);
            return;
        }

        if (monsterHp.CanTakeDamage == false) return;

        if (controller.IsSkill == false)
        {
            if (controller.GetPlayerDistance() <= attackDistance)
            {
                controller.TransactionToState(e_BossState.Run);
            }


            if (controller.GetPlayerDistance() > detactDistance)
            {
                controller.TransactionToState(e_BossState.Run);
            }
        }


    }

    public override void ExitState()
    {
        StopCoroutine(DamagerCoolDoun());
    }
}
