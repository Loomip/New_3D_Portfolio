using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    // ���߿� Ȯ�强�� ����ٰ� ����� �迭�� �Ἥ �޷�� �� ����
    [SerializeField] HuntingGround huntingGround;
    [SerializeField] BossGround bossGround;

    public HuntingGround HuntiogGround { get => huntingGround; set => huntingGround = value; }
    public BossGround BossGround { get => bossGround; set => bossGround = value; }
}
