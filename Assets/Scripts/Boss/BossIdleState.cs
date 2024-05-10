using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossAttackableState
{
    [SerializeField] protected float time; // �ð� ����
    [SerializeField] protected float checkTime; // ��� üũ �ð�
    [SerializeField] protected Vector2 checkTimeRange; // ��� üũ �ð� (�ּ� �ִ�)

    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(e_BossState state)
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
        if (state.Hp <= 0)
        {
            controller.TransactionToState(e_BossState.Die);
            return;
        }

        time += Time.deltaTime; // ��� �ð� ���

        if (controller.IsSkill == false)
        {
            // �÷��̾ ���� ���� �Ÿ��ȿ� ���Դٸ�
            if (controller.GetPlayerDistance() <= attackDistance)
            {
                controller.TransactionToState(e_BossState.Run);
                return;
            }

            // �÷��̾ ���� ���� �Ÿ����� �־�����
            if (controller.GetPlayerDistance() > detactDistance)
            {
                // ���� ���·� ��ȯ
                controller.TransactionToState(e_BossState.Run);
                return;
            }

            // ��� ���°� �����ٸ�
            if (time > checkTime)
            {
                // ��ȸ ���·� ��ȯ
                controller.TransactionToState(e_BossState.Run);
            }
        }
    }

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {
        time = 0;
    }
}
