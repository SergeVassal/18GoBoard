using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutoTest : MonoBehaviour 
{
    private float moveDelay=1.2f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene current,Scene next)
    {
        StartTesting();
    }

    private void StartTesting()
    {
        GameObject startButtonGO = GameObject.Find("StartButton");
        Button startButton = startButtonGO.GetComponent<Button>();
        startButton.onClick.Invoke();

        StartCoroutine(MovePlayer());    
    }

    private IEnumerator MovePlayer()
    {
        yield return new WaitForSeconds(3f);

        GameObject player = GameObject.Find("Player");
        PlayerMover playerMover = player.GetComponent<PlayerMover>();
        playerMover.MoveRight();

        yield return new WaitForSeconds(moveDelay);

        playerMover.MoveBackward();

        yield return new WaitForSeconds(moveDelay);

        playerMover.MoveLeft();

        yield return new WaitForSeconds(moveDelay);

        playerMover.MoveBackward();

        yield return new WaitForSeconds(moveDelay);

        playerMover.MoveRight();

        yield return new WaitForSeconds(moveDelay);

        playerMover.MoveBackward();
    }
}
