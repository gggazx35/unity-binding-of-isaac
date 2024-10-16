using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Player player;
    //public GameObject Boss;

    void Update()
    {
        if (player == null)
        {
            SceneManager.LoadScene("Died");
        }
        
    }
}
