using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangedAttackState : MonsterAttackableState
{
    // �Ѿ��� �߻�� ��ġ
    [SerializeField] private Transform bulletPos;

    // �Ѿ� ������
    [SerializeField] private GameObject bullet;

    // ȸ�� ���� ��ġ
    [SerializeField] protected float smoothValue;

    // ���ݷ��� ���� ����
    private int atk
    {
        get => state.Atk;
        set => state.Atk = value;
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

    public void Shot()
    {
        // �Ѿ� ����
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        Bullet bullets = intantBullet.GetComponent<Bullet>();

        // �Ѿ� �߻� �ӵ�
        bulletRigid.velocity = bulletPos.forward * 20f;

        bullets.Atk = atk;
    }

    public override void EnterState(e_MonsterState monsterState)
    {
        nav.isStopped = true;
        nav.speed = 0f;

        animator.SetInteger("State", (int)monsterState);
    }

    public override void UpdateState()
    {
        if (state.Hp <= 0)
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
