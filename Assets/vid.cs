using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class vid : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Play();
    }
}
