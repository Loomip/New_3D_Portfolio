using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGround : MonoBehaviour
{
    // 보스 소환 이팩트
    [SerializeField] private GameObject bossEffectPrefab;
    // 보스 몬스터
    [SerializeField] private GameObject bossPrefabs;
    // 문 오브젝트
    [SerializeField] private Door doorin;
    // Player를 탐지할 BoxCollider
    [SerializeField] private BoxCollider zone;

    // 보스 소환 위치
    [SerializeField] private Vector3 spawnAreaCenter;

    // 맵 재시작 방지 
    private bool isGroundStart = false;


    public void MonsterDied()
    {
        doorin.OpenDoor(); // 문을 염

        // 클리어 될때 나올 기능
        Debug.Log("구역 클리어!");
    }

    IEnumerator AppearEffectAndSpawn()
    {
        //이팩트 소환
        GameObject SummonsEffect = Instantiate(bossEffectPrefab, spawnAreaCenter, bossEffectPrefab.transform.rotation);

        yield return new WaitForSeconds(1f);

        //보스 소환
        GameObject BossSummons = Instantiate(bossPrefabs, spawnAreaCenter, Quaternion.Euler(0, -90, 0));

        Destroy(SummonsEffect, 5f);

        doorin.CloseDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || !isGroundStart)
        {
            StartCoroutine(AppearEffectAndSpawn());
            isGroundStart = true;
        }
    }
}
