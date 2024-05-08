using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHealth : Health
{
    private Animator animator;

    // ��Ʈ �Ǹ� �ٲ� �� ���͸���
    protected SkinnedMeshRenderer meshs;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private Transform regdollPosition;

    private void Start()
    {
        meshs = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRegdoll();
    }

    public override void Hit(int damage)
    {
        if (hp > 0 && CanTakeDamage)
        {
            // ����� ȿ��
            StartCoroutine(IsHitCoroutine(damage));
            StartCoroutine(DamagerCoolDoun());
        }
        else if (hp <= 0)
        {
            // ����
            animator.enabled = false;

            EnableRegdoll();

            foreach (Rigidbody rb in rigidbodies)
            {
                rb.AddExplosionForce(10, regdollPosition.position, 20f, 5f, ForceMode.Impulse);
            }
        }
    }

    IEnumerator DamagerCoolDoun()
    {
        Material[] materialsCopy = meshs.materials;

        // �� ��Ƽ������ ������ ����
        for (int i = 0; i < materialsCopy.Length; i++)
        {
            materialsCopy[i].color = Color.red;
        }

        meshs.materials = materialsCopy;

        // �´� ����
        //SoundManager.instance.PlaySfx(e_Sfx.Hit);

        yield return new WaitForSeconds(0.2f);

        materialsCopy = meshs.materials;

        // �� ��Ƽ������ ������ ����
        for (int i = 0; i < materialsCopy.Length; i++)
        {
            materialsCopy[i].color = Color.white;
        }

        meshs.materials = materialsCopy;
    }

    public void EnableRegdoll()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
            rigidbodies[i].velocity = Vector3.zero;
        }
    }

    public void DisableRegdoll()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
        }
    }
}
