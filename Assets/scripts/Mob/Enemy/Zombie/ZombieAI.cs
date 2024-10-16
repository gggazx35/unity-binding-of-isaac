using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : Mob
{
    public eSfx[] Grunt;
    public Pathfinding2D pathfinding2D;
    EnemyAnimator Ema;
    float Roar = 2.0f;
    void Start()
    {
        pathfinding2D = GetComponent<Pathfinding2D>();
        // InvokeRepeating로 path를 0.5초 마다 업데이트
        InvokeRepeating("UpdatePath", 0.5f, 0.5f);

        // EnemyAnimator 객체 찾기
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
            AudioManager.instance.PlaySfxByEnum(AudioManager.instance.GenerateRandomESfxL(Grunt));
        }

        pathfinding2D.Trace(2000.0f * speed);
    }
}