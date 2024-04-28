using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    // 나중에 확장성은 여기다가 사냥터 배열을 써서 쭈루룩 할 예정
    [SerializeField] HuntingGround ground;

    public HuntingGround Ground { get => ground; set => ground = value; }
}
