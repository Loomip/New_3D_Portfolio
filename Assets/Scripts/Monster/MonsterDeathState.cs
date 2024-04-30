using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDeathState : MonsterState
{
    // �׾����� ���� ��ü ��
    [SerializeField] private GameObject body;

    // �׾����� ������ ���׵� ��
    [SerializeField] private GameObject ragdoll;

    // ��� �Ϸ� ó�� �ð�
    [SerializeField] protected float time;
    [SerializeField] protected float deathDelayTime;

    // ��� ó�� ����Ʈ
    [SerializeField] protected GameObject destroyParticlePrefab;

    // ����Ʈ ���� ��ġ
    [SerializeField] protected Transform effectPos;

    private void CopyCharacterTransformToRagdoll(Transform origin, Transform ragdoll)
    {
        for(int i = 0; i < origin.childCount; i++)
        {
            if(origin.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }
            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }

    public override void EnterState(e_MonsterState monsterState)
    {
        // �̵� ����
        nav.isStopped = true;

        CopyCharacterTransformToRagdoll(body.transform, ragdoll.transform);

        // ��ü �� ����
        body.SetActive(false);

        // ���׵� �� ����
        ragdoll.SetActive(true);
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
