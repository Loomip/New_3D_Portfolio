using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [Header("��ġ")]
    //������ ���Ե��� ���� ��ġ
    public Transform enhanceContent;

    [Header("������")]
    //������ ���� ������
    public GameObject slotPrefab;

    [Header("��ư")]
    //���/���� ��ư 
    public TextMeshProUGUI btn_Enhance_Text;

    //���õ� �������� �̹���
    public Image tooltip_Icon;

    // �ý��� �޽���
    [SerializeField] private TextMeshProUGUI systemMessageText;

    // ��ȭ �ܰ躰 Ȯ��
    private float[] enhanceChances = new float[] { 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };

    // ��ȭ �ܰ� (0 ~ 4: 0�̸� 1��, 4�� 5��)
    private int enhanceLevel = 0;

    Slot slot;

    Data_Item.Param m_Item;

    List<Slot> slotList = new List<Slot>();

    //��ư�� Delegate(�븮��)
    delegate void UseButton();

    UseButton useButton;

    //������ ��ư�� ������ ȣ���ϴ� �Լ�
    public void ItemButton() => useButton.Invoke();

    //��ȭ�� �κ��丮
    private void InitSlots()
    {
        List<Slot> slots = InventoryManager.instance.SlotList;

        for (int i = 0; i < slots.Count; i++)
        {
            slot = Instantiate(slotPrefab, enhanceContent).GetComponent<Slot>();
            slot.SLOTINDEX = i;
            slot.onItemClick += Refresh_Tooltip;
            slot.onItemClick += Refresh_Button;
            slotList.Add(slot);
        }
    }

    //�κ��丮�� ������ �������� �����ϴ� �޼���
    private void RefreshIcon()
    {
        List<ItemData> dataList = InventoryManager.instance.GetItemList();

        int weaponIndex = 0; // ���� �����۸��� ī��Ʈ�ϴ� ���ο� �ε��� ����

        for (int i = 0; i < dataList.Count; i++)
        {
            string ItemType = DataManager.instance.GetWeaponData(dataList[i].id).ItemType;
            int baseItemId = (dataList[i].id / 1000) * 1000;
            enhanceLevel = dataList[i].id - baseItemId;

            if (ItemType == e_EquipType.Weapon.ToString() && enhanceLevel < Consts.MAX_ENHANCE_LEVEL)
            {
                if (weaponIndex < slotList.Count)
                {
                    slotList[weaponIndex].Set_Icon(dataList[i]);
                    weaponIndex++;
                }
            }
        }

        // ���� ���Ե��� �ʱ�ȭ�մϴ�.
        for (int i = weaponIndex; i < slotList.Count; i++)
        {
            slotList[i].ClearSlot();
        }
    }

    //�������� ������ ǥ�����ִ� �Լ�
    private void Refresh_Tooltip(ItemData item)
    {
        if (item == null)
        {
            tooltip_Icon.color = Color.clear;
            tooltip_Icon.sprite = null;
        }
        else
        {
            m_Item = DataManager.instance.GetWeaponData(item.id);
            tooltip_Icon.color = Color.white;
            tooltip_Icon.sprite = Resources.Load<Sprite>("Itemicons/" + m_Item.SpritName);
        }
    }

    //��ư�� �ٲ��ִ� �Լ�
    private void Refresh_Button(ItemData item)
    {
        if (item == null)
        {
            btn_Enhance_Text.transform.parent.gameObject.SetActive(false);
            return;
        }

        btn_Enhance_Text.transform.parent.gameObject.SetActive(true);

        if (DataManager.instance.GetWeaponData(item.id) != null)
        {
            string ItemType = DataManager.instance.GetWeaponData(item.id).ItemType;

            switch (ItemType)
            {
                case "Weapon":
                    btn_Enhance_Text.text = DataManager.instance.GetWordData("Enhance");
                    useButton = () => Button_Enhance(item);
                    break;
            }
        }
    }

    // ��ȭ ��� (��ư�� �ٴ� ���)
    public void Button_Enhance(ItemData itme)
    {
        if (itme == null)
        {
            btn_Enhance_Text.transform.parent.gameObject.SetActive(false);
            return;
        }

        // �������� ���� �κ��丮�� �ִ��� Ȯ��
        if (!InventoryManager.instance.ItemExist(itme))
        {
            Debug.Log("The item does not exist in the inventory!");
            return;
        }

        // ������ ������ ���� �⺻ ������ ID�� ������
        int baseItemId = (itme.id / 1000) * 1000;

        // ������ ID�� ������� ��ȭ ������ ���
        int itemEnhanceLevel = itme.id - baseItemId;

        // ��ȭ ������ enhanceChances �迭�� ������ ����� ��ȭ�� �õ����� ����
        if (itemEnhanceLevel >= enhanceChances.Length)
        {
            StartCoroutine(ShowSystemMessage(DataManager.instance.GetWordData("Max")));
            return;
        }

        // ��ȭ Ȯ���� ����
        float enhanceChance = enhanceChances[itemEnhanceLevel];

        // ������ ���ڸ� ���ؼ� ��ȭ Ȯ���� ��
        if (Random.value <= enhanceChance)
        {
            // ��ȭ ���� �� ����
            EnhanceItem(itme);

            // ��ȭ ���� �޽��� ǥ��
            StartCoroutine(ShowSystemMessage(DataManager.instance.GetWordData("Success")));

            // ��ȭ ���� �� ���� ��Ȱ��ȭ
            Refresh_Tooltip(null);
            Refresh_Button(null);

            // ������ ����Ʈ ������Ʈ
            RefreshIcon();
        }
        else
        {
            // ��ȭ ���� �޽��� ǥ��
            StartCoroutine(ShowSystemMessage(DataManager.instance.GetWordData("Failed")));
        }
    }

    private IEnumerator ShowSystemMessage(string message)
    {
        // �޽����� �̹����� ǥ��
        systemMessageText.text = message;
        systemMessageText.enabled = true;

        yield return null;
    }

    private void EnhanceItem(ItemData item)
    {
        // ��ȭ ���� ����
        enhanceLevel++;

        // ��ȭ�� �������� ���ο� ���Կ� �߰�
        AddEnhancedItemToNewSlot(item);

        // ���� ������ ����
        InventoryManager.instance.RemoveItem(item);

        // ������ �ʱ�ȭ
        slot.ClearSlot();
    }

    private void AddEnhancedItemToNewSlot(ItemData item)
    {
        // ��ȭ�� ������ ID�� ����
        int enhancedItemId = item.id + 1;

        // ��ȭ�� ������ �����͸� �ҷ���
        Data_Item.Param enhancedItemData = DataManager.instance.GetWeaponData(enhancedItemId);

        // �ҷ��� ������ �����Ͱ� ���ٸ� ��ȭ�� �������� ���� ���̹Ƿ� ����
        if (enhancedItemData == null)
        {
            Debug.Log("No item data for the enhanced item ID: " + enhancedItemId);
            return;
        }

        // ���ο� ������ �����͸� ����
        ItemData newItem = new ItemData
        {
            id = enhancedItemId
        };

        // ���ο� ���Կ� ������ �߰�
        InventoryManager.instance.AddItem(newItem);

        // ���Ӱ� �߰��� ������ ������ ��������
        RefreshIcon();
    }

    private void Start()
    {
        InitSlots();
        tooltip_Icon.enabled = true;
        Refresh_Tooltip(slot.CurrentItem);
        Refresh_Button(slot.CurrentItem);
    }

    private void OnEnable()
    {
        systemMessageText.enabled = false;
    }

    private void Update()
    {
        RefreshIcon();
    }
}
