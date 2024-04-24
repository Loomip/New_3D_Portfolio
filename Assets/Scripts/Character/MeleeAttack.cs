using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackController
{
    // ���� Ÿ�� �߽��� ��ġ
    [SerializeField] private Transform attackTransfom;

    // ���� ����
    [SerializeField] private float attackRadius;

    // ���� ���� ����
    [SerializeField] private float hitAngle;

    // ���� �ִϸ��̼� �ǰ� �̺�Ʈ
    public void AttackHitAnimationEvent()
    {
        // Physics.OverlapSphere(�浹üũ�߽�����ġ, �浹üũ����, ����̾�);
        //  - ����ĳ��Ʈó�� �ش� �޼ҵ尡 ����Ǵ� ���� ���� �����ȿ� �ִ� �浹 ������ ������
        Collider[] hits = Physics.OverlapSphere(attackTransfom.position, attackRadius, targetLayer);

        // �ǰݵ� ���� �� ������ ���� �ȿ� �ִ� ����� Ÿ����
        foreach (Collider hit in hits)
        {
            // �÷��̾ Ÿ���� ���� ���� ���͸� ����
            Vector3 directionToTargert = hit.transform.position - transform.position;

            // Ÿ�� ������ �ü� ������ ����
            float angleToTarget = Vector3.Angle(transform.forward, directionToTargert);

            if(angleToTarget < hitAngle)
            {
                if(hit.tag == "Enemy")
                {
                    attackPower = state.GetStat(e_StatType.Atk);
                    hit.GetComponent<MonsterFSMController>().Hit();
                    hit.GetComponent<Health>().Hit(attackPower);
                }

                if (hit.tag == "Boss")
                {
                    attackPower = state.GetStat(e_StatType.Atk);
                    hit.GetComponent<BossFSMController>().Hit();
                    hit.GetComponent<Health>().Hit(attackPower);
                }
            }

        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttack");
        }
    }
}
