using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDeathState : MonsterState
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private Transform regdollPosition;

    // ��� �Ϸ� ó�� �ð�
    [SerializeField] protected float time;
    [SerializeField] protected float deathDelayTime;

    // ��� ó�� ����Ʈ
    [SerializeField] protected GameObject destroyParticlePrefab;

    // ����Ʈ ���� ��ġ
    [SerializeField] protected Transform effectPos;


    private void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRegdoll();
    }

    public void EnableRegdoll()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
            rigidbodies[i].velocity = Vector3.zero;
        }
    }

    public void DisableRegdoll()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
        }
    }

    public override void EnterState(e_MonsterState monsterState)
    {
        // �̵� ����
        nav.isStopped = true;

        SoundManager.instance.PlaySfx(e_Sfx.EnemyDie);

        animator.enabled = false;

        EnableRegdoll();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddExplosionForce(10, regdollPosition.position, 20f, 5f, ForceMode.Impulse);
        }
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        // ��� ó�� �����ð��� �����ٸ�
        if (time >= deathDelayTime)
        {
            huntingGround.MonsterDied(this);
            Instantiate(destroyParticlePrefab, effectPos.position, destroyParticlePrefab.transform.rotation);
            Destroy(gameObject);
        }
    }

    public override void ExitState()
    {
        
    }
}
