using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGround : MonoBehaviour
{
    // ���� ��ȯ ����Ʈ
    [SerializeField] private GameObject bossEffectPrefab;
    // ���� ����
    [SerializeField] private GameObject bossPrefabs;
    // �� ������Ʈ
    [SerializeField] private Door doorin;
    // Player�� Ž���� BoxCollider
    [SerializeField] private BoxCollider zone;

    // ���� ��ȯ ��ġ
    [SerializeField] private Vector3 spawnAreaCenter;

    // �� ����� ���� 
    private bool isGroundStart = false;


    public void MonsterDied()
    {
        doorin.OpenDoor(); // ���� ��

        // Ŭ���� �ɶ� ���� ���
        Debug.Log("���� Ŭ����!");
    }

    IEnumerator AppearEffectAndSpawn()
    {
        //����Ʈ ��ȯ
        GameObject SummonsEffect = Instantiate(bossEffectPrefab, spawnAreaCenter, bossEffectPrefab.transform.rotation);

        yield return new WaitForSeconds(1f);

        //���� ��ȯ
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
