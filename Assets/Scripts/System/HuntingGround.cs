using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingGround : MonoBehaviour
{
    // ���Ͱ� ��ȸ�� ��ġ�� �Ѱ��� ��ȸ��ġ
    [SerializeField] private Transform[] wanderPoints;
    public Transform[] WanderPoints { get => wanderPoints; set => wanderPoints = value; }

    [SerializeField] private List<GameObject> effectPrefabs;  // ��ȯ ����Ʈ
    [SerializeField] private List<GameObject> monsterPrefabs; // ����
    [SerializeField] private Door doorin;               // �� ������Ʈ
    [SerializeField] private Door doorOut;              // �� ������Ʈ
    [SerializeField] private BoxCollider zone;                // ������ BoxCollider
    [SerializeField] private float effectAppearTime = 2.0f;   // ��ȯ ����Ʈ�� ��Ÿ���� �ð�

    [SerializeField] private Vector3 spawnAreaCenter; // ���� ���� ���� �߽� ��ġ
    [SerializeField] private Vector3 spawnAreaSize;   // ���� ���� ���� ũ��

    [SerializeField] private int monstersToClear = 10; // Ŭ�����ϱ� ���� �ʿ��� ���� ��

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    // ���Ϳ��� ��ȸ��ġ�� �Ѱ��� �޼ҵ�
    public Transform[] GetWanderPoints()
    {
        return wanderPoints;
    }

    // ��� �� ��ȯ ����
    Vector3 GetRandomSpawnPosition()
    {
        float radius = spawnAreaSize.x / 2f; // ���� �������� spawnAreaSize�� �������� ����
        float randomAngle = Random.Range(0f, 360f); // 0���� 360 ������ ������ ����

        // ���� ������ ��ġ ��� (y ��ǥ�� ����)
        float x = spawnAreaCenter.x + radius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float z = spawnAreaCenter.z + radius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        return new Vector3(x, spawnAreaCenter.y, z);
    }

    IEnumerator SpawnEffectAndMonster()
    {
        while (GetEnemyCount() < monstersToClear)
        {
            // ���� ����Ʈ ���� �ڵ�
            GameObject selectedEffectPrefab = effectPrefabs[Random.Range(0, effectPrefabs.Count)];

            // ���� ���� ���� �ڵ�
            GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];

            // ���� ���� ��ġ ���
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();

            // ����Ʈ ����
            GameObject effectInstance = Instantiate(selectedEffectPrefab, randomSpawnPosition, selectedEffectPrefab.transform.rotation);

            // ���� ����
            GameObject monsterInstance = Instantiate(selectedMonsterPrefab, randomSpawnPosition, Quaternion.identity);

            Destroy(effectInstance, 5f);

            // MonsterState ��ũ��Ʈ�� SetHuntingGroundController �޼��带 ȣ���Ͽ� HuntingGround �ν��Ͻ� ����
            MonsterState enemy = monsterInstance.GetComponent<MonsterState>();
            if (enemy != null)
            {
                enemy.SetHuntingGroundController(this);

                enemyList.Add(enemy.gameObject);

                Debug.Log("currentMonsterCount : " + GetEnemyCount());
            }

            if (GetEnemyCount() == monstersToClear)
            {
                StopCoroutine(AppearEffectAndSpawn());
            }

            yield return null;
        }
    }

    public void MonsterDied(MonsterState _enemy)
    {
        if (enemyList.Contains(_enemy.gameObject))
        {
            enemyList.Remove(_enemy.gameObject);
        }
        else
        {
            Debug.Log("The enemy is not in the list.");
        }

        if (GetEnemyCount() <= 0)
        {
            // ��� ���Ͱ� ����Ͽ� Ŭ���� ����

            doorin.OpenDoor(); // ���� ��
            doorOut.OpenDoor(); // ���� ��

            Debug.Log("���� Ŭ����!");

            // Ŭ���� �Ŀ��� �ʱ�ȭ �Ǵ� ���� ������ �����ϴ� ���� ���� �߰�
        }

        Debug.Log("DiedcurrentMonsterCount : " + GetEnemyCount());
    }

    public int GetEnemyCount() => enemyList.Count;

    IEnumerator AppearEffectAndSpawn()
    {
        float elapsedTime = 0f;
        float startScale = 0.1f;
        float targetScale = 0.1f;

        // ����Ʈ ��Ÿ���� �ִϸ��̼�
        while (elapsedTime < effectAppearTime)
        {
            float t = elapsedTime / effectAppearTime;
            float scale = Mathf.Lerp(startScale, targetScale, t);

            foreach (GameObject effectPrefab in effectPrefabs)
            {
                effectPrefab.transform.localScale = new Vector3(scale, scale, scale);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SpawnEffectAndMonster());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AppearEffectAndSpawn());

            // ���� ��ȯ ���� ���� ����
            doorin.CloseDoor();
            doorOut.CloseDoor();
        }

    }

    //��ȯ ���� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(spawnAreaCenter.x, spawnAreaCenter.y, spawnAreaCenter.z), spawnAreaSize.x / 2f);
    }

}
