using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EHeath : Health
{
    [SerializeField] private GameObject healthBarPrefab;

    public override void Hit(int damage)
    {
        if (hp > 0 && CanTakeDamage)
        {
            // 대미지 효과
            StartCoroutine(IsHitCoroutine(damage));
        }
    }

    void Start()
    {
        if (gameObject.tag == "Enemy")
        {
            GameObject healthBarObject = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            Slider healthBar = healthBarObject.GetComponentInChildren<Slider>();
            TextMeshProUGUI name = healthBarObject.GetComponentInChildren<TextMeshProUGUI>();
            name.text = State.CharacterName;
            UIManager.instance.RegisterEnemyHealthBar(this, healthBar);
        }
    }
}
