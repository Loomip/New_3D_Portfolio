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

    // ���׵��� ���ư� ��
    [SerializeField] private float backPos;

    // ��� �Ϸ� ó�� �ð�
    [SerializeField] protected float time;
    [SerializeField] protected float deathDelayTime;

    // ��� ó�� ����Ʈ
    [SerializeField] protected GameObject destroyParticlePrefab;

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

        foreach (Rigidbody rb in ragdoll.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(-ragdoll.transform.forward, ForceMode.Impulse);
        }
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        // ��� ó�� �����ð��� �����ٸ�
        if (time >= deathDelayTime)
        {
            // ���Ͱ� �Ҹ��
            //Instantiate(destroyParticlePrefab, ragdoll.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public override void ExitState()
    {
        
    }
}
