using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class Pathfinding2D : MonoBehaviour
{

    public Transform seeker, target;
    Grid2D grid;
    Node2D seekerNode, targetNode;
    //public GameObject GridOwner;
    public Grid2D GridOwner;
    public int idx = 0;
    bool PathfindingCompeleted = false;
    public Node2D Current = null;
    public Vector3 ps;
    public Rigidbody2D rb;
    public bool ReachedToEnd = false;
    bool DoneCalc = true;

    public List<Node2D> Path;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = this.transform;
        grid = GridOwner.GetComponent<Grid2D>();

    }
    /*
        private void FixedUpdate()
        {
            if(target == null) return;

            Trace();
        }*/

    public void UpdatePath()
    {
        if (target.GetComponent<PlayerMoveChecker>().Moved == true)
        {
            DoneCalc = false;
            //print(rb.position.ToString());

            // findpath함수 호출 리지드바디 포지션와 타킷 포지션을 넣어
            FindPath(rb.position, target.position);
        }
    }

    // Trace 함수 플레이어를 추적할때사용
    public bool Trace(float _speed)
    {

        

        if (GridOwner ==  null) return false;

        if (Path.Count > 0)
        {


            // Vector2.Distance로 거리계산
            float Distance = Vector2.Distance(rb.position, Path[0].worldPosition);
            

            if (Distance >= 0.05f)
            {
                // vector2로 변화한 현재 path[idx]의 월드내 포지션을 구하고 노멀라이즈(옆에서 이동할때 더 빠르게 움직임을 막음) 
                Vector2 Direction = ((Vector2)Path[idx].worldPosition - rb.position).normalized;
                Vector2 Force = Direction * _speed * Time.fixedDeltaTime;

                // 리지드바디에 Force만큼 힘을줌
                rb.AddForce(Force);
                return false;

            } else
            {  
                DoneCalc = true;
                return true;
                //Current = GridOwner.path[0];
                Path.RemoveAt(0);
            }
        }
        return false;
       
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //get player and target position in grid coords
        seekerNode = grid.NodeFromWorldPoint(startPos);
        targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node2D> openSet = new List<Node2D>();
        HashSet<Node2D> closedSet = new HashSet<Node2D>();
        openSet.Add(seekerNode);

        //calculates path for pathfinding
        while (openSet.Count > 0)
        {

            //iterates through openSet and finds lowest FCost
            // 현재노드를 openSet 목적지로
            Node2D node = openSet[0];
            // openset[0](목적지)는 이미 정해져 있슴으로 i는 1이 초기값
            for (int i = 1; i < openSet.Count; i++)
            {
                // 
                if (openSet[i].FCost <= node.FCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            //타깃 찾았으면 retrace
            if (node == targetNode)
            {
                RetracePath(seekerNode, targetNode);
                return;
            }

            // 이웃노드 openset에 삽입
            foreach (Node2D neighbour in grid.GetNeighbors(node))
            {
                if (neighbour.obstacle || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    // 계산완료됀 path를 뒤집음
    void RetracePath(Node2D startNode, Node2D endNode)
    {
        List<Node2D> path = new List<Node2D>();
        Node2D currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        Path = path;

    }

    //gets distance between 2 nodes for calculating cost
    // 두 노드의 거리계산
    int GetDistance(Node2D nodeA, Node2D nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}