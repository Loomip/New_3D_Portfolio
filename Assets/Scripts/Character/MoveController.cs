using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // �ִϸ����� ������Ʈ
    public Animator animator;
    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    private CharacterController controller;
    // ĳ���� ������Ʈ ������Ʈ
    private CharacterState characterState;
    // ī�޶� ���� ��ġ
    [SerializeField] private Transform camLookPoint;
    // �̵� �ӵ�
    private float speed;
    // ȸ�� �ӵ�
    [SerializeField] private float rotateSpeed;
    // �߷°�
    private float gravity = 9.8f;
    // ��ġ
    private Vector3 velocity;

    // �÷��̾� ��ġ
    private Vector3 movement;

    // ȸ�� ������ ���� ��ġ
    private Vector3 avoidPos;

    // ���⸦ �������
    private bool isWeaponEquipped = false;

    public bool IsWeaponEquipped { get => isWeaponEquipped; set => isWeaponEquipped = value; }

    // ü�� ������Ʈ
    private Health health;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        characterState = GetComponent<CharacterState>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
    }

    public void Move()
    {
        speed = characterState.GetStat(e_StatType.Spd);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ���� ���� ����
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        // �ִϸ��̼� ���
        animator.SetFloat("isRun", direction.magnitude);

        // ī�޶��� y���� yȸ�������� �������� ĳ������ �ü� ���⺤�͸� ����
        direction = Quaternion.AngleAxis(camLookPoint.rotation.eulerAngles.y, Vector3.up) * direction;
        direction.Normalize(); // ���� ���͸� ����ȭ��

        // �̵� ���͸� ����
        movement = direction * speed * Time.deltaTime;

        // ĳ���� ��Ʈ�ѷ��� �̿��� �̵��� ����
        controller.Move(movement);

        // �̵��� �����ϰ� �ִ� ���̶��
        if (movement != Vector3.zero)
        {
            // ���� ���� ������ ���ʹϾ��� ��ȯ��
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // ĳ���͸� ȸ����
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void Avoid()
    {
        if (Input.GetKeyDown(KeyCode.Space) && health.CanTakeDamage)
        {
            health.CanTakeDamage = false;

            // ȸ�� ���� ����
            avoidPos = transform.forward;

            // ȸ�� ���� �� ���ǵ� ����
            int dodgeSpeed = characterState.GetStat(e_StatType.Spd) * 2;

            characterState.SetStat(e_StatType.Spd, dodgeSpeed);

            animator.SetTrigger("isAvoid");

            // ȸ�� ���� ���� �ð�
            float dodgeDuration = 0.8f;

            // ���� �ð� �� ���ǵ带 �ٽ� ���� ������ ������ �ڷ�ƾ ����
            StartCoroutine(ResetSpeedAfterDelay(characterState.GetStat(e_StatType.Spd) / 2, dodgeDuration));
        }
    }

    IEnumerator ResetSpeedAfterDelay(int originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���ǵ带 ���� ������ �����ϴ�.
        characterState.SetStat(e_StatType.Spd, originalSpeed);
    }

    void Update()
    {
        Move();
        Avoid();

        if (controller.isGrounded == false)
        {
            // �߷��� ����
            velocity.y -= gravity * Time.deltaTime;

            // �߷��� ������ ���͸� �̵��� ����
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            // ĳ���Ͱ� ���� ������ y ���͸� 0���� ����
            velocity.y = 0;
        }
    }
}
