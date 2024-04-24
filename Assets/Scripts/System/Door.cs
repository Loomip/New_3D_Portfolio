using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    // ���� ������ �ӵ�
    public float openSpeed = 2f; 
    // ���� �� �� �ִ� �ִ� �Ÿ�
    public float activationDistance = 3f; 

    // ���� ���� ��ġ
    private Vector3 closedPosition; 
    // ���� ���� ��ġ
    private Vector3 openPosition; 

    // ���� ���� �ִ���
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
        // ���� ����
        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = openPosition;
    }

    private IEnumerator CloseDoorCoroutine()
    {
        // ���� ����
        while (Vector3.Distance(transform.position, closedPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = closedPosition;
    }

}
