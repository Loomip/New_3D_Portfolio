using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    [SerializeField] private Transform target; // ĳ����
    [SerializeField] private float speed; // �ӵ�
    [SerializeField] private float radiusX; // Ÿ���� X ������
    [SerializeField] private float radiusY; // Ÿ���� Y ������
    [SerializeField] private float rotationSpeed = 10f; // ȸ�� �ӵ�
    [SerializeField] private float minY; // �ּ� y ��ġ
    [SerializeField] private float maxY; // �ִ� y ��ġ

    private float angle; // ���� ����
    private Vector3 rotationAxis; // ȸ�� ��

    private void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // ������ ȸ�� �� ����
        rotationAxis = new Vector3(Random.value, Random.value, Random.value);
    }

    private void Update()
    {
        // ���� ������Ʈ
        angle += speed * Time.deltaTime;

        // �� ��ġ ���
        float x = Mathf.Cos(angle) * radiusX;
        float z = Mathf.Sin(angle) * radiusY;
        float y = Mathf.PerlinNoise(Time.time * 0.1f , 0) * (maxY - minY) + minY; // �ε巴�� ����Ǵ� y ��ġ
        Vector3 newPos = target.position + new Vector3(x, y, z);

        // ������Ʈ ��ġ ������Ʈ
        transform.position = newPos;

        StartCoroutine(RnmdomAxis());

        // ������Ʈ ȸ�� ������Ʈ
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    IEnumerator RnmdomAxis()
    {
        // ������ ȸ�� �� ����
        rotationAxis = new Vector3(Random.value, Random.value, Random.value);

        yield return new WaitForSeconds(5f);
    }
}
