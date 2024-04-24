using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    public float openHeight = 5f; // 문이 열릴 때의 높이
    public float openSpeed = 2f; // 문이 열리는 속도
    public float openDuration = 5f; // 문이 열려 있을 시간
    public float activationDistance = 3f; // 문을 열 수 있는 최대 거리

    private Vector3 closedPosition; // 문이 닫힌 위치
    private Vector3 openPosition; // 문이 열린 위치

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = new Vector3(transform.position.x, transform.position.y + openHeight, transform.position.z);
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    public void CloseDoor()
    {
        StopCoroutine(CloseDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        // 문을 열음
        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = openPosition;
    }

    private IEnumerator CloseDoorCoroutine()
    {
        // 문을 닫음
        while (Vector3.Distance(transform.position, closedPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = closedPosition;
    }

}
