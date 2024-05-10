using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("���")]
    //��� ���� ������
    [SerializeField] private GameObject equipslotPrefab;
    //������ ��� ���Ե��� ���� ��ġ
    [SerializeField] private Transform equiplist;

    [Header("�÷��̾� ����")]
    // ü��
    [SerializeField] private TextMeshProUGUI hpText;
    // ����
    [SerializeField] private TextMeshProUGUI mpText;
    // ���ݷ�
    [SerializeField] private TextMeshProUGUI atkText;
    // ����
    [SerializeField] private TextMeshProUGUI defText;

    [Header("��ư")]
    [SerializeField] private TextMeshProUGUI but_Unbind_Text;

    // �κ��丮 ����
    [SerializeField] private ItemSlot itemSlot;

    // �÷��̾� ���� ����
    CharacterState player;

    // ��� ���� ����
    EquipSlot equipslot;

    // ��� Ÿ�Կ� ���� ��������� ���� ��ųʸ�
    Dictionary<e_EquipType, EquipSlot> equipSlotList = new Dictionary<e_EquipType, EquipSlot>();

    public Dictionary<e_EquipType, EquipSlot> EquipSlotList { get => equipSlotList; set => equipSlotList = value; }

    //��� ���� ����
    private void InitEquipSlots()
    {
        for (int i = 0; i < (int)e_EquipType.Length; ++i)
        {
            equipslot = Instantiate(equipslotPrefab, equiplist).GetComponent<EquipSlot>();
            equipslot.ClearSlot();
            EquipSlotList.Add((e_EquipType)i, equipslot);

            equipslot.onItemClick += OnItemClick;
        }
    }

    // ĳ���� ���� ��������
    public void Refresh_Stat()
    {
        hpText.text = "HP: " + player.Hp + " / " + player.MaxHp;
        mpText.text = "MP: " + player.Mp + " / " + player.MaxMp;
        atkText.text = "Attack : " + player.Atk;
        defText.text = "Defense: " + player.Def;
    }

    // ������ Ŭ�� �̺�Ʈ �ڵ鷯
    public void OnItemClick(ItemData item)
    {
        // ��ư Ȱ��ȭ
        but_Unbind_Text.transform.parent.gameObject.SetActive(item != null);

        // ��ư �̸� ����
        but_Unbind_Text.text = DataManager.instance.GetWordData("Unbind");

        // ���� ���õ� ������ ����
        equipslot.CurrentItem = item;
    }

    // ��� �����ϱ� (��ư�� ���� �� �޼ҵ�)
    public void UnbindItem()
    {
        if (equipslot.CurrentItem != null)
        {
            string item = DataManager.instance.GetWeaponData(equipslot.CurrentItem.id).ItemType;

            if (item != null && item == "Weapon")
            {
                // ������ EquipSlot�� ��쿡�� ������ ����
                if (equipslot != null)
                {
                    equipslot.Detach();
                    itemSlot.MoveController.animator.SetInteger("WeaponState", (int)e_Weapon.None);
                    UIManager.instance.RefreshHp(player.tag, player.GetComponent<Health>());
                    UIManager.instance.RefreshPlayerMp(player.GetComponent<Health>());
                }

                // ���� ���õ� ������ �ʱ�ȭ
                equipslot.CurrentItem = null;
            }
        }
        else
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
        }

    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<CharacterState>();
        InitEquipSlots();
        OnItemClick(equipslot.CurrentItem);
    }

    private void Update()
    {
        Refresh_Stat();
    }

    private void OnEnable()
    {
        but_Unbind_Text.transform.parent.gameObject.SetActive(false);
    }
}
