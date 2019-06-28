using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour 
{
    private Vector2 coordinate;
    public Vector2 RoundedCoordinate{ get { return Utility.Vector2Round(coordinate); } }

    public List<Node> NeighborNodes { get; private set; }

    [SerializeField] private GameObject linkPrefab;

    [SerializeField] private GameObject geometry;
    public GameObject Geometry { get { return geometry; }  }

    [SerializeField] private float scaleTime;
    public float ScaleTime { get { return scaleTime; } }

    [SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public iTween.EaseType EaseType { get { return easeType; } }

    [SerializeField] private float delay;
    public float Delay { get { return delay; } }

    [SerializeField] private bool autoRun;
    public bool AutoRun { get { return autoRun; } }

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

            if (AutoRun)
            {
                InitNode();
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

    private void InitNode()
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
        Debug.Log("Init neigh");
        StartCoroutine(InitNeighborsRoutine());
    }

    private IEnumerator InitNeighborsRoutine()
    {
        yield return new WaitForSeconds(delay);

        foreach(Node n in NeighborNodes)
        {
            if (!linkedNodes.Contains(n))
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
}
