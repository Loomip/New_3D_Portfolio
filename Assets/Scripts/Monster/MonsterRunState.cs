using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRunState : MonsterAttackableState
{
    // �������� ���ƴٴ� ���� ��ġ��
    public Vector3 targetPosition = Vector3.positiveInfinity;

    // ���� ��ġ������ �������� ���� �ٴ� ��ġ
    private Vector3 randomDestination;

    // �̵� ����
    [SerializeField] private float moveInterval;

    // Player�� �ν� �ߴ���
    [SerializeField] private bool hasPlayerRecognition = false;

    Transform FindClosest(Transform currentPosition, Transform[] transforms)
    {
        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform transform in transforms)
        {
            float distance = Vector3.Distance(currentPosition.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = transform;
            }
        }

        return closestTransform;
    }


    // NavMesh ���� ������ ��ġ�� ��ȯ�ϴ� �޼ҵ�
    Vector3 RandomNavmeshLocation(Vector3 center)
    {
        // center�� �߽����� �ݰ� 10f ���� ������ ������ ����
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += center;

        // NavMeshHit ����ü�� �ʱ�ȭ
        NavMeshHit navHit;

        // ������ ������ ��ġ���� ���� ����� NavMesh�� ��ġ�� ã��
        // ã�� ��ġ�� navHit�� ����Ǹ�, �ִ� �Ÿ��� 10f, ��� NavMesh ���̾ ���
        NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas);

        // ã�� NavMesh�� ��ġ�� ��ȯ
        return navHit.position;
    }

    private void NewDerection()
    {
        Transform[] transforms = controller.WanderPoints;
        Transform currentPosition = transform;
        Transform closestTransform = FindClosest(currentPosition, transforms);

        // ���� ����� ��ȸ ����Ʈ�� �������� ������ ��ġ�� ����
        randomDestination = RandomNavmeshLocation(closestTransform.position);

        // ���õ� ��ġ�� �̵�
        nav.SetDestination(randomDestination);
    }

    public override void EnterState(e_MonsterState monsterState)
    {
        nav.speed = state.Spd;

        animator.SetInteger("State", (int)monsterState);

        nav.isStopped = false;

        NewDerection();
    }

    public override void UpdateState()
    {
        if (state.Hp <= 0)
        {
            controller.TransactionToState(e_MonsterState.Die);
            return;
        }

        // �÷��̾ ���� ���� �Ÿ��ȿ� ���Դٸ�
        if (controller.GetPlayerDistance() <= attackDistance)
        {
            // ���� ���·� ��ȯ
            controller.TransactionToState(e_MonsterState.Attack);
            return;
        }

        if (controller.GetPlayerDistance() <= detactDistance)
        {
            // ���� ��� ���� ó��
            nav.isStopped = false;
            nav.SetDestination(controller.Player.transform.position);
            hasPlayerRecognition = false;
            return;
        }

        if (controller.GetPlayerDistance() > detactDistance)
        {
            if (!hasPlayerRecognition)
            {
                NewDerection();
                hasPlayerRecognition = true;
            }

            if (Vector3.Distance(transform.position, randomDestination) < 0.1f)
            {
                controller.TransactionToState(e_MonsterState.Idle);
                return;
            }
        }
    }

    public override void ExitState()
    {

    }


}
