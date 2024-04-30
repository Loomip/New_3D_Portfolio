using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDeathState : MonsterState
{
    // 죽엇을때 꺼질 본체 모델
    [SerializeField] private GameObject body;

    // 죽엇을때 나오는 레그돌 모델
    [SerializeField] private GameObject ragdoll;

    // 사망 완료 처리 시간
    [SerializeField] protected float time;
    [SerializeField] protected float deathDelayTime;

    // 사망 처리 이펙트
    [SerializeField] protected GameObject destroyParticlePrefab;

    // 이펙트 생성 위치
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
        // 이동 중지
        nav.isStopped = true;

        CopyCharacterTransformToRagdoll(body.transform, ragdoll.transform);

        // 본체 모델 제거
        body.SetActive(false);

        // 레그돌 모델 생성
        ragdoll.SetActive(true);
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        // 사망 처리 지연시간이 지났다면
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
