using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : SpriteEffect
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        AudioManager.instance.PlaySfxByEnum(AudioManager.instance.GenerateRandomESfx(eSfx.Explode, eSfx.Explode1, eSfx.Explode2));
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            Mob a = other.GetComponent<Mob>();
            if (a != null)
            {
                a.TakeDamage(2.0f);
            }
        }
    }
}
