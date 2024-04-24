using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [Header("인벤토리 UI")]
    [SerializeField] private GameObject inven;

    //===================================================================================================

    [Header("대화창 UI")]
    [SerializeField] private GameObject InteractText;
    private NPCInteraction player;

    //상호작용
    void InteractableObject()
    {
        if (player.GetInterctableObject() != null)
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
        if (player.GetDoorableObject() != null)
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
    public void CloseDoor()
    {
        openDoorUI.SetActive(false);
    }

    private void Start()
    {
        player = FindObjectOfType<NPCInteraction>();
    }

    private void Update()
    {
        InteractableObject();
        DoorableObject();

        if (Input.GetKeyDown(KeyCode.I))
        {
            inven.SetActive(!inven.activeInHierarchy);

            var isShow = inven.activeInHierarchy;

            if (isShow) inven.GetComponent<InventoryMenuController>().InvenShow();
        }
    }
}
