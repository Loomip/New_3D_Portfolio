using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossAttackableState
{
    // �Ѿ��� �߻�� ��ġ
    [SerializeField] private Transform bulletPos;

    // �Ѿ� ������
    [SerializeField] private GameObject bullet;

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
        bulletRigid.velocity = bulletPos.forward * 10f;

        bullets.Atk = atk;
    }

    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(e_BossState state)
    {
        // �ִϸ��̼� ���
        animator.SetInteger("State", (int)state);
        controller.IsSkill = true;
    }

    // ��� ���� ��� ���� ó�� (���� ����)
    public override void UpdateState()
    {
        if (state.GetStat(e_StatType.Hp) <= 0)
        {
            controller.TransactionToState(e_BossState.Die);
            return;
        }

        LookAtTarget();

        if (controller.IsSkill == false)
        {
            if (controller.GetPlayerDistance() > detactDistance)
            {
                controller.TransactionToState(e_BossState.Idle);
                return;
            }
        }
    }

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {

    }
}
