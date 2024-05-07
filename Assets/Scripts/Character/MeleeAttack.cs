using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeleeAttack : AttackController
{
    // ���� Ÿ�� �߽��� ��ġ
    [SerializeField] private Transform attackTransfom;

    // ���� ����
    [SerializeField] private float attackRadius;

    // ���� ���� ����
    [SerializeField] private float hitAngle;

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI�� ���������� �������� ���ϰ� ����
            if (EventSystem.current.IsPointerOverGameObject()) return;
            animator.SetTrigger("isAttack");
        }
    }

    // ���� �ִϸ��̼� �ǰ� �̺�Ʈ
    public void AttackHitAnimationEvent()
    {
        // Physics.OverlapSphere(�浹üũ�߽�����ġ, �浹üũ����, ����̾�);
        //  - ����ĳ��Ʈó�� �ش� �޼ҵ尡 ����Ǵ� ���� ���� �����ȿ� �ִ� �浹 ������ ������
        Collider[] hits = Physics.OverlapSphere(attackTransfom.position, attackRadius, targetLayer);

        SoundManager.instance.PlaySfx(e_Sfx.Sword);

        // �ǰݵ� ���� �� ������ ���� �ȿ� �ִ� ����� Ÿ����
        foreach (Collider hit in hits)
        {
            // �÷��̾ Ÿ���� ���� ���� ���͸� ����
            Vector3 directionToTargert = hit.transform.position - transform.position;

            // Ÿ�� ������ �ü� ������ ����
            float angleToTarget = Vector3.Angle(transform.forward, directionToTargert);

            if (angleToTarget < hitAngle)
            {
                if (hit.tag == "Enemy")
                {
                    hit.GetComponent<MonsterFSMController>().Hit();
                    hit.GetComponent<Health>().Hit(attackPower);
                }

                if (hit.tag == "Boss")
                {
                    hit.GetComponent<BossFSMController>().Hit();
                    hit.GetComponent<Health>().Hit(attackPower);
                }
            }

        }
    }


}
