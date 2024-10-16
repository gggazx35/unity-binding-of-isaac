using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveChecker : MonoBehaviour
{
    public bool Moved = false;
    public Vector3 v3;
    public Grid2D GridOwner;
    public int dx, dy = 0;
    public bool str = false;
    // Start is called before the first frame update
    void Start()
    {
        //dx = Mathf.RoundToInt(transform.position.x - 1 + (GridOwner.gridSizeX / 2));
        //dy = Mathf.RoundToInt(transform.position.y + (GridOwner.gridSizeY / 2));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GridOwner == null) return;
        int x = 0;
        int y = 0;
        
        x = Mathf.RoundToInt(transform.position.x - 1 + (GridOwner.gridSizeX / 2));
        y = Mathf.RoundToInt(transform.position.y - 1 + (GridOwner.gridSizeY / 2));

        if (str == true)
        {
            if (x != dx || y != dy)
            {
                SwitchMoved();
            }
        }

        dx = x; 
        dy = y;
        v3 = new Vector3((int)x, (int)y, 0);
        str = true;
    }   

    void SwitchMoved()
    {
        Moved = true;
    }
}
