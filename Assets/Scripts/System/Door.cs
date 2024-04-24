using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    private Transform door; // 문 오브젝트
    public float openHeight = 5f; // 문이 열릴 때의 높이
    public float openSpeed = 2f; // 문이 열리는 속도
    public float openDuration = 5f; // 문이 열려 있을 시간
    public float activationDistance = 3f; // 문을 열 수 있는 최대 거리

    private Vector3 closedPosition; // 문이 닫힌 위치
    private Vector3 openPosition; // 문이 열린 위치

    private void Start()
    {
        closedPosition = door.position;
        openPosition = new Vector3(door.position.x, door.position.y + openHeight, door.position.z);
    }

    public void OpenDoor(Vector3 pPosition)
    {
        if (Vector3.Distance(pPosition, door.position) < activationDistance)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        StartCoroutine(OpenAndCloseDoor());
    }

    private IEnumerator OpenAndCloseDoor()
    {
        // 문을 열음
        while (Vector3.Distance(door.position, openPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, openPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = openPosition;

        // 일정 시간 동안 대기
        yield return new WaitForSeconds(openDuration);

        // 문을 닫음
        while (Vector3.Distance(door.position, closedPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, closedPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = closedPosition;
    }

}
