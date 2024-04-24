using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    //상호작용 키
    void NPCInteract()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            InteractWithCurrentTarget();
        }
    }

    //상호 작용
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

    //상호작용 오브젝트
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

    private void Update()
    {
        NPCInteract();
    }
}
