using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mullieboom : Mob
{
    public Pathfinding2D pathfinding2D;
    EnemyAnimator Ema;
    public GameObject Boom;
    float Roar = 2.0f;
    void Start()
    {
        pathfinding2D = GetComponent<Pathfinding2D>();
        InvokeRepeating("UpdatePath", 0.5f, 0.5f);

        Ema = transform.Find("Sprites").GetComponent<EnemyAnimator>();
    }

    void UpdatePath()
    {
        pathfinding2D.UpdatePath();
    }
    
    public override void TakeDamage(float _dmg)
    {
        Ema.Hurt = 1.0f;
        base.TakeDamage(_dmg);
    }

    private void FixedUpdate()
    {
        if (pathfinding2D.target == null) return;
        Roar -= Time.fixedDeltaTime;
        if (Roar <= 0.0f)
        {
            Roar = Random.Range(1.5f, 5.5f);
            AudioManager.instance.PlaySfxByEnum(AudioManager.instance.GenerateRandomESfx(eSfx.ZombieGrunt, eSfx.ZombieGrunt1));
        }
        // pathfinding2D ????? Trace? ??? ????? ??
        pathfinding2D.Trace(4000.0f);
    }

    


    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Health a = other.GetComponent<Health>();
            if (a != null && a.Ignore == false)
            {
                a.TakeDamage(1f);
                a.Ignore = true;
            }
        }
    }*/
}
