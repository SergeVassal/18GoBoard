using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour 
{
    public static float spacing = 2f;

    public static readonly Vector2[] directions =
    {
        new Vector2(spacing,0f),
        new Vector2(-spacing,0f),
        new Vector2(0f,spacing),
        new Vector2(0f,-spacing)
    };

    private List<Node> allNodes = new List<Node>();
    public List<Node> AllNodes { get { return allNodes; } }

    private Node goalNode;
    public Node GoalNode { get { return goalNode;} }

    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private float drawGoalTime = 2f;
    [SerializeField] private float drawGoalDelay =2f;
    [SerializeField] private iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;



    private Node playerNode;
    public Node PlayerNode { get { return playerNode; } }

    private PlayerMover player;


    private void Awake()
    {
        player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();
        goalNode = FindGoalNode();
    }

    public void GetNodeList()
    {
        Node[] nArray = GameObject.FindObjectsOfType<Node>();
        allNodes = new List<Node>(nArray);   
    }

    public Node FindNodeAt(Vector3 position)
    {
        Vector2 boardCoord = Utility.Vector2Round(new Vector2(position.x, position.z));
        return allNodes.Find(n => n.RoundedCoordinate == boardCoord);
    }

    private Node FindGoalNode()
    {
        return allNodes.Find(n => n.IsLevelGoal);
    }

    public Node FindPlayerNode()
    {
        if(player!=null && !player.IsMoving)
        {
            return FindNodeAt(player.transform.position);
        }
        return null;
    }

    public void UpdatePlayerNode()
    {
        playerNode = FindPlayerNode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        if (playerNode != null)
        {
            Gizmos.DrawSphere(playerNode.transform.position, 0.2f);
        }
    }

    public void DrawGoal()
    {
        if (goalPrefab != null && goalNode!=null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, goalNode.transform.position,
                Quaternion.identity);
            iTween.ScaleFrom(goalInstance,iTween.Hash(
                "scale",Vector3.zero,
                "time",drawGoalTime,
                "delay",drawGoalDelay,
                "easetype",drawGoalEaseType));
        }
    }

    public void InitBoard()
    {
        if (PlayerNode != null)
        {
            PlayerNode.InitNode();
        }
    }
}
