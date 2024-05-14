using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int atk;
    public int Atk { get => atk; set => atk = value; }

    // ������ �ٵ� ������Ʈ
    Rigidbody rigid;

    // Ÿ�� ���̾�
    [SerializeField] LayerMask layerMask = 0;

    // Ÿ�� ���̾� �Ǵ� �ڵ�
    [SerializeField] List<string> hitLayerNames;

    // �ν��� Ÿ�� Ʈ������
    Transform tfTarget;

    //�ְ� ���ǵ�
    public float speed = 0f;

    //���� ���ǵ�
    float currentSpeed = 0f;

    // Ÿ���� �����Ǿ����� ���θ� ��Ÿ���� �÷���
    public bool isTargetSet = false;

    float targetSetTime; // Ÿ���� ������ �ð�

    // �Ѿ��� ����� ���� ��ġ ����
    private Vector3 launchPosition;

    // �Ѿ��� ������ߵ� �ִ� ����
    private float maxTravelDistance = 50f;

    //����ź
    void SearchEnemy(Vector3 bulletDirection)
    {
        Collider[] searchRange = Physics.OverlapSphere(transform.position, 5f, layerMask);

        float closestAngle = 360f; // �Ѿ��� �ν��� �� �ִ� �ִ� ����

        foreach (var collider in searchRange)
        {
            // Player�� �߻��ϴ� �Ѿ��� ����ź
            if (collider.tag == "Enemy" || collider.tag == "Boss")
            {
                // �Ѿ��� Ÿ���� ���� ���� ���͸� ����
                Vector3 toTarget = collider.transform.position - transform.position;

                // �Ѿ��� Ÿ�� ������ �ü� ������ ����
                float angle = Vector3.Angle(bulletDirection, toTarget);

                if (angle < closestAngle)
                {
                    closestAngle = angle;

                    // Ÿ�� ����� ������
                    tfTarget = collider.transform;
                    isTargetSet = true;
                    targetSetTime = Time.time;
                    break;
                }
            }
        }
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 bulletDirection = rigid.velocity.normalized; // �Ѿ��� ������ ���մϴ�

        while (true)
        {
            if (!isTargetSet)
            {
                SearchEnemy(bulletDirection);

                if (Time.time - targetSetTime >= 1f)
                {
                    // Ÿ���� �������� �ʾ��� �� ���� �ð��� ������ �ı�
                    Destroy(gameObject);
                    yield break;
                }
            }
            else
            {
                yield break; // Ÿ���� �����Ǿ����Ƿ� �ڷ�ƾ ����
            }

            yield return null; // ���� �����ӱ��� ���
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // hitLayerName�� �ش��ϴ� ���̾ �ִ� ������Ʈ�� �ִ��� Ȯ��
        if ((other != null && hitLayerNames.Contains(LayerMask.LayerToName(other.gameObject.layer))) || other.tag == "Ground")
        {
            // �ش� ������Ʈ�� Health ������Ʈ�� ������
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                if (other.tag == "Enemy")
                {
                    other.GetComponent<MonsterFSMController>().Hit();
                }
                if (other.tag == "Boss")
                {
                    other.GetComponent<BossFSMController>().Hit();
                }
                other.GetComponent<Health>().Hit(Atk);
            }

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (tfTarget != null)
        {
            // �Ѿ��� ������ ������
            Vector3 tfTargets = tfTarget.position;
            tfTargets.y += 1f;
            Vector3 Direction = (tfTargets - transform.position).normalized;

            if (currentSpeed <= speed)
                currentSpeed += speed * Time.deltaTime;

            transform.position += Direction * currentSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.position, Direction, 0.25f);
        }

        // ���׷� �Ѿ��� ���¾��� ��� �Ѿ��� ��� ����ֱ� ������ ���� ���� �̻����� ��������
        // �Ѿ��� ���� �������
        if (Vector3.Distance(transform.position, launchPosition) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        targetSetTime = Time.time; // �ʱ�ȭ
        launchPosition = transform.position;
        StartCoroutine(LaunchDelay());
    }
}
