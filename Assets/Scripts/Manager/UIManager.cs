using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("�κ��丮 UI")]
    [SerializeField] private GameObject inven;

    //===================================================================================================

    [Header("��ȭâ UI")]
    [SerializeField] private GameObject InteractText;
    [SerializeField] private NPC_Shop Npc;
    private NPCInteraction player;

    //��ȣ�ۿ�
    void InteractableObject()
    {
        if (player != null && player.GetInterctableObject() != null && Npc.IsShopOpen() == false)
            InteractableShow();
        else
            InteractableHide();
    }

    // ��ȣ�ۿ� ������Ʈ�� ��
    private void InteractableShow()
    {
        InteractText.SetActive(true);
    }

    // ��ȣ�ۿ� ������Ʈ�� ��
    public void InteractableHide()
    {
        InteractText.SetActive(false);
    }

    //===================================================================================================

    [Header("��ȣ�ۿ� UI")]
    // ���� UI
    [SerializeField] private GameObject openDoorUI;

    private void DoorableObject()
    {
        if (player != null && player.GetDoorableObject() != null)
            OpenDoor();
        else
            CloseDoor();
    }

    // ��ȣ�ۿ� ������Ʈ�� ��
    private void OpenDoor()
    {
        openDoorUI.SetActive(true);
    }

    // ��ȣ�ۿ� ������Ʈ�� ��
    private void CloseDoor()
    {
        openDoorUI.SetActive(false);
    }

    //===================================================================================================

    [Header("UI")]
    [SerializeField] private Slider playerHp;
    [SerializeField] private Slider playerMp;
    [SerializeField] private Slider bossHp;

    [SerializeField] private Dictionary<Health, Slider> enemyHealthBars = new Dictionary<Health, Slider>();

    public void RegisterEnemyHealthBar(Health enemyHealth, Slider healthBar)
    {
        enemyHealthBars[enemyHealth] = healthBar;
    }

    public void RefreshHp(string tag, Health entity)
    {
        switch (tag)
        {
            case "Player":
                playerHp.value = (float)entity.hp / entity.State.MaxHp;
                break;
            case "Enemy":
                if (enemyHealthBars.TryGetValue(entity, out Slider enemyHp))
                {
                    enemyHp.value = (float)entity.hp / entity.State.MaxHp;
                }
                break;
            case "Boss":
                bossHp.value = (float)entity.hp / entity.State.MaxHp;
                break;
        }
    }

    public void RefreshPlayerMp(Health entity)
    {
        playerMp.value = (float)entity.mp / entity.State.MaxMp;
    }


    private void Start()
    {
        player = FindObjectOfType<NPCInteraction>();
    }

    private void Update()
    {
        InteractableObject();
        DoorableObject();

        // ������ �����ִ��� Ȯ��
        if (Npc.IsShopOpen() == true)
        {
            // ������ ���������� �κ��丮�� ��Ȱ��ȭ
            inven.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.I) && Npc.IsShopOpen() == false)
        {
            // ������ �����ְ� 'I' Ű�� �������� �κ��丮�� Ȱ�� ���¸� ���
            inven.SetActive(!inven.activeInHierarchy);

            var isShow = inven.activeInHierarchy;

            if (isShow)
            {
                inven.GetComponentInChildren<InventoryMenuController>().InvenShow();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
