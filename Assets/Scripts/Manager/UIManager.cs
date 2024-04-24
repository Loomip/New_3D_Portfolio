using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [Header("�κ��丮 UI")]
    [SerializeField] private GameObject inven;

    //===================================================================================================

    [Header("��ȭâ UI")]
    [SerializeField] private GameObject InteractText;
    private NPCInteraction player;

    //��ȣ�ۿ�
    void InteractableObject()
    {
        if (player.GetInterctableObject() != null)
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
        if (player.GetDoorableObject() != null)
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
