using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public GameObject Collision;
    

    public void SwitchDoorCollision()
    {
        Collision.SetActive(false);
    }

    public void UnActive()
    {
        if (gameObject.active)
        {
            Collision.SetActive(false);
        }
        else
        {
            Collision.SetActive(true);
        }
    }
}
