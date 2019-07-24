using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    [SerializeField] private float delay;

    private Board board;
    private PlayerManager playerManager;

    private bool hasLevelStarted;
    public bool HasLevelStarted { get; private set; } 
    
    private bool isGamePlaying;
    public bool IsGamePlaying { get; private set; }

    private bool isGameOver;
    public bool IsGameOver { get; private set; }

    private bool hasLevelFinished;
    public bool HasLevelFinished { get; private set; }

    
    private void Awake()
    {
        board = UnityEngine.Object.FindObjectOfType<Board>().GetComponent<Board>();
        playerManager= UnityEngine.Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if(playerManager!=null && board != null)
        {
            StartCoroutine(RunGameLoop());
        }
        else
        {
            Debug.LogWarning("GameManager Error: no player or board found!");
        }           
    }

    private IEnumerator RunGameLoop()
    {
        yield return StartCoroutine(StartLevelRoutine());
        yield return StartCoroutine(PlayLevelRoutine());
        yield return StartCoroutine(EndLevelRoutine());
    }

    private IEnumerator StartLevelRoutine()
    {
        Debug.Log("Starting level!");
        playerManager.playerInput.InputEnabled = false;
        while (!hasLevelStarted)
        {
            yield return null;
        }

        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    private IEnumerator PlayLevelRoutine()
    {
        Debug.Log("Play level!");
        isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        playerManager.playerInput.InputEnabled = true;
        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }
        while (!isGameOver)
        {
            yield return null;
        }
    }

    private IEnumerator EndLevelRoutine()
    {
        Debug.Log("End level!");
        playerManager.playerInput.InputEnabled = false;
        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }
        while (!hasLevelFinished)
        {
            yield return null;
        }

        RestartLevel();
    }

    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        hasLevelStarted = true;
    }
}
