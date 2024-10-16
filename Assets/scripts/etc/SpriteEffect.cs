using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    SpriteRenderer Sr;
    public float Length = 1.0f;
    public List<Sprite> SpriteSheet = new List<Sprite>();
    public int Frame;
    public float RUs;
    public float tTime;
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("Freaking grod");
        RUs = Length / SpriteSheet.Count;
        tTime = 0.0f;
        Sr = GetComponent<SpriteRenderer>();
        Frame = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        if((Length / SpriteSheet.Count) <= tTime)
        {
            tTime = 0.0f;
            Sr.sprite = SpriteSheet[Frame];
            Frame++;
            if (Frame >= SpriteSheet.Count)
            {
                OnLastFrame();
            }
        }
        tTime += Time.deltaTime;
    }

    public virtual void OnLastFrame()
    {
        Destroy(gameObject);
    } 
}
