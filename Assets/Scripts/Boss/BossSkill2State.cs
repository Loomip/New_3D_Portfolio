using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill2State : BossAttackableState
{
    [SerializeField] private GameObject Skill_2;
    [SerializeField] private Transform bulletPos3;


    IEnumerator ExecutePattern3()
    {
        GameObject rangeInstant = Instantiate(Skill_2, bulletPos3.position, bulletPos3.rotation);
        var evt = rangeInstant.GetComponentInChildren<ExplosionEvent>();
        evt.SetOwner(gameObject);

        yield return new WaitForSeconds(8f);
        DestroyImmediate(rangeInstant, true);
    }

    public void BossSkill_2()
    {
        StartCoroutine(ExecutePattern3());
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


    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(e_BossState state)
    {
        // �ִϸ��̼� ���
        animator.SetInteger("State", (int)state);
        controller.IsSkill = true;
        SoundManager.instance.PlaySfx(e_Sfx.BossSkill2);
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
