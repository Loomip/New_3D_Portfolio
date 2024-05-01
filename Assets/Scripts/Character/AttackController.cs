using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // 애니메이터 컴포넌트
    protected Animator animator;

    // 캐릭터 스테이트 컴포넌트
    protected CharacterState state;
    protected Health health;

    // 이동 컨포넌트
    protected MoveController movement;

    // 공격 대상 레이어
    [SerializeField] protected LayerMask targetLayer;

    // 공격력
    protected int attackPower
    {
        get => state.GetStat(e_StatType.Atk);
        set => state.SetStat(e_StatType.Atk, value);
    }

    // 공격 가능 여부
    protected bool isAttack;

    // 무기 장착 위치
    [SerializeField] protected Transform weaponTransfom;

    public Transform WeaponTransfom { get => weaponTransfom; set => weaponTransfom = value; }

    // 무기를 장착하는 메소드
    public void EquipWeapon(GameObject weapon)
    {
        // 장착된 무기가 있다면 파괴
        if (InventoryManager.instance.EquippedWeapon != null)
        {
            Destroy(InventoryManager.instance.EquippedWeapon);
        }

        // 무기 게임 오브젝트를 인스턴스화
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
