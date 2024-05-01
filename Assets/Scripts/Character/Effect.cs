using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // ��ų ���ݷ��� ���޹��� ���
    private int atk;
    public int Atk { get => atk; set => atk = value; }

    // ��ų ���ӽð�
    [SerializeField] private float duration;

    private void Start()
    {
        // �����ð��� �ö���
        StartCoroutine(DestroyAfterTime(duration));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<MonsterFSMController>().Hit();
            other.GetComponent<Health>().Hit(Atk);
        }

        if (other.tag == "Boss")
        {
            other.GetComponent<BossFSMController>().Hit();
            other.GetComponent<Health>().Hit(Atk);
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
