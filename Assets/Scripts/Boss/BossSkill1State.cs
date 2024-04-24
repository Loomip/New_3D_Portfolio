using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill1State : BossAttackableState
{
    [SerializeField] private GameObject Skiil_1;
    [SerializeField] private Transform BulletPos2;

    //���� �ִϸ��̼� �̺�Ʈ
    public void BossSkill_1()
    {
        GameObject rangeInstant = Instantiate(Skiil_1, BulletPos2.position, BulletPos2.rotation);
        Effect effect = rangeInstant.GetComponent<Effect>();
        effect.Atk = atk;
        StartCoroutine(DestroyAfterDelay(rangeInstant, 5f));
    }

    // ���� ����� �ֽ�
    protected void LookAtTarget()
    {
        // ���� ����� ���� ������ ���
        Vector3 direction = (controller.Player.transform.position - transform.position).normalized;

        // ȸ�� ���ʹϾ� ���
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        // ���� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothValue);
    }

    IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }

    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(e_BossState state)
    {
        // �ִϸ��̼� ���
        animator.SetInteger("State", (int)state);
        controller.IsSkill = true;
    }

    // ��� ���� ��� ���� ó�� (���� ����)
    public override void UpdateState()
    {
        if (state.GetStat(e_StatType.Hp) <= 0)
        {
            controller.TransactionToState(e_BossState.Die);
            return;
        }

        LookAtTarget();

        if (controller.IsSkill == false)
        {
            if (controller.GetPlayerDistance() > detactDistance)
            {
                controller.TransactionToState(e_BossState.Idle);
                return;
            }
        }
    }

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {

    }
}
