using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitforlittle : MonoBehaviour
{
    public float t = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.active = false;

        Invoke("thing", t);

    }

    // Update is called once per frame
    void thing()
    {

        gameObject.active = true;
    }
}
