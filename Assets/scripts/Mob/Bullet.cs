using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Dmg = 3.5f;
    public string YourTag;
    public string TargetTag;
    public float speed;
    public LayerMask targetLayer;
    float Lifetime = 3.0f;

    void Start()
    {
        AudioManager.instance.PlaySfxByEnum(AudioManager.instance.GenerateRandomESfx(eSfx.TearFire, eSfx.TearFire1));
    }

    void Update()
    {
        if (Lifetime > 0.0f) { Lifetime -= Time.deltaTime; } else Blcoked();
        //transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // ¸·ÇûÀ»¶§
    void Blcoked()
    {
        AudioManager.instance.PlaySfxByEnum(eSfx.TearBlcok);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != YourTag) { }

        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
            Blcoked();
        if (other.tag == TargetTag)
        {
            
            Mob a = other.GetComponent<Mob>(); 
            if (a != null)
            {
                a.TakeDamage(Dmg);
            }
            Blcoked();
        }
    }
}
