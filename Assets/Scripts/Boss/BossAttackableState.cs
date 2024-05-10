using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackableState : BossState
{
    [SerializeField] protected float attackDistance; // �÷��̾� ���� ���� �Ÿ�
    [SerializeField] protected float detactDistance; // �÷��̾� ���� ���� �Ÿ�

    // ȸ�� ���� ��ġ
    [SerializeField] protected float smoothValue;

    // ���ݷ��� ���� ����
    public int atk
    {
        get => state.Atk;
        set => state.Atk = value;
    }

    public override void EnterState(e_BossState state)
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }
}
