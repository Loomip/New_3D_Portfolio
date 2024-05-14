using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("인벤토리 UI")]
    [SerializeField] private GameObject inven;

    //===================================================================================================

    [Header("대화창 UI")]
    [SerializeField] private GameObject InteractText;
    [SerializeField] private NPC_Shop Npc;
    private NPCInteraction player;

    //상호작용
    void InteractableObject()
    {
        if (player != null && player.GetInterctableObject() != null && Npc.IsShopOpen() == false)
            InteractableShow();
        else
            InteractableHide();
    }

    // 상호작용 오브젝트를 켬
    private void InteractableShow()
    {
        InteractText.SetActive(true);
    }

    // 상호작용 오브젝트를 끔
    public void InteractableHide()
    {
        InteractText.SetActive(false);
    }

    //===================================================================================================

    [Header("상호작용 UI")]
    // 열기 UI
    [SerializeField] private GameObject openDoorUI;

    private void DoorableObject()
    {
        if (player != null && player.GetDoorableObject() != null)
            OpenDoor();
        else
            CloseDoor();
    }

    // 상호작용 오브젝트를 켬
    private void OpenDoor()
    {
        openDoorUI.SetActive(true);
    }

    // 상호작용 오브젝트를 끔
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

        // 상점이 열려있는지 확인
        if (Npc.IsShopOpen() == true)
        {
            // 상점이 열려있으면 인벤토리를 비활성화
            inven.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.I) && Npc.IsShopOpen() == false)
        {
            // 상점이 닫혀있고 'I' 키가 눌렸으면 인벤토리의 활성 상태를 토글
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
