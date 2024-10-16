using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health healthScript;
    public Image[] healths;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    [SerializeField] private Image BossHealthUI;

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < healthScript.MaxHealth; i += 2)
        {
            if(i < (healthScript.CurrentHealth-1))
            {
                healths[i / 2].sprite = fullHeart;
            } else if(i == (healthScript.CurrentHealth-1))
            {
                healths[i / 2].sprite = halfHeart;
            } else
            {
                healths[i / 2].sprite = emptyHeart;
            }
        }
        if (!RoomManager.instance) return;
        if(RoomManager.instance.BossHealth != null)
        {
            BossHealthUI.gameObject.active = true;

        } else
        {
            BossHealthUI.gameObject.active = false;
        }
    }
}
