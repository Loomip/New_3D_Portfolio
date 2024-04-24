using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSMController : MonoBehaviour
{
    // ������ ���� ���� ���� ���� ������Ʈ
    [SerializeField] private BossState currentState;

    // ������ ��� ���� ������Ʈ��
    [SerializeField] private BossState[] monsterStatas;

    // �÷��̾� ����
    protected GameObject player;

    public GameObject Player { get => player; set => player = value; }

    // ��ų ��� ������
    protected bool isSkill = false;
    public bool IsSkill { get => isSkill; set => isSkill = value; }

    // ���� ��ȯ �޼ҵ�
    public void TransactionToState(e_BossState state)
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
        TransactionToState(e_BossState.Hit);
    }

    // ���� ���ϸ��̼��� ������ �˾Ƽ� ���� ���¸� �ٲ� �޼ҵ�
    public void OnAnimationEnd()
    {
        TransactionToState(e_BossState.Idle);
        IsSkill = false;
    }


    void Start()
    {
        player = GameObject.FindWithTag("Player");

        // ��� ���·� ����
        TransactionToState(e_BossState.Idle);
    }

    private void Update()
    {
        // ���� ������ ������ ����� ����
        currentState?.UpdateState();
    }
}
