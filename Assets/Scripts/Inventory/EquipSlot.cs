using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : Slot
{
    ItemData equippedItem; // �� ���Կ� ������ ������
    public ItemData EquippedItem { get => equippedItem; set => equippedItem = value; }

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
        var status = DataManager.instance.GetWeaponData(data.id);

        state.AddAtk(status.Atk);
        state.AddDef(status.Def);
        state.AddSpd(status.Spd);
        state.AddMaxHp(status.MaxHp);
        state.AddMaxMp(status.MaxMp);
        state.AddExhaustion(status.Exhaustion);
        state.AddCooldown(status.Cooldown);

        // ü�°� ���� ��������
        UIManager.instance.RefreshHp(state.tag, state.GetComponent<Health>());
        UIManager.instance.RefreshPlayerMp(state.GetComponent<Health>());

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

        var status = DataManager.instance.GetWeaponData(detachedItem.id);

        // ����� �ɷ�ġ�� ����
        state.RemoveAtk(status.Atk);
        state.RemoveDef(status.Def);
        state.RemoveSpd(status.Spd);
        state.RemoveMaxHp(status.MaxHp);
        state.RemoveMaxMp(status.MaxMp);
        state.RemoveExhaustion(status.Exhaustion);
        state.RemoveCooldown(status.Cooldown);

        // ���� ä�°� ������ �ִ� ���� ��
        state.Hp = Mathf.Min(state.Hp, state.MaxHp);
        state.Mp = Mathf.Min(state.Mp, state.MaxMp);

        // ������ �������� ������
        img_Icon.sprite = null;
        img_Icon.enabled = false;

        // ������ �������� �κ��丮�� �߰�
        InventoryManager.instance.AddItem(detachedItem);

        // ������ �������� ���� ������Ʈ�� �ı�
        Destroy(InventoryManager.instance.EquippedWeapon);

        EquippedItem = null;
        IsEquipped = false;
        currentItem = null;

        // ���� �����Ǵ� ������ ������ ��ȯ
        return detachedItem;
    }

    private void Start()
    {
        state = GameObject.FindWithTag("Player").GetComponent<CharacterState>();
        weaponTransfom = GameObject.FindWithTag("Player").GetComponentInChildren<AttackController>();
    }
}
