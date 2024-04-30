using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // 애니메이터 컴포넌트
    public Animator animator;
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController controller;
    // 캐릭터 스테이트 컴포넌트
    private CharacterState characterState;
    // 카메라 참조 위치
    [SerializeField] private Transform camLookPoint;
    // 이동 속도
    private float speed;
    // 회전 속도
    [SerializeField] private float rotateSpeed;
    // 중력값
    private float gravity = 9.8f;
    // 위치
    private Vector3 velocity;

    // 플레이어 위치
    private Vector3 movement;

    // 회피 방향을 담을 위치
    private Vector3 avoidPos;

    // 무기를 들었는지
    private bool isWeaponEquipped = false;

    public bool IsWeaponEquipped { get => isWeaponEquipped; set => isWeaponEquipped = value; }

    // 체력 컴포넌트
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

        // 이동 방향 벡터 생성
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        // 애니메이션 재생
        animator.SetFloat("isRun", direction.magnitude);

        // 카메라의 y축의 y회전각도을 기준으로 캐릭터의 시선 방향벡터를 설정
        direction = Quaternion.AngleAxis(camLookPoint.rotation.eulerAngles.y, Vector3.up) * direction;
        direction.Normalize(); // 방향 벡터를 정규화함

        // 이동 벡터를 설정
        movement = direction * speed * Time.deltaTime;

        // 캐릭터 컨트롤러를 이용한 이동을 수행
        controller.Move(movement);

        // 이동을 수행하고 있는 중이라면
        if (movement != Vector3.zero)
        {
            // 현재 방향 기준의 쿼터니언을 반환함
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // 캐릭터를 회전함
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void Avoid()
    {
        if (Input.GetKeyDown(KeyCode.Space) && health.CanTakeDamage)
        {
            health.CanTakeDamage = false;

            // 회피 방향 설정
            avoidPos = transform.forward;

            // 회피 동작 중 스피드 적용
            int dodgeSpeed = characterState.GetStat(e_StatType.Spd) * 2;

            characterState.SetStat(e_StatType.Spd, dodgeSpeed);

            animator.SetTrigger("isAvoid");

            // 회피 동작 지속 시간
            float dodgeDuration = 0.8f;

            // 일정 시간 후 스피드를 다시 원래 값으로 돌리는 코루틴 시작
            StartCoroutine(ResetSpeedAfterDelay(characterState.GetStat(e_StatType.Spd) / 2, dodgeDuration));
        }
    }

    IEnumerator ResetSpeedAfterDelay(int originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 스피드를 원래 값으로 돌립니다.
        characterState.SetStat(e_StatType.Spd, originalSpeed);
    }

    void Update()
    {
        Move();
        Avoid();

        if (controller.isGrounded == false)
        {
            // 중력을 적용
            velocity.y -= gravity * Time.deltaTime;

            // 중력을 적용한 벡터를 이동에 적용
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            // 캐릭터가 땅에 닿으면 y 벡터를 0으로 리셋
            velocity.y = 0;
        }
    }
}
