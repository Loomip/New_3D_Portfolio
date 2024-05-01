using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // �ִϸ����� ������Ʈ
    protected Animator animator;

    // ĳ���� ������Ʈ ������Ʈ
    protected CharacterState state;
    protected Health health;

    // �̵� ������Ʈ
    protected MoveController movement;

    // ���� ��� ���̾�
    [SerializeField] protected LayerMask targetLayer;

    // ���ݷ�
    protected int attackPower
    {
        get => state.GetStat(e_StatType.Atk);
        set => state.SetStat(e_StatType.Atk, value);
    }

    // ���� ���� ����
    protected bool isAttack;

    // ���� ���� ��ġ
    [SerializeField] protected Transform weaponTransfom;

    public Transform WeaponTransfom { get => weaponTransfom; set => weaponTransfom = value; }

    // ���⸦ �����ϴ� �޼ҵ�
    public void EquipWeapon(GameObject weapon)
    {
        // ������ ���Ⱑ �ִٸ� �ı�
        if (InventoryManager.instance.EquippedWeapon != null)
        {
            Destroy(InventoryManager.instance.EquippedWeapon);
        }

        // ���� ���� ������Ʈ�� �ν��Ͻ�ȭ
        InventoryManager.instance.EquippedWeapon = weapon;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        state = GetComponentInParent<CharacterState>();
        health = GetComponentInParent<Health>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("isSkill");
        }
    }
}
