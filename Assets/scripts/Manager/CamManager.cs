using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static CamManager instance;

    public Room CurrentRoom;
    public float MoveSpeedWhenRoomChange;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if(CurrentRoom == null) return;

        Vector3 TargetLoc = GetCameraTargetLocation();

        transform.position = Vector3.MoveTowards(transform.position, TargetLoc, Time.deltaTime * MoveSpeedWhenRoomChange);
        if(TargetLoc.x > transform.position.x || TargetLoc.x < transform.position.x || TargetLoc.y > transform.position.y || TargetLoc.y < transform.position.y){
            //AstarPath.active.astarData.gridGraph = TargetLoc;
        }
    }

    Vector3 GetCameraTargetLocation()
    {
        if(CurrentRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 TargetLoc = CurrentRoom.GetRoomCentre();
        TargetLoc.z = transform.position.z;
        return TargetLoc;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetLocation()) == false;
    }
}
