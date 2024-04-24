using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    public float openHeight = 5f; // ���� ���� ���� ����
    public float openSpeed = 2f; // ���� ������ �ӵ�
    public float openDuration = 5f; // ���� ���� ���� �ð�
    public float activationDistance = 3f; // ���� �� �� �ִ� �ִ� �Ÿ�

    private Vector3 closedPosition; // ���� ���� ��ġ
    private Vector3 openPosition; // ���� ���� ��ġ

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
