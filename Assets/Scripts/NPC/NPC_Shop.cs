using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Shop : NPC_Base
{
    [Header("���� UI")]
    [SerializeField] private GameObject shopObject;
    [SerializeField] private GameObject shopSlotPrefab;
    [SerializeField] private Transform shopContent;
    [SerializeField] private TextMeshProUGUI addText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI closeText;

    [Header("Player �κ��丮 UI")]
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventoryContent;

    private List<GameObject> shopSlots = new List<GameObject>(); // ������ ������ ������ ����Ʈ
    private List<Slot> inventorySlots = new List<Slot>();
    private ItemData playerSelectedItem; // Palyer �κ����� ���õ� ������
    private Data_Shop.Param selectedItem; // ���� ���õ� ������
    private ShopSlot selectedSlot;
    private Slot PlayerSlot;

    [SerializeField] private bool shopOpen = false;

    // ������ �����ִ��� Ȯ���ϴ� �޼���
    public override bool IsShopOpen()
    {
        return shopOpen;
    }

    public void SetSelectedItem(Data_Shop.Param item)  // ���õ� �������� �����ϴ� �Լ�
    {
        if (selectedSlot != null)
        {
            selectedSlot.Deselect();
        }

        selectedItem = item;
        selectedSlot = shopSlots.Find(slot => slot.GetComponent<ShopSlot>().shopData == item).GetComponent<ShopSlot>();

        if (selectedSlot != null)
        {
            selectedSlot.Select();
        }
    }

    void PlayerInven()
    {
        // �÷��̾��� ������ ����� �����ɴϴ�.
        List<Slot> playerSlots = InventoryManager.instance.SlotList;

        for (int i = 0; i < playerSlots.Count; ++i)
        {
            // ���Կ� �÷��̾��� ������ �����͸� �����մϴ�.
            PlayerSlot = Instantiate(inventorySlotPrefab, inventoryContent).GetComponent<Slot>();
            PlayerSlot.SLOTINDEX = i;

            // ������ ������ ����Ʈ�� �߰�
            inventorySlots.Add(PlayerSlot);

            PlayerSlot.onItemClick += SelectItem;
        }
    }

    // Player �κ��丮 ������ ���� (���� Ŭ�� �� ȣ��)
    public void SelectItem(ItemData selectedItem)
    {
        playerSelectedItem = selectedItem;
    }


    void RefreshIcon()
    {
        List<ItemData> dataList = InventoryManager.instance.GetItemList();

        // ��� ������ Ŭ�����մϴ�.
        foreach (Slot slot in inventorySlots)
        {
            slot.ClearSlot();
        }

        // ������ ����� ��ȸ�ϸ鼭 �� ������ �������� �����մϴ�.
        for (int i = 0; i < dataList.Count; i++)
        {
            if (InventoryManager.instance.GetItemList()[i] != null)
            {
                ItemData item = InventoryManager.instance.GetItemList()[i];
                inventorySlots[i].Set_Icon(item);
            }
        }
    }

    public override void OnInteract()
    {
        shopOpen = true;
        shopObject.SetActive(shopOpen);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        addText.text = DataManager.instance.GetWordData("Purchase");
        sellText.text = DataManager.instance.GetWordData("Sell");
        closeText.text = DataManager.instance.GetWordData("Exit");

        RefreshIcon();

        // ������ ������ ���� ����
        foreach (GameObject slot in shopSlots)
        {
            Destroy(slot);
        }

        shopSlots.Clear();

        // ���� �������� ��Ʈ�� �����ɴϴ�.
        List<Data_Shop.Param> shopItems = DataManager.instance.GetAllShopItems();

        foreach (Data_Shop.Param shopItem in shopItems)
        {
            // ���� ���� �������� �����Ͽ� ������ �����մϴ�.
            GameObject shopSlotObject = Instantiate(shopSlotPrefab, shopContent);

            // ���Կ� ���� ������ �����͸� �����մϴ�.
            ShopSlot shopSlot = shopSlotObject.GetComponent<ShopSlot>();
            shopSlot.SetShopData(shopItem, this);

            // ������ ������ ����Ʈ�� �߰�
            shopSlots.Add(shopSlotObject);
        }
    }

    // ��� (��ư�� �ٴ� ���)
    public void Buy()
    {
        if (selectedItem != null)
        {
            // �������� ������ ���� ��庸�� ������ Ȯ��
            if (selectedItem.AddPrise <= InventoryManager.instance.gold)
            {
                // ���� ��忡�� �������� ������ ����
                InventoryManager.instance.gold -= selectedItem.AddPrise;
                InventoryManager.instance.Refresh_Gold(); // ��� UI ������Ʈ

                // ���õ� ������ �����ͷ� �� ������ ����
                ItemData newItem = new ItemData();
                newItem.id = selectedItem.ID;
                ++newItem.amount;

                // �� �������� �κ��丮�� �߰�
                InventoryManager.instance.AddItem(newItem);

                // ���� �κ��丮 UI�� ������Ʈ�մϴ�.
                RefreshIcon();

                // �÷��̾� �κ��丮 UI�� ������Ʈ �մϴ�.
                InventoryManager.instance.RefreshIcon();
            }
            else
            {
                Debug.Log("���� �����ϴ�.");
            }
        }
        else
        {
            Debug.Log("�������� �������� ���õǾ����� �ʾҽ��ϴ�.");
        }
    }

    // �ȱ�
    public void SellSelected(ItemData pSelectedItem)
    {
        // ���õ� �������� �ִ��� Ȯ���մϴ�.
        if (pSelectedItem != null)
        {
            Data_Shop.Param prise = DataManager.instance.GetShopData(pSelectedItem.id);

            // ���� ��忡 �������� ������ �����ݴϴ�.
            InventoryManager.instance.gold += prise.SalePrise;
            InventoryManager.instance.Refresh_Gold(); // ��� UI ������Ʈ

            // �κ��丮���� �������� �����մϴ�.
            List<ItemData> items = InventoryManager.instance.GetItemList();
            ItemData itemToRemove = items.Find(item => item.id == pSelectedItem.id);
            if (itemToRemove != null)
            {
                // �������� ���� ����
                itemToRemove.amount -= 1;

                // �������� ������ 0�� �Ǹ� �κ��丮���� ������ ����
                if (itemToRemove.amount <= 0)
                {
                    InventoryManager.instance.RemoveItem(itemToRemove);
                    pSelectedItem = null;
                }
            }

            // ���� �κ��丮 UI�� ������Ʈ�մϴ�.
            RefreshIcon();

            // �÷��̾� �κ��丮 UI�� ������Ʈ �մϴ�.
            InventoryManager.instance.RefreshIcon();
        }
        else
        {
            Debug.Log("�κ��丮�� ���õǾ��� �������� �����ϴ�.");
        }
    }

    // ������ �Ǹ� (��ư�� �� ���)
    public void Sell()
    {
        SellSelected(playerSelectedItem);
    }

    //������ (��ư�� �ٴ� ���)
    public void Close()
    {
        shopOpen = false;
        shopObject.SetActive(shopOpen);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        PlayerInven();
    }

    private void Update()
    {
        RefreshIcon();
    }
}
