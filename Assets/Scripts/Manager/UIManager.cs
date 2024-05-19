using Cinemachine;
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

    [Header("ī�޶� ������Ʈ")]
    [SerializeField] private CinemachineFreeLook camera;
    public CinemachineFreeLook Camera { get => camera; set => camera = value; }
    // ī�޶� ȸ�� �ӵ�
    [SerializeField] private float xSpeed; // m_XAxis �ӵ�
    [SerializeField] private float ySpeed; // m_YAxis �ӵ�
    // ���� �ܾƿ� �ӵ�
    [SerializeField] private float zoomSpeed; // �� �ӵ�
    [SerializeField] private float minFOV; // �ּ� FOV
    [SerializeField] private float maxFOV; // �ִ� FOV

    //===================================================================================================

    [Header("��ȭâ UI")]
    [SerializeField] private GameObject InteractText;
    [SerializeField] private NPC_Shop Npc;
    [SerializeField] private GameObject player;
    public GameObject Player { get => player; set => player = value; }

    //��ȣ�ۿ�
    void InteractableObject()
    {
        
        if (Player != null && Player.GetComponent<NPCInteraction>().GetInterctableObject() != null && Npc.IsShopOpen() == false)
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
        if (Player != null && Player.GetComponent<NPCInteraction>().GetDoorableObject() != null)
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

    // ������ �� ������ ü�¹ٸ� ������ ��ųʸ�
    [SerializeField] private Dictionary<Health, Slider> enemyHealthBars = new Dictionary<Health, Slider>();

    // ���Ϳ� �׿� �ش��ϴ� ü�¹ٸ� ��ųʸ��� ���
    public void RegisterEnemyHealthBar(Health enemyHealth, Slider healthBar)
    {
        enemyHealthBars[enemyHealth] = healthBar;
    }

    // ü�� ��������
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

    // ���� ��������
    public void RefreshPlayerMp(Health entity)
    {
        playerMp.value = (float)entity.mp / entity.State.MaxMp;
    }

    //============================================================================================================

    private void Update()
    {
        InteractableObject();
        DoorableObject();

        // ������ �����ִ��� Ȯ��
        if (Npc.IsShopOpen() == true)
        {
            // ������ ���������� �κ��丮�� ��Ȱ��ȭ
            inven.SetActive(false);
            Player.GetComponent<MoveController>().Animator.SetFloat("isRun", 0f);
            Player.GetComponent<MoveController>().enabled = false;

        }
        else if (Input.GetKeyDown(KeyCode.I) && Npc.IsShopOpen() == false)
        {
            // ������ �����ְ� 'I' Ű�� �������� �κ��丮�� Ȱ�� ���¸� ���
            inven.SetActive(!inven.activeInHierarchy);

            var isShow = inven.activeInHierarchy;

            // �κ��丮�� ������
            if (isShow)
            {
                // �κ��丮 �ʱ�ȭ
                inven.GetComponentInChildren<InventoryMenuController>().InvenShow();

                // ���콺�� ������
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // �κ��丮�� ������ 
            else
            {
                // ���콺 ����
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // ���콺 �� ��ư�� ������ 
        if (Input.GetMouseButton(2))
        {
            // ī�޶� ȸ�� ����
            Camera.m_XAxis.m_MaxSpeed = xSpeed;
            Camera.m_YAxis.m_MaxSpeed = ySpeed;
        }
        else
        {
            // ī�޶� ȸ�� ����
            Camera.m_XAxis.m_MaxSpeed = 0;
            Camera.m_YAxis.m_MaxSpeed = 0;

            // ���콺 ���� �����ӿ� ���� FOV�� �����Ͽ� �� ��/�ƿ��� ����
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            Camera.m_Lens.FieldOfView -= scrollInput * zoomSpeed;
            Camera.m_Lens.FieldOfView = Mathf.Clamp(Camera.m_Lens.FieldOfView, minFOV, maxFOV);
        }
    }
}
