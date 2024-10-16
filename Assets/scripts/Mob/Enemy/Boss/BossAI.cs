//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : Mob
{
    public GameObject BBullet;
    public Pathfinding2D pathfinding2D;
    public float Timer;
    public int JumpStack = 0;
    public bool Fo = false;
    //public BoxCollider2D collision;
    SpriteRenderer sp;
    Animator animator;
    int LilBullets;
    float Hurt = 0.0f;

    public Slider BossHealthUI;

    void Start()
    {
        Timer = -1.0f;
        pathfinding2D = GetComponent<Pathfinding2D>();
        InvokeRepeating("UpdatePath", 0.5f, 3f);
        JumpStack = 0;
        animator = transform.Find("Sprite").GetComponent<Animator>();
        sp = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        RoomManager.instance.BossRoom = gameObject;
        RoomManager.instance.BossHealth = GetComponent<Health>();
        AudioManager.instance.SetBgmToBossBgm();
    }

    void UpdatePath()
    {
        pathfinding2D.UpdatePath();
    }

    public void ResetJumpStack()
    {
        
    }

    void SpawnLilBullet()
    {
        Vector2 len = pathfinding2D.target.position - transform.position;
        float z = Mathf.Atan2(len.x, len.y) * Mathf.Rad2Deg;
        var bullet = Instantiate(BBullet, new Vector2(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y + Random.Range(-1.0f, 1.0f)), transform.rotation);
        bullet.transform.rotation = Quaternion.Euler(0, 0, -z);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 5.0f;
    }

    private void FixedUpdate()
    {
        if (pathfinding2D.target == null) return;
        
        if(JumpStack == 5)
        {
            // Attack 애니메이션 실행
            animator.SetTrigger("Attack");




            AudioManager.instance.PlaySfxByEnum(eSfx.MonstroBlob);
            for (int i = 0; i < 10; i++) { 
    

                SpawnLilBullet();
            }
            //bullet.GetComponent<Rigidbody2D>().velocity = transform.forward * 5.0f;
            JumpStack = 0;
            return;
        }

        if (Timer < 1.5f)
        {
            if(Fo == false)
            {
                
               // collision.enabled = true;
            }
            Timer += Time.fixedDeltaTime;
            //if (shit[0] == false && shit[1] == false)
            //{
            pathfinding2D.Trace(3000.0f);


            //Fo = true;

        } else {
            JumpStack++;

            animator.SetTrigger("Jump");
            Timer = 0.0f;
            if (Fo == true)
            {
                
                //transform.position = pathfinding2D.target.position;
                //collision.enabled = false;
                //Timer = 0.0f;
            }
        }

        if (Hurt > 0.0f)
        {
            Hurt -= Time.fixedDeltaTime;
            sp.material.color = new Color(1.0f - Hurt, Hurt > 0.0f ? 0.0f : 1.0f, Hurt > 0.0f ? 0.0f : 1.0f);
        }
    }
    public override void TakeDamage(float _dmg)
    {
        Hurt = 1.0f;
        base.TakeDamage(_dmg);
        if (!BossHealthUI) return;
        //BossHealthUI.value = health.CurrentHealth;
    }

    public override void Death()
    {
        SceneManager.LoadScene("End");
    }
}

