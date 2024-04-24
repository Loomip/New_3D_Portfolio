using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    // 문이 열리는 속도
    public float openSpeed = 2f; 
    // 문을 열 수 있는 최대 거리
    public float activationDistance = 3f; 

    // 문이 닫힌 위치
    private Vector3 closedPosition; 
    // 문이 열린 위치
    private Vector3 openPosition; 

    // 문이 닫혀 있는지
    private bool isDoorClosed = true;

    private void Start()
    {
        closedPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        openPosition = new Vector3(transform.position.x, 10f, transform.position.z);
    }

    public void CloseDoor()
    {
        if (!isDoorClosed)
        {
            StartCoroutine(CloseDoorCoroutine());
            isDoorClosed = true;
        }
    }

    public void OpenDoor()
    {
        if (isDoorClosed)
        {
            StartCoroutine(OpenDoorCoroutine());
            isDoorClosed = false;
        }
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
