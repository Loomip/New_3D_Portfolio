using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : MonsterAttackableState
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

    public override void EnterState(e_MonsterState monsterState)
    {
        if (monsterHp.CanTakeDamage)
        {
            // �̵� ����
            nav.isStopped = true;

            StartCoroutine(DamagerCoolDoun());
        }

        if (monsterHp.hp <= 0)
        {
            // ��� ���·� ��ȯ
            controller.TransactionToState(e_MonsterState.Die);
            return;
        }
    }

    public override void UpdateState()
    {
        if (state.GetStat(e_StatType.Hp) <= 0)
        {
            controller.TransactionToState(e_MonsterState.Die);
            return;
        }

        if (monsterHp.CanTakeDamage == false) return;

        // �÷��̾ ���� ���� �Ÿ��ȿ� ���Դٸ�
        if (controller.GetPlayerDistance() <= attackDistance)
        {
            // ���� ���·� ��ȯ
            controller.TransactionToState(e_MonsterState.Attack);
            return;
        }

        if (controller.GetPlayerDistance() <= detactDistance)
        {
            // �ν� ������ ���� ���
            nav.speed = state.GetStat(e_StatType.Spd);

            nav.SetDestination(controller.Player.transform.position);
            return;
        }

        if (controller.GetPlayerDistance() > attackDistance)
        {
            controller.TransactionToState(e_MonsterState.Run);
            return;
        }
    }

    public override void ExitState()
    {
        StopCoroutine(DamagerCoolDoun());
    }
}
