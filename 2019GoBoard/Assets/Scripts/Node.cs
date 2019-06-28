using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour 
{
    private Vector2 coordinate;
    public Vector2 RoundedCoordinate{ get { return Utility.Vector2Round(coordinate); } }

    public List<Node> NeighborNodes { get; private set; }

    [SerializeField] private GameObject geometry;
    public GameObject Geometry { get { return geometry; }  }

    [SerializeField] private float scaleTime=0.3f;
    public float ScaleTime { get { return scaleTime; } }

    [SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public iTween.EaseType EaseType { get { return easeType; } }

    [SerializeField] private float delay = 1f;
    public float Delay { get { return delay; } }

    [SerializeField] private bool autoRun;
    public bool AutoRun { get { return autoRun; } }

    private Board board;
    private bool isInitialized;

    private void Awake()
    {
        board = UnityEngine.Object.FindObjectOfType<Board>();        
        coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    private void Start()
    {
        if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;
        }

        if (board != null)
        {            
            NeighborNodes = FindNeighbors(board.AllNodes);            
        }

        if (AutoRun)
        {
            InitNode();
        }


    }

    public void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time",scaleTime,
                "scale",Vector3.one,
                "easetype",easeType,
                "delay",delay
                ));
        }
    }

    public List<Node> FindNeighbors(List<Node> nodes)
    {
        List<Node> nList = new List<Node>();
        //Debug.Log(nodes);
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

    private void InitNeighbors()
    {
        StartCoroutine(InitNeighborsRoutine());
    }

    private IEnumerator InitNeighborsRoutine()
    {
        yield return new WaitForSeconds(delay);

        foreach(Node n in NeighborNodes)
        {
            n.InitNode();
        }
    }
}
