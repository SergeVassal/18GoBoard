using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMan : MonoBehaviour 
{
    [SerializeField]
    private int sceneToLoadIndex;

    private void Start()
    {
        SceneManager.LoadScene(sceneToLoadIndex);
    }

}
