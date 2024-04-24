using System.Collections;
using System.Collections.Generic;
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

    // ���⸦ �������
    public bool isWeaponEquipped = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        characterState = GetComponent<CharacterState>();
        animator = GetComponentInChildren<Animator>();
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
        Vector3 movement = direction * speed * Time.deltaTime;

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

    void Update()
    {
        Move();

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
