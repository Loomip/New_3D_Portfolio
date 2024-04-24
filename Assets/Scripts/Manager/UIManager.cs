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
    // 문 참조
    [SerializeField] private Door door;
    // 문을 열 수 있는 최대 거리
    public float detectDistance = 3f;
    // 문이 열렷는지
    private bool isOpen;

    void OpenDoorControl()
    {
        // 플레이어와 문 사이의 거리를 계산
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // 플레이어가 문 근처에 있으면 UI를 활성화
        if (distance < detectDistance)
        {
            openDoorUI.SetActive(true);

            // 플레이어가 "열기" 키를 누르면 문을 열거나 닫음
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                // 여기에 문을 열거나 닫는 코드를 추가
                door.OpenDoor(player.transform.position);
            }
        }
        else
        {
            // 플레이어가 문에서 멀어지면 UI를 비활성화
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
