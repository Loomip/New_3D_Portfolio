using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    // ������ �����ִ��� Ȯ���ϴ� �޼���
    public virtual bool IsShopOpen()
    {
        return false;
    }

    public virtual void OnInteract()
    {

    }
}
