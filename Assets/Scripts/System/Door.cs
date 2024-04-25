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
        closedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        openPosition = new Vector3(transform.position.x, 10f, transform.position.z);
    }

    public bool isOpening = false;
    public bool isClosing = false;

    public void CloseDoor()
    {
        if (!isDoorClosed)
        {
            isClosing = true;
            isDoorClosed = true;
        }
    }

    public void OpenDoor()
    {
        if (isDoorClosed)
        {
            isOpening = true;
            isDoorClosed = false;
        }
    }

    private void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, openSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, openPosition) <= 0.01f)
            {
                transform.position = openPosition;
                isOpening = false;
            }
        }
        else if (isClosing)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, openSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, closedPosition) <= 0.01f)
            {
                transform.position = closedPosition;
                isClosing = false;
            }
        }
    }
}
