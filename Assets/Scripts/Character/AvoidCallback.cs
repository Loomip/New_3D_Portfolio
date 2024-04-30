using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCallback : MonoBehaviour
{
    [SerializeField] Health isAvoid;

    public void IsAvoid()
    {
        isAvoid.CanTakeDamage = true;
    }
}
