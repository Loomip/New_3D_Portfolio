using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    // ���߿� Ȯ�强�� ����ٰ� ����� �迭�� �Ἥ �޷�� �� ����
    [SerializeField] HuntingGround ground;

    public HuntingGround Ground { get => ground; set => ground = value; }
}
