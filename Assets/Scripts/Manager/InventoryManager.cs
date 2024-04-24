using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//�������� ���� ID�� ������ ����
public class ItemData
{
    public int id; //������ ���� ID
    public int amount; //������ ����
    public int enhanceLevel; // �������� ��ȭ ����
}

public class InventoryManager : SingletonDontDestroy<InventoryManager>
{
    //���
    [SerializeField] TextMeshProUGUI goldText;

    // ���� ��� �� �����ϴ� ����
    public int gold;

    //��� UI�� �������� ���ִ� �Լ�
    public void Refresh_Gold()
    {
        if (goldText != null)
            goldText.text = string.Format("{0: #,##0}", gold);
    }

    public void StartGold()
    {
        gold = Consts.START_GOLD;
    }


    //============================================================================================================

    [Header("�κ��丮 �ִ� ����")]
    private int maxSlotCount = Consts.MAX_SLOT_COUNT;
    public int MAXSLOTCOUNT
    {
        get => maxSlotCount;
    }

    [Header("�κ��丮 ���� ����")]
    private int curSlotCount;
    public int CUR_SLOT_COUNT
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }

    public bool ItemExist(ItemData item)
    {
        return GetItemList().Contains(item);
    }

    // ������ ������ ����
    protected GameObject equippedWeapon;

    public GameObject EquippedWeapon { get => equippedWeapon; set => equippedWeapon = value; }

    // ���Ե��� ����Ǵ� ����Ʈ
    List<Slot> slotList = new List<Slot>();

    public List<Slot> SlotList { get => slotList; set => slotList = value; }

    //���� �κ��丮�� �ִ� ������ ����� �����ϴ� ����Ʈ
    private List<ItemData> items = new List<ItemData>();

    public List<ItemData> GetItemList()
    {
        CUR_SLOT_COUNT = items.Count;
        return items;
    }

    //�κ��丮�� ������ �ʱ�ȭ �ϴ� �޼���
    public void InitSlots()
    {
        for (int i = 0; i < MAXSLOTCOUNT; i++)
        {
            Slot slot = new Slot();
            slotList.Add(slot);
        }
    }

    //�κ��丮�� ������ �������� �����ϴ� �޼���
    public void RefreshIcon()
    {
        CUR_SLOT_COUNT = items.Count;

        for (int i = 0; i < MAXSLOTCOUNT; i++)
        {
            if (i < CUR_SLOT_COUNT)
            {
                if (items[i] != null)
                {
                    ItemData item = items[i];
                    SlotList[i].Set_Icon(item);
                }
                else
                {
                    SlotList[i].ClearSlot();
                }
            }
            else
            {
                SlotList[i].ClearSlot();
            }
        }
    }

    //�־��� �������� �κ��丮�� �ִ� ��ġ�� ã�� �޼���
    private int FindItemIndex(ItemData newItem)
    {
        int result = -1;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].id == newItem.id)
            {
                result = i;
                break;
            }
        }
        return result;
    }

    //�κ��丮�� �� �������� �߰��ϴ� �޼���
    public void AddItem(ItemData newItem)
    {
        int index = FindItemIndex(newItem);

        Data_Item.Param item = DataManager.instance.GetWeaponData(newItem.id);

        if (item != null)
        {
            if (-1 < index) //�κ��丮�� �ִ� ������
            {
                items[index].amount += 1;
            }
            else // ���� �κ��丮�� ���� ������
            {
                newItem.id = item.ID;
                newItem.amount = 1;
                items.Add(newItem);
                curSlotCount++;
            }
        }

        RefreshIcon();
    }

    // �κ��丮���� �������� ����
    public void RemoveItem(ItemData newItem)
    {
        int index = FindItemIndex(newItem);

        if (index >= 0) // �κ��丮�� �������� ������ ���
        {
            // �������� ���� ����
            items[index].amount -= 1;

            // �������� ������ 0�� �Ǹ� �κ��丮���� ������ ����
            if (items[index].amount <= 0)
            {
                items.RemoveAt(index);
                curSlotCount--;
            }
        }
        else
        {
            Debug.Log("�κ��丮�� ���� �������� �����Ϸ��� �õ��߽��ϴ�.");
        }

        RefreshIcon();
    }

    protected override void DoAwake()
    {
        // ��� �ʱ�ȭ
        StartGold();
        InitSlots();
    }
}
