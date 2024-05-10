using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossRunState : BossAttackableState
{
    // ���� �ߵ��� ��ų �ε���
    [SerializeField] private int skillIndex;

    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(e_BossState state)
    {
        // �׺���̼� ������Ʈ �̵�
        nav.isStopped = false;

        // �ִϸ��̼� ���
        animator.SetInteger("State", (int)state);
    }

    // ��� ���� ��� ���� ó�� (���� ����)
    public override void UpdateState()
    {
        if (state.Hp <= 0)
        {
            controller.TransactionToState(e_BossState.Die);
            return;
        }

        if (controller.GetPlayerDistance() <= attackDistance)
        {
            int selectState;

            do
            {
                selectState = Random.Range(0, 3); // ���� ���� ���� ���
            }
            while (selectState == skillIndex); // ������ ������ ��ų�� �ٸ� ��ų�� ���õ� ������ �ݺ�

            skillIndex = selectState; // ������ ��ų �ε��� ����

            switch (selectState)
            {
                case 0:
                    // �⺻ ����
                    controller.TransactionToState(e_BossState.Attack);
                    return;
                case 1:
                    // ��ų 1��
                    controller.TransactionToState(e_BossState.Skill1);
                    return;
                case 2:
                    // ��ų 2��
                    controller.TransactionToState(e_BossState.Skill2);
                    return;
            }
        }

        if(controller.GetPlayerDistance() > detactDistance)
        {
            // ���� ��� ���� ó��
            nav.SetDestination(controller.Player.transform.position);
        }
    }

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {

    }
}
