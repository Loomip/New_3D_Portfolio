using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterIdleState : MonsterAttackableState
{
    [SerializeField] protected float time; // �ð� ����
    [SerializeField] protected float checkTime; // ��� üũ �ð�
    [SerializeField] protected Vector2 checkTimeRange; // ��� üũ �ð� (�ּ� �ִ�)

    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(e_MonsterState state)
    {
        // ���� üũ �ֱ� �ð��� ��÷��
        time = 0;
        checkTime = Random.Range(checkTimeRange.x, checkTimeRange.y);

        // �׺���̼� ������Ʈ �̵� ����
        nav.isStopped = true;

        // ��� �ִϸ��̼� ���
        animator.SetInteger("State", (int)state);
    }

    // ��� ���� ��� ���� ó�� (���� ����)
    public override void UpdateState()
    {
        if (state.GetStat(e_StatType.Hp) <= 0)
        {
            controller.TransactionToState(e_MonsterState.Die);
            return;
        }

        time += Time.deltaTime; // ��� �ð� ���

        // �÷��̾ ���� ���� �Ÿ��ȿ� ���Դٸ�
        if (controller.GetPlayerDistance() <= attackDistance)
        {
            // ���� ���·� ��ȯ
            controller.TransactionToState(e_MonsterState.Attack);
            return;
        }

        // �÷��̾ ���� ���� �Ÿ��ȿ� ���Ӵٸ�
        if (controller.GetPlayerDistance() <= detactDistance)
        {
            // ���� ���·� ��ȯ
            controller.TransactionToState(e_MonsterState.Run);
            return;
        }

        // ��� ���°� �����ٸ�
        if (time > checkTime)
        {
            // ��ȸ ���·� ��ȯ
            controller.TransactionToState(e_MonsterState.Run);
        }
    }

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {
        time = 0;
    }
}
