using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRegdoll : MonoBehaviour
{
    [SerializeField] protected Transform boneRoot;

    public void CopyCharacterTransformToRagdoll(Transform origin)
    {
        for (int i = 0; i < boneRoot.childCount; i++)
        {
            if (boneRoot.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i));
            }
            boneRoot.GetChild(i).localPosition = origin.GetChild(i).localPosition;
        }
    }
}
