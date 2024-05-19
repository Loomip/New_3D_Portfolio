using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    [Header("�κ��丮")]
    //������ ���� ������
    [SerializeField] private GameObject itemslotPrefab;
    //������ ���Ե��� ���� ��ġ
    [SerializeField] private Transform itemContent;

    [Header("��ư")]
    // ���� ��ư
    [SerializeField] private TextMeshProUGUI btn_Create_Text;
    // ���/���� ��ư 
    [SerializeField] private TextMeshProUGUI btn_Use_Text;
    // ������ ��ư
    [SerializeField] private TextMeshProUGUI btn_Discard_Text;

    [Header("�˾�")]
    //������ ���� ���õ� ����
    [SerializeField] private GameObject itemPopup;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemExplanText;
    [SerializeField] private Image tooltip_Icon;

    // �÷��̾� ���� ����
    private CharacterState player;

    // �÷��̾� �̵� ����
    private MoveController moveController;
    public MoveController MoveController { get => moveController; set => moveController = value; }

    // �������ͽ� ����
    [SerializeField] private Status status;

    //��ư�� Delegate(�븮��) : �������� ��ü���� �޴� ���
    delegate void UseButton();

    UseButton useButton;

    //������ ��ư�� ������ ȣ���ϴ� �Լ�
    public void ItemButton() => useButton.Invoke();

    // �κ��丮 ����
    Slot slot;

    // ���� �Ǿ��� ������
    ItemData selectItem;

    //�κ��丮�� ������ �ʱ�ȭ �ϴ� �޼���
    private void InitSlots()
    {
        List<Slot> slots = InventoryManager.instance.SlotList;

        for (int i = 0; i < slots.Count; i++)
        {
            slot = Instantiate(itemslotPrefab, itemContent).GetComponent<Slot>();
            slot.SLOTINDEX = i;
            slot.onItemClick += Refresh_Button;
            slot.onItemClick += SelectItem;

            // ���� ������ ������ InventoryManager�� ���� ����Ʈ�� �Ҵ�
            InventoryManager.instance.SlotList[i] = slot;
        }
    }

    // Player �κ��丮 ������ ���� (���� Ŭ�� �� ȣ��)
    public void SelectItem(ItemData selectedItem)
    {
        selectItem = selectedItem;
        OpenPopUp();
    }

    //�������� ������ ǥ�����ִ� �Լ�
    private void OpenPopUp()
    {
        if (selectItem == null)
        {
            itemPopup.SetActive(false);
            return;
        }

        var data = DataManager.instance.GetLacalizeData(selectItem.id);

        if (data != null)
        {
            itemPopup.SetActive(true);
            tooltip_Icon.color = Color.white;
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + data.SpritName);
            itemExplanText.text = data.Explan;
            itemNameText.text = data.TooltipName;
        }
        else
        {
            Debug.Log("Item data is null for id: " + selectItem.id);
        }

    }

    // ��� ���� ���
    public void Button_Equip(ItemData item)
    {
        // ������ ����� Ÿ��
        Data_Item.Param itemData = DataManager.instance.GetWeaponData(item.id);

        if (item == null)
        {
            Debug.LogError("������ �����Ͱ� �����ϴ�.");
            return;
        }

        // ���� Ÿ���� ������
        e_Weapon weaponType = GetWeaponType(itemData);

        //�ִϸ��̼� ����
        MoveController.Animator.SetInteger("WeaponState", (int)weaponType);

        // ������ �������� Ÿ�԰� ��ġ�ϴ� ��� ������ ã��
        foreach (var equipSlot in status.EquipSlotList.Values)
        {
            if (equipSlot.Type.ToString() == itemData.ItemType)
            {
                // �̹� ������ ��� �ִ��� Ȯ��
                if (equipSlot.IsEquipped)
                {
                    // ������ ��� ����
                    equipSlot.Detach();

                    Debug.Log(player.Mp);
                    Debug.Log(player.Exhaustion);
                }

                // ������ ��� ����
                equipSlot.Set(item);
            }
        }

        ClosePopup();

        InventoryManager.instance.RefreshIcon();
    }

    private e_Weapon GetWeaponType(Data_Item.Param item)
    {
        // SpritName�� ���� ���� ������ ����
        switch (item.SpritName)
        {
            case "Weapon":
                return e_Weapon.Weapon;
            case "Spell":
                return e_Weapon.Spell;
            default:
                MoveController.IsWeaponEquipped = false;
                return e_Weapon.None;
        }
    }

    // �Ҹ�ǰ ��� ���
    public void Button_Use(ItemData item)
    {
        var status = DataManager.instance.GetWeaponData(item.id);

        if (status != null)
        {
            switch (status.Name)
            {
                case "Potion":
                    player.AddHp(status.Hp);
                    UIManager.instance.RefreshHp(player.tag, player.GetComponent<Health>());
                    break;
                case "Leaf":
                    player.AddMp(status.Mp);
                    UIManager.instance.RefreshPlayerMp(player.GetComponent<Health>());
                    break;
            }
        }

        InventoryManager.instance.RemoveItem(item);

        // �������� ����� �Ŀ� �������� ������ Ȯ��
        if (item.amount < 1)
        {
            slot.ClearSlot();
            ClosePopup();
        }
    }

    // ��ư ��ȯ ���
    public void Refresh_Button(ItemData item)
    {
        if (item == null)
        {
            btn_Discard_Text.transform.parent.gameObject.SetActive(false);
            return;
        }

        btn_Discard_Text.transform.parent.gameObject.SetActive(true);

        btn_Discard_Text.text = DataManager.instance.GetWordData("Discard");

        if (DataManager.instance.GetWeaponData(item.id) != null)
        {
            string ItemType = DataManager.instance.GetWeaponData(item.id).ItemType;

            switch (ItemType)
            {
                case "Weapon":
                    btn_Use_Text.text = DataManager.instance.GetWordData("Wear");
                    useButton = () => Button_Equip(item);
                    break;

                case "Consumable":
                    btn_Use_Text.text = DataManager.instance.GetWordData("Use");
                    useButton = () => Button_Use(item);
                    break;
            }
        }
        else
        {
            Debug.Log("���õ� �����Ͱ� �����ϴ�. : " + item.id);
        }
    }

    //������ ��ư ��� (��ư�� ���� �ٴ� ���)
    private void Button_Discard()
    {
        // �������� �κ��丮���� ����
        InventoryManager.instance.RemoveItem(selectItem);

        // �������� ������ 0�̰ų� �������� null�� ���
        if (selectItem.amount < 1)
        {
            slot.ClearSlot();
            ClosePopup();
        }
    }

    // �ݱ� ��ư (��ư�� �ٴ� ���)
    public void ClosePopup()
    {
        tooltip_Icon.color = Color.clear;
        tooltip_Icon.sprite = null;
        itemExplanText.text = string.Empty;
        itemNameText.text = string.Empty;
        slot.ClearSlot();

        itemPopup.SetActive(false);
    }

    // ������ư ��� (��ư�� ���� �ٴ� ���)
    public void Btu()
    {
        // ������ ������ ������ ��������
        Data_Item.Param randomItemData = DataManager.instance.GetRandomItemDataParams();

        // ������ ������ �����ͷ� �� ������ ����
        if (randomItemData != null)
        {
            ItemData newItem = new ItemData();
            newItem.id = randomItemData.ID;
            ++newItem.amount;

            // �� �������� �κ��丮�� �߰�
            InventoryManager.instance.AddItem(newItem);
        }
        else
        {
            Debug.Log("���� ���ÿ� ����� �� �ִ� �׸� �����Ͱ� �����ϴ�.");
        }
    }

    void Start()
    {
        InitSlots();
        player = GameObject.Find("Player").GetComponent<CharacterState>();
        MoveController = GameObject.Find("Player").GetComponent<MoveController>();
        btn_Create_Text.text = DataManager.instance.GetWordData("Create");
    }

    private void OnEnable()
    {
        itemPopup.SetActive(false);
    }
}
