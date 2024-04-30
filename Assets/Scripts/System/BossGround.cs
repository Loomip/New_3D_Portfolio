using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    // ���� �� ���Խ� ������ HP
    [SerializeField] private GameObject bossHp;

    // ���� ��ȯ ��ġ
    [SerializeField] private Vector3 spawnAreaCenter;

    // �� ����� ���� 
    private bool isGroundStart = false;

    public bool IsGroundStart { get => isGroundStart; set => isGroundStart = value; }

    public void MonsterDied()
    {
        doorin.OpenDoor(); // ���� ��
        IsGroundStart = false;
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
        if (other.CompareTag("Player") || !IsGroundStart)
        {
            string CharacterName = bossPrefabs.GetComponent<CharacterState>().CharacterName;

            StartCoroutine(AppearEffectAndSpawn());
            IsGroundStart = true;
            bossHp.SetActive(true);
            bossHp.GetComponentInChildren<TextMeshProUGUI>().text = CharacterName;
        }
    }
}