using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class Player : Mob
{
    public float Speed = 1.0f;
    Rigidbody2D RigidBody;
    Vector2 InputVelocity;
    public Vector2Int GridLocation;
    [SerializeField] float ShotSpeed = 2.0f;
    public float Delay = 0.0f;
    public float DmgDelay = 0.0f;
    bool Hurt = false;

    SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletSpawnPointTop;
    [SerializeField] private Transform BulletSpawnPointBottom;
    [SerializeField] private Transform BulletSpawnPointRight;
    [SerializeField] private Transform BulletSpawnPointLeft;
    Health health;
    float ExHealth = 0.0f;

    Animator animator;
    Animator animatorHead;

    SpriteRenderer Sr;
    SpriteRenderer SrHead;

    Transform transformHead;
    Transform transformBody;

    public void AddShotSpeed(float speed)
    {
        ShotSpeed += speed;
    }

    //[SerializeField] Vector2Int Infloat;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        ExHealth = health.CurrentHealth;
        InputVelocity = new Vector2(0.0f, 0.0f);
        Delay = 0.0f;
        RigidBody = GetComponent<Rigidbody2D>();
        animator = transform.Find("SpriteBody").GetComponent<Animator>();
        animatorHead = transform.Find("SpriteHead").GetComponent<Animator>();

        transformBody = transform.Find("SpriteBody").transform;
        transformHead = transform.Find("SpriteHead").transform;

        Sr = transform.Find("SpriteBody").GetComponent<SpriteRenderer>();
        SrHead = transform.Find("SpriteHead").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (DmgDelay <= 0.01f)
        {
            Debug.Log("Bitch");
            health.Ignore = true;
            if (health.CurrentHealth < ExHealth)
            {
                ExHealth = health.CurrentHealth;
                DmgDelay = 1.0f;
            }
        }
        else
        {
            health.Ignore = false;
            DmgDelay -= Time.deltaTime;
        }
*/


        InputVelocity.x = Input.GetAxisRaw("Horizontal");
        InputVelocity.y = Input.GetAxisRaw("Vertical");
        
        if (Delay >= (1.0f / ShotSpeed))
        {
            animatorHead.speed = 0.18f * ShotSpeed;
            //animatorHead.speed = 3.0f;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                print("Doomed");
                var bullet = Instantiate(Bullet, BulletSpawnPointTop.position, BulletSpawnPointTop.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpawnPointTop.up * 5.0f;
                animatorHead.Play("attackUp");

                Delay = 0.0f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                var bullet = Instantiate(Bullet, BulletSpawnPointTop.position, BulletSpawnPointTop.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpawnPointBottom.up * -5.0f;
                animatorHead.Play("attackDown");

                Delay = 0.0f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                var bullet = Instantiate(Bullet, BulletSpawnPointTop.position, BulletSpawnPointTop.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpawnPointRight.right * 5.0f;
                transformHead.localScale = new Vector3(5.0f, 5.0f, 5.0f);
                animatorHead.Play("AttackLR");

                Delay = 0.0f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                var bullet = Instantiate(Bullet, BulletSpawnPointTop.position, BulletSpawnPointTop.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpawnPointLeft.right * -5.0f;
                //animatorHead.Play("AttackLR");
                animatorHead.Play("AttackLR");

                transformHead.localScale = new Vector3(-5.0f, 5.0f, 5.0f);
                Delay = 0.0f;
            } else
            {
                animatorHead.Play("StandHead");
                //animatorHead.Play("attack");
            }
        }
        else
        {
            
            Delay += Time.deltaTime;
        }

        
        //RigidBody.velocity = new Vector3(Horizontal * Speed, Vertical * Speed, 0.0f);
    }

    void FixedUpdate()
    {
        Vector2 NextVec = InputVelocity.normalized * Speed * Time.fixedDeltaTime;

        animator.SetBool("walkUpNdown", false);
        animator.SetBool("walk", false);

        if (InputVelocity.y > 0.01f)
        {
            animator.SetBool("walkUpNdown", true);
        } else if (InputVelocity.y < -0.01f)
        {
            animator.SetBool("walkUpNdown", true);
        }

        if (InputVelocity.x > 0.01f)
        {
            transformBody.localScale = new Vector3(5.0f, 5.0f, 5.0f);
            animator.SetBool("walk", true);
        } else if (InputVelocity.x < -0.01f) {
            transformBody.localScale = new Vector3(-5.0f, 5.0f, 5.0f);
            animator.SetBool("walk", true);
        }
        
        RigidBody.MovePosition(RigidBody.position + NextVec);

        
        //GridLocation = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(1.0f);
        }
    }

    public override void TakeDamage(float _dmg)
    {
        if (!Hurt)
        {

            Hurt = true;
            gameObject.layer = 8;
            //spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            Sr.color = new Color(1, 1, 1, 0.4f);
            SrHead.color = new Color(1, 1, 1, 0.4f);

            
            //RigidBody.AddForce(new Vector2(dircX, dircY) * 9979, ForceMode2D.Impulse);
            health.TakeDamage(1.0f);
            AudioManager.instance.PlaySfxByEnum(AudioManager.instance.GenerateRandomESfx(eSfx.IsaacHurt, eSfx.IsaacHurt1, eSfx.IsaacHurt2));
            Invoke("OffDamage", 1.5f);
        }

    }

    void OffDamage()
    {
        
        gameObject.layer = 6;
        Sr.color = new Color(1, 1, 1, 1.0f);
        SrHead.color = new Color(1, 1, 1, 1.0f);
        Hurt = false;
    }
} 