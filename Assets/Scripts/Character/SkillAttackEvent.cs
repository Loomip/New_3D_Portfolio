using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackEvent : MonoBehaviour
{
    // ���� ��ų
    [SerializeField] private GameObject meleeSkill;
    // ���Ÿ� ��ų
    [SerializeField] private GameObject rangedSkill;
    // ��ų�� ���� ��ġ
    [SerializeField] private Transform skillPos;

    //�÷��̾� �ִϸ��̼� �̺�Ʈ
    void MeleeSkill()
    {
        GameObject rangeInstant = Instantiate(meleeSkill, skillPos.position, meleeSkill.transform.rotation);
        Effect effect = rangeInstant.GetComponent<Effect>();
        CharacterState state = GetComponentInParent<CharacterState>();
        effect.Atk = state.GetStat(e_StatType.Atk);
    }

    void RangedSkill()
    {
        GameObject rangeInstant = Instantiate(rangedSkill, skillPos.position, rangedSkill.transform.rotation);
        Rigidbody bulletRigid = rangeInstant.GetComponent<Rigidbody>();
        bulletRigid.velocity = skillPos.forward * 10f;
        Effect effect = rangeInstant.GetComponent<Effect>();
        CharacterState state = GetComponentInParent<CharacterState>();
        effect.Atk = state.GetStat(e_StatType.Atk);
    }
}
