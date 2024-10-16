using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float MaxHealth = 0;
    public float CurrentHealth = 0;
    [SerializeField] bool HasGracePeriod = false;
    public bool Ignore = false;
    public float delay = 0.0f;
    Mob mob;
    void Start()
    {
        mob = GetComponent<Mob>();
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float _dmg)
    {
        
        
        CurrentHealth = Mathf.Clamp(CurrentHealth - _dmg, 0, MaxHealth);
        
        if(CurrentHealth <= 0.0f)
        {

            if(gameObject.tag == "Enemy")
            {
                RoomManager.instance.CurrentRoom.Remaings--;
            }

            if (mob == null)
            {
                Destroy(gameObject);
            } else
            {
                mob.Death();
            }
        }
    }

    

    public void DisableCollision()
    {
        Ignore = true;
        //gameObject.GetComponent<Rigidbody2D>().enabled = false;
    }

    public void HealHealth(float _healRate)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + _healRate, 0, MaxHealth);
    }
}
