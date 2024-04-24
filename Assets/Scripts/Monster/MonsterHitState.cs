using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterHitState : MonsterAttackableState
{
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
        // �̵� ����
        nav.isStopped = true;

        StartCoroutine(DamagerCoolDoun());

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
