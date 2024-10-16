using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Random;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


// 룸의 정보
public class RoomInfo
{
    public string Name;
    public int x;
    public int y;
}

// 룸매니저 클래스
public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject RoomPrefeb;
    public GameObject GoldenRoomPrefeb;
    [SerializeField] private int MaxRooms = 15;
    [SerializeField] private int MinRooms = 10;
    int RoomWidth = 17;
    int RoomHeight = 9;

    [SerializeField] int GridSizeX = 10;
    [SerializeField] int GridSizeY = 10;
    [SerializeField] List<GameObject> RoomInside;
    //public GameObject GoldenRoom;
    [SerializeField] List<Vector2Int> bools;
    public static RoomManager instance;
    [SerializeField] private GameObject GoldenRoom;
    [SerializeField] private List<GameObject> RoomObjects = new List<GameObject>();
    [SerializeField] public GameObject BossRoom;
    public Health BossHealth;


    private Queue<Vector2Int> RoomQueue = new Queue<Vector2Int>();

    private int[,] RoomGrid;

    private int RoomCount;
    [SerializeField] string CurrentRoomName = "Basement";

    public Room CurrentRoom;
    RoomInfo CurrentRoomData;
    private bool IsLoadingRoom = false;
    private bool GenerationCompelete = false;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        RoomGrid = new int[GridSizeX, GridSizeY];
        RoomQueue = new Queue<Vector2Int>();
        // ㅜ룸의 시작점
        Vector2Int InitialRoomIndex = new Vector2Int(GridSizeX / 2, GridSizeY / 2);
        StartRoomGenerationFromRoom(InitialRoomIndex);
    }
    private void Update()
    {
        UpdateRoom();

    }

    private void UpdateRoom()
    {
        // 맵이 모두 생성됄떄까지 반복
        if (RoomQueue.Count > 0 && RoomCount < MaxRooms && !GenerationCompelete)
        {
            Vector2Int RoomIndex = RoomQueue.Dequeue();
            int GridX = RoomIndex.x;
            int GridY = RoomIndex.y;
            // 앞뒤옆 모든 방향에서 맵 생성시도
            TryGenerateRoom(new Vector2Int(GridX - 1, GridY));
            TryGenerateRoom(new Vector2Int(GridX + 1, GridY));
            TryGenerateRoom(new Vector2Int(GridX, GridY + 1));
            TryGenerateRoom(new Vector2Int(GridX, GridY - 1));
        }
        else if (RoomCount < MinRooms)
        {
            // 룸이 최소 룸보다 적으면 다시 생성
            Debug.Log("RoomCount wasless than the minimum amount of rooms. Trying Again");

            RegenerateRooms();
        }
        else if (!GenerationCompelete)
        {
            for (int i = 0; i < RoomGrid.GetLength(0); i++)
                for (int j = 0; j < RoomGrid.GetLength(1); j++)
                    if (RoomGrid[i, j] == 1) SelectDeadEndRooms(new Vector2Int(i, j));

            if(bools.Count < 2 || BossRoom == null)
            {
                RegenerateRooms();
            }

            Debug.Log($"Generation Compelete, {RoomCount} Rooms Created");
            GenerationCompelete = true;

            int idx = Random.Range(0, bools.Count - 1);
            Vector2Int v2 = bools[idx];
            //GoldenRoom = RoomObjects.Find(x => x.GetComponent<Room>().RoomIndex.x == v2.x && x.GetComponent<Room>().RoomIndex.y == v2.y);
            //GoldenRoom.tag = "GoldenRoom";
            //bools.RemoveAt(idx);

            /*idx = Random.Range(0, bools.Count - 1);
            v2= bools[idx];
            GoldenRoom = RoomObjects.Find(x => x.GetComponent<Room>().RoomIndex.x == v2.x && x.GetComponent<Room>().RoomIndex.y == v2.y);
            GoldenRoom.tag = "BossRoom";
            bools.RemoveAt(idx);*/
            
        }

    }

        private void SelectDeadEndRooms(Vector2Int _roomIndex)
        {
            List<Vector2Int> flox = new List<Vector2Int>();
            if ((_roomIndex.x + 1) < GridSizeX) flox.Add(new Vector2Int(_roomIndex.x + 1, _roomIndex.y));
            if ((_roomIndex.x - 1) >= 0) flox.Add(new Vector2Int(_roomIndex.x - 1, _roomIndex.y));

            if ((_roomIndex.y + 1) < GridSizeY) flox.Add(new Vector2Int(_roomIndex.x, _roomIndex.y + 1));
            if ((_roomIndex.y - 1) >= 0) flox.Add(new Vector2Int(_roomIndex.x, _roomIndex.y - 1));


            int EmptyRooms = 0;
            for (int i = 0; i < flox.Count; i++)
            {
                if (RoomGrid[flox[i].x, flox[i].y] == 0)
                {
                    EmptyRooms++;
                }
            }

            if (EmptyRooms == 3)
            {
                if (BossRoom.GetComponent<Room>().RoomIndex != _roomIndex)
                {
                    bools.Add(_roomIndex);
                }
            }
        }

        private void StartRoomGenerationFromRoom(Vector2Int _roomIndex)
        {

            RoomQueue.Enqueue(_roomIndex);
            int x = _roomIndex.x;
            int y = _roomIndex.y;
            // RoomGrid를 마크표시
            RoomGrid[x, y] = 1;
            RoomCount++;
            // 룸인덱스로 포지션 구하기
            var InitialRoom = Instantiate(RoomPrefeb, GetPositionFromGridIndex(_roomIndex), Quaternion.identity);
            InitialRoom.name = $"Room-{RoomCount}";
            InitialRoom.GetComponent<Room>().RoomIndex = _roomIndex;

            var th = Instantiate(RoomInside[0], GetPositionFromGridIndex(_roomIndex), Quaternion.identity);
            th.transform.SetParent(InitialRoom.transform);
            th.SetActive(false);

            // InitalRoom의 Room컴포넌트의 Enimie 즉 적리스트를 th의 자식 "Enimies"로 정함
            InitialRoom.GetComponent<Room>().Enemie = th.transform.Find("Enemies").gameObject;
            InitialRoom.GetComponent<Room>().grid2D = th.transform.Find("Grid").GetComponent<Grid2D>();

            CamManager.instance.CurrentRoom = InitialRoom.GetComponent<Room>();



            InitialRoom.GetComponent<Room>().NewDiscovered = true;
            InitialRoom.GetComponent<Room>().door = true;
            RoomObjects.Add(InitialRoom);
        }

        private bool TryGenerateRoom(Vector2Int _roomIndex)
        {

            int x = _roomIndex.x;
            int y = _roomIndex.y;

            if (RoomCount >= MaxRooms || RoomObjects.Find(cx => cx.GetComponent<Room>().RoomIndex.x == x && cx.GetComponent<Room>().RoomIndex.y == y)) { return false; }
            if (Random.value < 0.5f && _roomIndex != Vector2Int.zero) { return false; }
            if (CountAdjacentRooms(_roomIndex) > 1) { return false; }


            RoomQueue.Enqueue(_roomIndex);
            RoomGrid[x, y] = 1;
            RoomCount++;



            var NewRoom = Instantiate(RoomPrefeb, GetPositionFromGridIndex(_roomIndex), Quaternion.identity);
            NewRoom.GetComponent<Room>().RoomIndex = _roomIndex;
            GameObject th;


            // 만약 마지막 Room이면 BossRoom으로 생성
            if (RoomCount == (MaxRooms - 1))
            {
                NewRoom.name = $"Room-Boss";
                th = Instantiate(RoomInside[RoomInside.Count - 1], GetPositionFromGridIndex(_roomIndex), Quaternion.identity);

                // BossRoom변수를 NewRoom의 Room스크립트로 정함
                BossRoom = NewRoom;
            }
            else
            {
                NewRoom.name = $"Room-{RoomCount}";
                th = Instantiate(RoomInside[Random.Range(1, RoomInside.Count - 2)], GetPositionFromGridIndex(_roomIndex), Quaternion.identity);

            }
            // Room의 내용물(th)를 Room의 배경(NewRoom)의 자식으로 삼음
            th.transform.SetParent(NewRoom.transform);

            // Room의 내용물
            th.SetActive(false);
            NewRoom.GetComponent<Room>().Enemie = th.transform.Find("Enemies").gameObject;
            NewRoom.GetComponent<Room>().grid2D = th.transform.Find("Grid").GetComponent<Grid2D>();




            RoomObjects.Add(NewRoom);


            OpenDoors(NewRoom, x, y);


            return true;
        }

        private void RegenerateRooms()
        {
            // RoomObject와 RoomQueue의 내용을 모두삭제 및 변수 초기화
            RoomObjects.ForEach(Destroy);
            RoomObjects.Clear();
            RoomGrid = new int[GridSizeX, GridSizeY];
            RoomQueue.Clear();
            // GenerationCompelete와 RoomCount를 초기화 함으로서 RoomUpdate에서 다시 Room를 생성할수있게함
            RoomCount = 0;
            GenerationCompelete = false;


            // InitalRoomIndex를 Grid의 중앙으로
            Vector2Int InitialRoomIndex = new Vector2Int(GridSizeX / 2, GridSizeY / 2);
            // 첫 Room 생성
            StartRoomGenerationFromRoom(InitialRoomIndex);
        }

        void OpenDoors(GameObject _room, int x, int y)
        {
            Room NewRoomScript = _room.GetComponent<Room>();

            Room LeftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
            Room RightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
            Room TopRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
            Room BottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));


            // 앞뒤옆 모든 방향의 문을 검사하고 1로 마크돼있으면 그 Room의 문을 염
            if (x > 0 && RoomGrid[x - 1, y] != 0)
            {
                NewRoomScript.OpenDoor(Vector2Int.left);
                LeftRoomScript.OpenDoor(Vector2Int.right);
            }
            if (x < GridSizeX - 1 && RoomGrid[x + 1, y] != 0)
            {
                NewRoomScript.OpenDoor(Vector2Int.right);
                RightRoomScript.OpenDoor(Vector2Int.left);
            }
            if (y > 0 && RoomGrid[x, y - 1] != 0)
            {
                NewRoomScript.OpenDoor(Vector2Int.down);
                BottomRoomScript.OpenDoor(Vector2Int.up);
            }
            if (y < GridSizeY - 1 && RoomGrid[x, y + 1] != 0)
            {
                NewRoomScript.OpenDoor(Vector2Int.up);
                TopRoomScript.OpenDoor(Vector2Int.down);

            }
        }

        Room GetRoomScriptAt(Vector2Int _index)
        {
            GameObject RoomObject = RoomObjects.Find(r => r.GetComponent<Room>().RoomIndex == _index);
            if (RoomObject != null)
            {
                return RoomObject.GetComponent<Room>();
            }
            return null;
        }
        private int CountAdjacentRooms(Vector2Int _roomIndex)
        {
            int x = _roomIndex.x;
            int y = _roomIndex.y;
            int Count = 0;

            if (x > 0 && RoomGrid[x - 1, y] != 0) Count++;
            if (x < GridSizeX - 1 && RoomGrid[x + 1, y] != 0) Count++;
            if (y > 0 && RoomGrid[x, y - 1] != 0) Count++;
            if (y < GridSizeY - 1 && RoomGrid[x, y + 1] != 0) Count++;

            return Count;
        }


        private Vector3 GetPositionFromGridIndex(Vector2Int _gridIndex)
        {
            int gridX = _gridIndex.x;
            int gridY = _gridIndex.y;
            return new Vector3(RoomWidth * (gridX - GridSizeX / 2),
                RoomHeight * (gridY - GridSizeY / 2));
        }

        /*public void OnDrawGi
        {
            Color gizmoColor = new Color(0, 1, 1, 0.05f);
            Gizmos.color = gizmoColor;

            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                    Gizmos.DrawWireCube(position, new Vector3(RoomWidth, RoomHeight, 1));
                }
            }
        }*/

        public void OnPlayerEnterRoom(Room _room)
        {
            if (CurrentRoom != null) CurrentRoom.NewDiscovered = false;
            // 플레이어가 특정 Room에 들어오면 CamManager의 CurrentRoom을 인수로 받은 _room변수로 정함
            CamManager.instance.CurrentRoom = _room;
            // 또한 자신(RoomManager)의 CurrentRoom또한 _room인수로 정함
            CurrentRoom = _room;
            _room.NewDiscovered = true;
            //if(BossRoom.GetComponent<Room>() == CurrentRoom) BossHealth = GameObject.FindWithTag("Boss").GetComponent<Health>();
            // 현재 Room의 몬스터를 생성
            CurrentRoom.SpawnMonsters();
            // 햔제 Room의 문을 염
            OpenDoors(CurrentRoom.gameObject, CurrentRoom.RoomIndex.x, CurrentRoom.RoomIndex.y);
        }

        /*IEnumerator LoadRoomRoutain(Vector2Int _roomLoc)
        {
            string RoomName = CurrentRoomName;

            AsyncOperation LoadRoom = SceneManager.LoadSceneAsync(RoomName, LoadSceneMode.Additive);

            while (LoadRoom.isDone) {
                yield return null;
            }
        }*/
    }
