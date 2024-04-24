using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMeleeAttackState : MonsterAttackableState
{
    // ���� Ÿ�� �߽��� ��ġ
    [SerializeField] private Transform attackTransfom;

    // ���� ����
    [SerializeField] private float attackRadius;

    // ���� ���� ����
    [SerializeField] private float hitAngle;

    // ���� ��� ���̾�
    [SerializeField] protected LayerMask targetLayer;

    // ȸ�� ���� ��ġ
    [SerializeField] protected float smoothValue;

    // ���ݷ��� ���� ����
    private int atk
    {
        get => state.GetStat(e_StatType.Atk);
        set => state.SetStat(e_StatType.Atk, value);
    }

    // ���� ����� �ֽ�
    protected void LookAtTarget()
    {
        // ���� ����� ���� ������ ���
        Vector3 direction = (controller.Player.transform.position - transform.position).normalized;

        // ȸ�� ���ʹϾ� ���
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        // ���� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothValue);
    }

    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackTransfom.position, attackRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTargert = hit.transform.position - transform.position;

            float angleToTarget = Vector3.Angle(transform.forward, directionToTargert);

            if (angleToTarget < hitAngle)
            {
                // Player �ǰ� ó��
                if (hit.tag == "Player")
                {
                    hit.GetComponent<Health>().Hit(atk);
                }
            }
        }
    }

    public override void EnterState(e_MonsterState monsterState)
    {
        nav.isStopped = true;
        nav.speed = 0f;

        animator.SetInteger("State", (int)monsterState);
    }

    public override void UpdateState()
    {
        if (state.GetStat(e_StatType.Hp) <= 0)
        {
            controller.TransactionToState(e_MonsterState.Die);
            return;
        }

        if (controller.GetPlayerDistance() > attackDistance)
        {
            controller.TransactionToState(e_MonsterState.Run);
            return;
        }

        LookAtTarget();
    }

    public override void ExitState()
    {

    }
}
