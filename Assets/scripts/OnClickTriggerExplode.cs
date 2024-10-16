using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickTriggerExplode : MonoBehaviour
{
    public GameObject Explosion;
    public Mob gobj;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            gobj.TakeDamage(9999.0f);
        }
    }
}
