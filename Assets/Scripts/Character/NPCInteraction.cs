using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] GroundController GC;

    //��ȣ�ۿ� Ű
    void NPCInteract()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            InteractWithCurrentTarget();
        }

        if (Input.GetKeyUp(KeyCode.E) && !GC.HuntiogGround.IsGroundStart && !GC.BossGround.IsGroundStart)
        {
            OpenDoorControl();
        }
    }

    //��ȣ �ۿ�
    public void InteractWithCurrentTarget()
    {
        float interactRange = 1.5f;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPC_Base npc))
            {
                npc.OnInteract();
            }
        }
    }

    //��ȣ�ۿ� ������Ʈ
    public NPC_Base GetInterctableObject()
    {
        float interactRange = 1.5f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPC_Base npc))
            {
                return npc;
            }
        }
        return null;
    }

    void OpenDoorControl()
    {
        float interactRange = 1.5f;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out Door door))
            {
                door.OpenDoor();
            }
        }
    }

    public Door GetDoorableObject()
    {
        float interactRange = 1.5f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out Door door))
            {
                return door;
            }
        }
        return null;
    }

    private void Update()
    {
        NPCInteract();
    }
}
