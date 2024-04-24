using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    // 상점이 열려있는지 확인하는 메서드
    public virtual bool IsShopOpen()
    {
        return false;
    }

    public virtual void OnInteract()
    {

    }
}
