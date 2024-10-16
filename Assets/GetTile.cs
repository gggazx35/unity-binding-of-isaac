using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;



//using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
class ANode
{
    public ANode(int x, int y, ANode prev)
    {
        this.x = x; 
        this.y = y;
        //this.Parent = prev;
    }
    //Vector2Int Parent;
    int x, y;
}

public class GetTile : MonoBehaviour
{
    public List<Vector3Int> get = new List<Vector3Int>();
    public int[,] TileTable = new int[17, 9];
    public int[,] Dis = new int[17, 9];
    public Tilemap _Gr;
    public Vector2Int Point = new Vector2Int(0, 0);
    public Queue<Vector2Int> vs = new Queue<Vector2Int>();
    public List<Vector2Int> Paths = new List<Vector2Int>();
    public List<int> patyh = new List<int>();

    public int Trys = 0;
    public bool b = false; 
    public GameObject TestFUcker;
    public GameObject T;
    public int Succeed = 0; 

    int Indexer = 0;
    //private Vector2Int[] = [ new Vector2Int(-1,0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)]
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("STARTED");
        for(int i = 0; i < 17; i++)
        {
            TileTable[i, 0] = 1;
            TileTable[i, 8] = 1;
            Dis[i, 0] = int.MaxValue;
            Dis[i, 8] = int.MaxValue;
        }

        for (int i = 0; i < 9; i++)
        {
            TileTable[0, i] = 1;
            TileTable[16, i] = 1;
            Dis[0, i] = int.MaxValue;
            Dis[16, i] = int.MaxValue;
        }
        se();
        if (TileTable[Point.x, Point.y] == 0) { 
            vs.Enqueue(Point);
            Debug.Log("FREA");
        } else {
            Debug.Log("FC");        
        } 

        vs.Enqueue(Point);
        TileTable[Point.x, Point.y] = 9;
        Debug.Log(Point.ToString());
    }

    void se()
    {
        int i = 0;
        foreach(Vector3Int pos in _Gr.cellBounds.allPositionsWithin)
        {
            
            if(_Gr.HasTile(pos))
            {
                TileTable[pos.x+8, pos.y+4] = 1;
            }
        }

        for(int x = 1; x < 16; x++)
        {
            for (int y = 1; y < 8; y++)
            {
                Debug.Log(x + ", " + y + " Hey: " + TileTable[x, y]);
            }
        }
    }

    /*private void bfs()
    {
        int[] dirX = new int[] { -1, 0, 1, 0 };
        int[] dirY = new int[] { 0, -1, 0, 1 };

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(Point);

        TileTable[Point.x, Point.y] = 1;
        for (int i = 0; i < 4; i++) {
            Vector2Int v = (dirX[i], dirY[i]);

        }
    }*/

    void TryAllDirection(Vector2Int _dir)
    {
        int X = Point.x + _dir.x;
        int Y = Point.y + _dir.y;
        if (!IsOutOfTilemap(X, Y))
        {

            if (TileTable[X, Y] == 0)
            {
                TileTable[X, Y] = 9;
                vs.Enqueue(new Vector2Int(X, Y));
                Dis[X, Y] = Dis[Point.x, Point.y] + 1;
                Succeed++;
                Debug.Log("HAHA " + TileTable[X, Y] + ", " + vs.Count);
            }
        }
        
       Indexer++;
        
    }

    void Pathfinding()
    {
        int PlayerX = 4;
        int PlayerY = 3;

        int MyX = 10;
        int MyY = 4;

        int PlayerDis = Dis[PlayerX, PlayerY];
        int MyDis = Dis[MyX, MyY];
        List<int> Dirs = new List<int>();
        Dirs.Add(0);
        Dirs.Add(0);
        Dirs.Add(0);
        Dirs.Add(0);
        bool un = false;
        int rs = 0;
        //patyh.Add(MyDis);
        //patyh.Add(PlayerDis);
        patyh.Add(PlayerDis);
        Vector2Int[] Directs = new Vector2Int[4];
        while (MyDis != PlayerDis)
        {
            Directs[0] = new Vector2Int(MyX + 1, MyY);
            Directs[1] = new Vector2Int(MyX, MyY - 1);
            Directs[2] = new Vector2Int(MyX - 1, MyY);
            Directs[3] = new Vector2Int(MyX, MyY + 1);
            if (!IsOutOfTilemap(MyX, MyY))
            {
                Dirs[0] = Dis[Directs[0].x, Directs[0].y];
                Dirs[1] = Dis[Directs[1].x, Directs[1].y];
                Dirs[2] = Dis[Directs[2].x, Directs[2].y];
                Dirs[3] = Dis[Directs[3].x, Directs[3].y];
                rs = Dirs[0];
                un = false;
                for(int i = 0; i < Dirs.Count; i++)
                {
                    if(rs > Dirs[i] && TileTable[Directs[i].x, Directs[i].y] != 1)
                    {
                        rs = i;
                        Debug.Log(Directs[i].ToString() + "; " + rs);
                        un = true;
                        Succeed++;
                    }
                }
                //if (!un) return;
                if (un == true)
                {
                    MyX = Directs[rs].x;
                    MyY = Directs[rs].y;
                    Debug.Log(new Vector2Int(MyX, MyY).ToString() + "; " + Dis[MyX, MyY] + "fas, d" + TileTable[MyX, MyY]);
                    MyDis = Dis[MyX, MyY];
                    patyh.Add(MyDis);
                }
                if (un == false) return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        int i = 0;
        if (Trys != 1)
        {
            Succeed = 0;

            if(vs.Count() != 0)
            {
                Vector2Int node = vs.Dequeue();
                Indexer = 2;
                Point = node;
                TryAllDirection(new Vector2Int(1, 0));
                TryAllDirection(new Vector2Int(0, -1));
                TryAllDirection(new Vector2Int(0, 1));
                TryAllDirection(new Vector2Int(-1, 0));

            } else
            {
                Trys = 1;
            }
            //vs.Clear();
        }
        else
        {
            if (b == true) return;
            for (int x = 1; x < 16; x++)
            {

                for (int y = 1; y < 8; y++)
                {
                    GameObject damn;
                    if (TileTable[x, y] == 9)
                    {
                        damn = Instantiate(TestFUcker);
                        damn.transform.position = new Vector3(x, y, 0);
                        damn.transform.localScale = new Vector3(1.0f / Dis[x, y], 1.0f / Dis[x, y], 1.0f / Dis[x, y]);
                        Debug.Log(x + ", " + y + " Hey: " + TileTable[x, y]);

                    }
                    else if(TileTable[x, y] == 1)
                    {
                        damn = Instantiate(T);
                        damn.transform.position = new Vector3(x, y, 0);
                        Debug.Log(x + ", " + y + " Hey: " + TileTable[x, y]);
                    }
                    
                }
            }
            Pathfinding();
            b = true;
        }
    }

    bool IsOutOfTilemap(int _x, int _y)
    {
        if (_x >= 1 && _x <= 15 && _y >= 1 && _y <= 7) return false;
        else return true;
    }
}

