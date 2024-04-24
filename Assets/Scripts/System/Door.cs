using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    private Transform door; // �� ������Ʈ
    public float openHeight = 5f; // ���� ���� ���� ����
    public float openSpeed = 2f; // ���� ������ �ӵ�
    public float openDuration = 5f; // ���� ���� ���� �ð�
    public float activationDistance = 3f; // ���� �� �� �ִ� �ִ� �Ÿ�

    private Vector3 closedPosition; // ���� ���� ��ġ
    private Vector3 openPosition; // ���� ���� ��ġ

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
        // ���� ����
        while (Vector3.Distance(door.position, openPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, openPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = openPosition;

        // ���� �ð� ���� ���
        yield return new WaitForSeconds(openDuration);

        // ���� ����
        while (Vector3.Distance(door.position, closedPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, closedPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = closedPosition;
    }

}
