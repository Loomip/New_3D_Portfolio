using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEvent : MonoBehaviour
{
    ParticleSystem particleSystem;
    [SerializeField] private GameObject explosionEffectPrefab; // ���� ����Ʈ ������
    List<ParticleCollisionEvent> collisionEvents;

    private GameObject owner = null;

    public void SetOwner(GameObject _owner) => owner = _owner;

    public void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        string otherLayerName = LayerMask.LayerToName(other.layer);

        ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, other, collisionEvents);

        if ((otherLayerName == "Ground"))
        {
            // ���� �浹���� �� ���� ����Ʈ ����
            var evt = collisionEvents[0];
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, evt.intersection, Quaternion.identity);
            //SoundManager.instance.PlaySfx(e_Sfx.ExplosionSound);
            Effect effect = explosionEffect.GetComponent<Effect>();
            BossAttackableState boss = owner.GetComponent<BossAttackableState>();
            effect.Atk = boss.atk;

            StartCoroutine(Destroy(explosionEffect, 1f));
        }
    }

    IEnumerator Destroy(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }
}
