using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackController
{
    // 총알이 발사될 위치
    [SerializeField] private Transform bulletPos;
    // 총알 프리펩
    [SerializeField] private GameObject bullet;

    public void Shot()
    {
        attackPower = state.GetStat(e_StatType.Atk);

        // 총알 생성
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        Bullet bullets = intantBullet.GetComponent<Bullet>();

        // 총알 발사 속도
        bulletRigid.velocity = bulletPos.forward * 20f;

        // 총알에게 공격력을 전해줌
        bullets.Atk = attackPower;
    }
}
