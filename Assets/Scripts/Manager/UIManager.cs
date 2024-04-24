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
    // �� ����
    [SerializeField] private Door door;
    // ���� �� �� �ִ� �ִ� �Ÿ�
    public float detectDistance = 3f;
    // ���� ���Ǵ���
    private bool isOpen;

    void OpenDoorControl()
    {
        // �÷��̾�� �� ������ �Ÿ��� ���
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // �÷��̾ �� ��ó�� ������ UI�� Ȱ��ȭ
        if (distance < detectDistance)
        {
            openDoorUI.SetActive(true);

            // �÷��̾ "����" Ű�� ������ ���� ���ų� ����
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                // ���⿡ ���� ���ų� �ݴ� �ڵ带 �߰�
                door.OpenDoor(player.transform.position);
            }
        }
        else
        {
            // �÷��̾ ������ �־����� UI�� ��Ȱ��ȭ
            openDoorUI.SetActive(false);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<NPCInteraction>();
    }

    private void Update()
    {
        InteractableObject();
        OpenDoorControl();

        if (Input.GetKeyDown(KeyCode.I))
        {
            inven.SetActive(!inven.activeInHierarchy);

            var isShow = inven.activeInHierarchy;

            if (isShow) inven.GetComponent<InventoryMenuController>().InvenShow();
        }
    }
}
