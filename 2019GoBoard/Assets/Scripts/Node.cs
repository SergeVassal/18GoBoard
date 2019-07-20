using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour 
{
    private Vector2 coordinate;
    public Vector2 RoundedCoordinate{ get { return Utility.Vector2Round(coordinate); } }

    public List<Node> NeighborNodes { get; private set; }

    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private bool isLevelGoal=false;
    public bool IsLevelGoal { get { return isLevelGoal;} }

    [SerializeField] private GameObject geometry;
    public GameObject Geometry { get { return geometry; }  }

    [SerializeField] private float scaleTime;
    public float ScaleTime { get { return scaleTime; } }

    [SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public iTween.EaseType EaseType { get { return easeType; } }

    [SerializeField] private float delay;
    public float Delay { get { return delay; } }

    private Board board;
    private bool isInitialized=false;

    private List<Node> linkedNodes=new List<Node>();
    public List<Node> LinkedNodes { get { return linkedNodes; } }


    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();        
        coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    private void Start()
    {
        if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;

            if (board != null)
            {
                NeighborNodes = FindNeighbors(board.AllNodes);
            }            
        }        
    }    

    public List<Node> FindNeighbors(List<Node> nodes)
    {
        List<Node> nList = new List<Node>();
        
        foreach(Vector2 dir in Board.directions)
        {
            Node foundNeghbor = nodes.Find(n => n.RoundedCoordinate == RoundedCoordinate + dir);
            
            if (foundNeghbor != null && !nList.Contains(foundNeghbor))
            {                
                nList.Add(foundNeghbor);
            }
        }

        return nList;
    }

    public void InitNode()
    {
        if (!isInitialized)
        {
            ShowGeometry();
            InitNeighbors();
            isInitialized = true;
        }
    }

    public void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time", scaleTime,
                "scale", Vector3.one,
                "easetype", easeType,
                "delay", delay
                ));
        }
    }    

    private void InitNeighbors()
    {
        StartCoroutine(InitNeighborsRoutine());
    }

    private IEnumerator InitNeighborsRoutine()
    {
        yield return new WaitForSeconds(delay);

        foreach(Node n in NeighborNodes)
        {
            if (linkedNodes.Contains(n))
            {
                continue;                              
            }
            Obstacle obstacle = FindObstacle(n);
            if (obstacle == null)
            {
                LinkNode(n);
                n.InitNode();
            }
        }
    }

    private void LinkNode(Node targetNode)
    {
        if (linkPrefab != null)
        {
            GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
            linkInstance.transform.parent = transform;

            Link link = linkInstance.GetComponent<Link>();
            if (link != null)
            {
                link.DrawLine(transform.position, targetNode.transform.position);
            }

            if (!linkedNodes.Contains(targetNode))
            {
                linkedNodes.Add(targetNode);
            }

            if (!targetNode.LinkedNodes.Contains(this))
            {
                targetNode.LinkedNodes.Add(this);
            }
        }
    }

    private Obstacle FindObstacle(Node targetNode)
    {
        Vector3 checkDirection = targetNode.transform.position - transform.position;
        RaycastHit raycastHit;

        if(Physics.Raycast(transform.position,checkDirection,out raycastHit,
            Board.spacing + 0.1f, obstacleLayer))
        {
            return raycastHit.collider.GetComponent<Obstacle>();
        }
        return null;
    }
}
