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

    private bool isAvoiding = false;  // ȸ�� ������ ��Ÿ���� �÷���

    private AttackController pAttack;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        characterState = GetComponent<CharacterState>();
        health = GetComponent<Health>();
        pAttack = GetComponent<AttackController>();
    }

    public void Move()
    {
        if (isAvoiding)
        {
            // ȸ�� ���̸� ȸ�� �������� �̵�
            controller.Move(avoidPos * characterState.Spd * Time.deltaTime);
        }
        else
        {
            speed = characterState.Spd;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // �̵� ���� ���� ����
            Vector3 direction = new Vector3(h, 0f, v).normalized;

            // �ִϸ��̼� ���
            animator.SetFloat("isRun", direction.magnitude);

            // ī�޶��� y���� yȸ�������� �������� ĳ������ �ü� ���⺤�͸� ����
            direction = Quaternion.AngleAxis(camLookPoint.rotation.eulerAngles.y, Vector3.up) * direction;
            direction.Normalize(); // ���� ���͸� ����ȭ��

            if (pAttack.IsAttack)
            {
                // �̵� ���͸� ����
                movement = direction * speed * Time.deltaTime;

                // ĳ���� ��Ʈ�ѷ��� �̿��� �̵��� ����
                controller.Move(movement);

                // �̵��� �����ϰ� �ִٸ�
                if (movement != Vector3.zero)
                {
                    // ���� ���� ������ ���ʹϾ��� ��ȯ��
                    Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                    // ĳ���͸� ȸ����
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void Avoid()
    {
        if (Input.GetKeyDown(KeyCode.Space) && health.CanTakeDamage)
        {
            isAvoiding = true;  // ȸ�� ����

            health.CanTakeDamage = false;

            // ȸ�� ������ �����մϴ�.
            if (movement == Vector3.zero)
            {
                avoidPos = transform.forward;
            }
            else
            {
                avoidPos = movement.normalized;
            }

            animator.SetTrigger("isAvoid");

            // ȸ�� ���� ���� �ð�
            float dodgeDuration = 1f;

            // ���� �ð� �� ���ǵ带 �ٽ� ���� ������ ������ �ڷ�ƾ ����
            StartCoroutine(ResetSpeedAfterDelay(characterState.Spd, dodgeDuration));
        }
    }

    IEnumerator ResetSpeedAfterDelay(int originalSpeed, float delay)
    {
        float elapsedTime = 0;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;

            // ȸ�� ���� �߿� �ӵ��� ���������� ���ҽ�ŵ�ϴ�.
            float currentSpeed = Mathf.Lerp(originalSpeed * 2, originalSpeed, elapsedTime / delay);
            characterState.Spd = (int)currentSpeed;

            yield return null;
        }

        // ȸ�� ������ ���� ������ �ӵ��� ���� ������ �����ϴ�.
        characterState.Spd = originalSpeed;

        isAvoiding = false;  // ȸ�� ����
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
