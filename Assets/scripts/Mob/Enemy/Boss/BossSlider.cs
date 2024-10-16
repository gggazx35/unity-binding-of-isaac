using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlider : MonoBehaviour
{
    public Slider BossHealthUI;
   
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (RoomManager.instance.BossHealth != null)
        {
            Health a = RoomManager.instance.BossHealth;
            BossHealthUI.maxValue = a.MaxHealth;
            BossHealthUI.value = a.CurrentHealth;
            
        } else
        {

        }
    }
}
