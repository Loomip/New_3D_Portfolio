using UnityEngine;

public class AttackSMB : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var attacking = animator.GetComponent<AttackController>().IsAttack = false;
        Debug.Log($"움직이면 안됨{attacking}");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var attacking = animator.GetComponent<AttackController>().IsAttack = true;
        Debug.Log($"움직여야됨{attacking}");
    }
}
