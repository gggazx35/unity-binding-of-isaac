using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;


struct Take
{
    public GameObject gameObject;
    public Vector2 where;
}

public class Room : MonoBehaviour
{
    [SerializeField] GameObject TopDoor;
    [SerializeField] GameObject BottomDoor;
    [SerializeField] GameObject LeftDoor;
    [SerializeField] GameObject RightDoor;
    [SerializeField] GameObject DoorCollision;
    [SerializeField] public Grid2D grid2D;
    List<GameObject> Enemies;
    [SerializeField] public GameObject Enemie;
    public Vector2Int Size;
    public Vector2Int RoomIndex { get; set; }
    [SerializeField] public int Remaings = 0;
    public bool NewDiscovered = false;
    public bool door;
    public Animator at;

    private void Awake()
    {
        Size.x = 20;
        Size.y = 12;
        door = false;
        //transform.
        //Enemies = GetChildren(Enemie);
    }


    private void SetChildrenPlayer(Player _gameObject)
    {
        //List<NaviAI> Children = new List<NaviAI>();

        foreach (Transform Child in Enemie.transform)
        {
            _gameObject.gameObject.GetComponent<PlayerMoveChecker>().GridOwner = grid2D;
            Child.gameObject.GetComponent<Pathfinding2D>().target = _gameObject.gameObject.transform;
            Child.gameObject.GetComponent<Pathfinding2D>().GridOwner = grid2D;
        } 
    }

    public void OpenDoor(Vector2Int _drection)
    {
        //DoorCollision.SetActive(false);
        if (_drection == Vector2Int.up)
        {
            TopDoor.SetActive(true);

            //if(TopDoor.active) TopDoor.SetActive(false);
        }
        if (_drection == Vector2Int.down)
        {
            BottomDoor.SetActive(true);
            //if (BottomDoor.active) BottomDoor.SetActive(false);
        }
        if (_drection == Vector2Int.left)
        {
            LeftDoor.SetActive(true);
            //if (LeftDoor.active) LeftDoor.SetActive(false);
        }
        if (_drection == Vector2Int.right)
        {
            RightDoor.SetActive(true);
            //if (RightDoor.active) RightDoor.SetActive(false);
        }

        /*if(RoomManager.Instance.BossRoom == gameObject)
        {
            if (_drection == Vector2Int.up)
            {
                TopDoor.GetComponent<SpriteRenderer>().sprite jopsdfho;

                //if(TopDoor.active) TopDoor.SetActive(false);
            }
            if (_drection == Vector2Int.down)
            {
                BottomDoor.SetActive(true);
                //if (BottomDoor.active) BottomDoor.SetActive(false);
            }
            if (_drection == Vector2Int.left)
            {
                LeftDoor.SetActive(true);
                //if (LeftDoor.active) LeftDoor.SetActive(false);
            }
            if (_drection == Vector2Int.right)
            {
                
                //if (RightDoor.active) RightDoor.SetActive(false);
            }
        }*/
    }


    public void SpawnMonsters()
    {
        if (gameObject.tag == "GoldenRoom")
        {
            Destroy(Enemie.transform.parent.gameObject);
            GameObject th = Instantiate(RoomManager.instance.GoldenRoomPrefeb, transform.position, Quaternion.identity);
            th.transform.SetParent(this.transform);

            Enemie = th.transform.Find("Enemies").gameObject;
            grid2D = th.transform.Find("Grid").GetComponent<Grid2D>();
        }
        
        Enemie.transform.parent.gameObject.SetActive(true);
        Remaings = Enemie.transform.childCount;
        
            /* for(int i = 0; i < Enemies.Count; i++) 
        {
            Enemies[i].SetActive(true);
        }*/
    }

    public Vector3 GetRoomCentre()
    {
        return transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            
            DisableDoorCollision();
            RoomManager.instance.OnPlayerEnterRoom(this);
            SetChildrenPlayer(other.gameObject.GetComponent<Player>());
            SpawnMonsters();
            //NewDiscovered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    void FixedUpdate()//void OnTriggerStay2D(Collider2D other)
    {
        
        if(Remaings <= 0)
        {
            if (door == true && NewDiscovered)
            {
                door = false;
                at.SetBool("open", true);
                //TopDoor.GetComponent<Door>().Open();
                AudioManager.instance.PlaySfxByEnum(eSfx.DoorOpen);
            }
            DisableDoorCollision();
        } else
        {
            EnableDoorCollision();
            if(door == false && NewDiscovered)
            {
                AudioManager.instance.PlaySfxByEnum(eSfx.DoorClose);
                at.SetBool("open", false);
                //TopDoor.GetComponent<Door>().Close();
                door = true;
            }
        }
    }

    public void DisableDoorCollision()
    {
        TopDoor.GetComponent<Door>().SwitchDoorCollision();
        LeftDoor.GetComponent<Door>().SwitchDoorCollision();
        RightDoor.GetComponent<Door>().SwitchDoorCollision();
        BottomDoor.GetComponent<Door>().SwitchDoorCollision();
    }

    public void EnableDoorCollision()
    {
        TopDoor.GetComponent<Door>().Collision.SetActive(true);
        LeftDoor.GetComponent<Door>().Collision.SetActive(true);
        RightDoor.GetComponent<Door>().Collision.SetActive(true);
        BottomDoor.GetComponent<Door>().Collision.SetActive(true);
    }

    
}
