using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RangedAttack : AttackController
{
    // �Ѿ��� �߻�� ��ġ
    [SerializeField] private Transform bulletPos;
    // �Ѿ� ������
    [SerializeField] private GameObject bullet;

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1) && isSkillCooldown)
        {
            // UI�� ���������� �������� ���ϰ� ����
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (health.mp >= 0)
            {
                StartCoroutine(isSkill());
            }
        }
    }

    public void Shot()
    {
        // �Ѿ� ����
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        Bullet bullets = intantBullet.GetComponent<Bullet>();

        SoundManager.instance.PlaySfx(e_Sfx.Bullet);

        // �Ѿ� �߻� �ӵ�
        bulletRigid.velocity = bulletPos.forward * 20f;

        // �Ѿ˿��� ���ݷ��� ������
        bullets.Atk = attackPower;
    }
}
