using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;


    public SpriteRenderer Sr;
    public SpriteRenderer SrHead;
    public float Hurt = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetBool("walkUpNdown", false);
        anim.SetBool("walk", false);
        if (rb.velocity.y < -0.01f || rb.velocity.y > 0.01f)
        {

            anim.SetBool("walkUpNdown", true);
        }

        if (rb.velocity.x < -0.01f)
        {
            //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            anim.SetBool("walk", true);
        }

        if (rb.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            anim.SetBool("walk", true);
        }

        if(Hurt > 0.0f)
        {
            Hurt -= Time.fixedDeltaTime;
            Sr.material.color = new Color(1.0f - Hurt, Hurt > 0.0f ? 0.0f : 1.0f, Hurt > 0.0f ? 0.0f : 1.0f);
            SrHead.material.color = new Color(1.0f - Hurt, Hurt > 0.0f ? 0.0f : 1.0f, Hurt > 0.0f ? 0.0f : 1.0f);
        }
    }
}
