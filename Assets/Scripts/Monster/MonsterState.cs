using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterState : MonoBehaviour
{
    // �ִϸ����� ����
    protected Animator animator;

    // �׺� ������Ʈ ����
    protected NavMeshAgent nav;

    // ĳ���� ���� ����
    protected CharacterState state;

    // ���� ���ѻ��±�� ��Ʈ�ѷ�
    protected MonsterFSMController controller;

    // ��Ʈ �Ǹ� �ٲ� �� ���͸���
    protected SkinnedMeshRenderer meshs;

    // ü�� ������Ʈ
    protected Health monsterHp;

    // ���� ���� ���� �������̽�(�����ƴ�) �޼ҵ� ����

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void EnterState(e_MonsterState state);

    // ���� ���� ������Ʈ �߻� �޼ҵ� (���� ���� ����)
    public abstract void UpdateState();

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void ExitState();

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        state = GetComponent<CharacterState>();
        controller = GetComponent<MonsterFSMController>();
        monsterHp = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        meshs = GetComponentInChildren<SkinnedMeshRenderer>();
    }
}
