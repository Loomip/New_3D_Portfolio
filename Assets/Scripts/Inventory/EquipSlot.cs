using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : Slot
{
    ItemData EquippedItem { get; set; } // 이 슬롯에 장착된 아이템

    // 캐릭터 스텟 참조
    CharacterState state;

    public e_EquipType Type { get; private set; }

    // 캐릭터 장비위치 참조
    AttackController weaponTransfom;

    //무기를 끼고 있는지
    public bool IsEquipped { get; private set; } = false;

    public void Set(ItemData data)
    {
        // 아이템 데이터 설정
        EquippedItem = data;

        // 장착 상태 변경
        IsEquipped = true;

        // 원본 아이템 제거
        InventoryManager.instance.RemoveItem(data);

        img_Icon.enabled = true;

        Set_Icon(data);

        // 플레이어 상태 업데이트
        var status = DataManager.instance.GetWeaponState(data.id);

        foreach (var s in status)
        {
            state.AddStat(s.Key, s.Value);
        }

        // 장착된 아이템의 게임 오브젝트를 인스턴스화
        string weaponName = DataManager.instance.GetWeaponData(data.id).SpritName;
        GameObject weaponPrefab = Resources.Load<GameObject>("Weapons/" + weaponName);
        GameObject weaponInstance = Instantiate(weaponPrefab, weaponTransfom.WeaponTransfom);
        weaponTransfom.EquipWeapon(weaponInstance);
    }

    public ItemData Detach()
    {
        // 장착 해제되는 아이템 정보를 임시 변수에 저장
        ItemData detachedItem = EquippedItem;

        var status = DataManager.instance.GetWeaponState(detachedItem.id);

        foreach (var s in status)
        {
            state.RemoveStat(s.Key, s.Value);
        }

        // 장착된 아이템을 제거함
        img_Icon.sprite = null;
        img_Icon.enabled = false;

        // 해제된 아이템을 인벤토리에 추가
        InventoryManager.instance.AddItem(detachedItem);

        // 장착된 아이템의 게임 오브젝트를 파괴
        Destroy(InventoryManager.instance.EquippedWeapon);

        EquippedItem = null;
        IsEquipped = false;
        CurrentItem = null;

        // 장착 해제되는 아이템 정보를 반환
        return detachedItem;
    }

    private void Start()
    {
        state = GameObject.FindWithTag("Player").GetComponent<CharacterState>();
        weaponTransfom = GameObject.FindWithTag("Player").GetComponentInChildren<AttackController>();
    }
}
