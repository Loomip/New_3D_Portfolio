using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BossState : MonoBehaviour
{
    // �ִϸ����� ����
    protected Animator animator;

    // �׺� ������Ʈ ����
    protected NavMeshAgent nav;
    public NavMeshAgent Nav { get => nav; set => nav = value; }

    // ĳ���� ���� ����
    protected CharacterState state;

    // ���� ���ѻ��±�� ��Ʈ�ѷ�
    protected BossFSMController controller;
    public BossFSMController Controller { get => controller; set => controller = value; }

    // ��Ʈ �Ǹ� �ٲ� �� ���͸���
    protected SkinnedMeshRenderer meshs;

    // ü�� ������Ʈ
    protected Health monsterHp;

    // ���� ���� ���� �������̽�(�����ƴ�) �޼ҵ� ����

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void EnterState(e_BossState state);

    // ���� ���� ������Ʈ �߻� �޼ҵ� (���� ���� ����)
    public abstract void UpdateState();

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void ExitState();

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        state = GetComponent<CharacterState>();
        controller = GetComponent<BossFSMController>();
        monsterHp = GetComponent<Health>();
        animator = GetComponent<Animator>();
        meshs = GetComponentInChildren<SkinnedMeshRenderer>();
    }
}
