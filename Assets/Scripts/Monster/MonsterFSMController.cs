using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSMController : MonoBehaviour
{
    // ������ ���� ���� ���� ���� ������Ʈ
    [SerializeField] private MonsterState currentState;

    // ������ ��� ���� ������Ʈ��
    [SerializeField] private MonsterState[] monsterStatas;

    // �÷��̾� ����
    protected GameObject player;

    public GameObject Player { get => player; set => player = value; }

    // ��ȸ ���� ��ġ ����Ʈ��
    [SerializeField] private Transform[] wanderPoints;
    public Transform[] WanderPoints { get => wanderPoints; set => wanderPoints = value; }


    // ���� ��ȯ �޼ҵ�
    public void TransactionToState(e_MonsterState state)
    {
        currentState?.ExitState(); // ���� ���� ����
        currentState = monsterStatas[(int)state]; // ���� ��ȯ ó��
        currentState.EnterState(state); // ���ο� ���� ����
    }

    // ���� ��Ʈ�ѷ� ��ɵ�

    // �÷��̾�� ���Ͱ��� �Ÿ� ����
    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    // �÷��̾�� ������ ����
    public void Hit()
    {
        // ���� ���°� �̹� ����� ���¸� �ǰ� ó������ ����
        if (currentState == monsterStatas[(int)e_MonsterState.Die]) return;

        // �ǰ� ���·� ��ȯ
        TransactionToState(e_MonsterState.Hit);
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        // ��� ���·� ����
        TransactionToState(e_MonsterState.Idle);
    }

    private void Update()
    {
        // ���� ������ ������ ����� ����
        currentState?.UpdateState();
    }
}
