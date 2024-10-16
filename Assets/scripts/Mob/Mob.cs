using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    //public Health health;
    public float speed = 1.0f;
    public Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
    }

    // 대지미 받는 메소드 기본적으로 health스크립트의 CurrentHealth를 떨어트리는 health.TakeDamage 메소드를 호출하지만
    // Mob의 자식 클래스 Player, Zombie가 함수를 오버라이드 할수있게 virtual 함수로 선언함
    public virtual void TakeDamage(float _dmg)
    {
        health.TakeDamage(_dmg);
        
        
    }

    public virtual void Death()
    {
        AudioManager.instance.PlaySfxByEnum(eSfx.HeartOut);
        Destroy(gameObject);
    }
}
