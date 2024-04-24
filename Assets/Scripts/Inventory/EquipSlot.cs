using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : Slot
{
    ItemData EquippedItem { get; set; } // �� ���Կ� ������ ������

    // ĳ���� ���� ����
    CharacterState state;

    public e_EquipType Type { get; private set; }

    // ĳ���� �����ġ ����
    AttackController weaponTransfom;

    //���⸦ ���� �ִ���
    public bool IsEquipped { get; private set; } = false;

    public void Set(ItemData data)
    {
        // ������ ������ ����
        EquippedItem = data;

        // ���� ���� ����
        IsEquipped = true;

        // ���� ������ ����
        InventoryManager.instance.RemoveItem(data);

        img_Icon.enabled = true;

        Set_Icon(data);

        // �÷��̾� ���� ������Ʈ
        var status = DataManager.instance.GetWeaponState(data.id);

        foreach (var s in status)
        {
            state.AddStat(s.Key, s.Value);
        }

        // ������ �������� ���� ������Ʈ�� �ν��Ͻ�ȭ
        string weaponName = DataManager.instance.GetWeaponData(data.id).SpritName;
        GameObject weaponPrefab = Resources.Load<GameObject>("Weapons/" + weaponName);
        GameObject weaponInstance = Instantiate(weaponPrefab, weaponTransfom.WeaponTransfom);
        weaponTransfom.EquipWeapon(weaponInstance);
    }

    public ItemData Detach()
    {
        // ���� �����Ǵ� ������ ������ �ӽ� ������ ����
        ItemData detachedItem = EquippedItem;

        var status = DataManager.instance.GetWeaponState(detachedItem.id);

        foreach (var s in status)
        {
            state.RemoveStat(s.Key, s.Value);
        }

        // ������ �������� ������
        img_Icon.sprite = null;
        img_Icon.enabled = false;

        // ������ �������� �κ��丮�� �߰�
        InventoryManager.instance.AddItem(detachedItem);

        // ������ �������� ���� ������Ʈ�� �ı�
        Destroy(InventoryManager.instance.EquippedWeapon);

        EquippedItem = null;
        IsEquipped = false;
        CurrentItem = null;

        // ���� �����Ǵ� ������ ������ ��ȯ
        return detachedItem;
    }

    private void Start()
    {
        state = GameObject.FindWithTag("Player").GetComponent<CharacterState>();
        weaponTransfom = GameObject.FindWithTag("Player").GetComponentInChildren<AttackController>();
    }
}
