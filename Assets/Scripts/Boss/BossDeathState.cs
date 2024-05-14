using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossAttackableState
{
    // ��� �Ϸ� ó�� �ð�
    [SerializeField] protected float time;
    [SerializeField] protected float deathDelayTime;

    // ��� ó�� ����Ʈ
    [SerializeField] protected GameObject destroyParticlePrefab;

    public override void EnterState(e_BossState state)
    {
        // �̵� ����
        nav.isStopped = true;

        // �ִϸ��̼� ���
        animator.SetInteger("State", (int)state);

        animator.SetBool("Die", true);
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        // ��� ó�� �����ð��� �����ٸ�
        if (time >= deathDelayTime)
        {
            // ���Ͱ� �Ҹ��
           
            bossGround.MonsterDied();
            Instantiate(destroyParticlePrefab, transform.position, destroyParticlePrefab.transform.rotation);
            Destroy(gameObject);
        }
    }

    public override void ExitState()
    {

    }
}
