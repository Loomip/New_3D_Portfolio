using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackController
{
    // �Ѿ��� �߻�� ��ġ
    [SerializeField] private Transform bulletPos;
    // �Ѿ� ������
    [SerializeField] private GameObject bullet;

    public void Shot()
    {
        attackPower = state.GetStat(e_StatType.Atk);

        // �Ѿ� ����
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        Bullet bullets = intantBullet.GetComponent<Bullet>();

        // �Ѿ� �߻� �ӵ�
        bulletRigid.velocity = bulletPos.forward * 20f;

        // �Ѿ˿��� ���ݷ��� ������
        bullets.Atk = attackPower;
    }
}
