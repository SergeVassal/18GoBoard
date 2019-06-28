﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Vector3 Destination { get; private set; }
    public bool IsMoving { get; private set; }
    public iTween.EaseType EaseType { get { return easeType; } private set { easeType = value; } }

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float iTweenDelay= 0f;
    [SerializeField] private iTween.EaseType easeType=iTween.EaseType.easeInOutExpo;



    void Start ()
    {        


    }

    public void Move(Vector3 destinationPos,float delayTime=0.25f)
    {
        StartCoroutine(MoveRoutine(destinationPos,delayTime));
    }

    private IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        IsMoving = true;
        Destination = destinationPos;
        yield return new WaitForSeconds(delayTime);

        iTween.MoveTo(gameObject, iTween.Hash(
            "x",destinationPos.x,
            "y", destinationPos.y,
            "z", destinationPos.z,
            "delay",iTweenDelay,
            "easetype",easeType,
            "speed",moveSpeed
            ));

        while (Vector3.Distance(destinationPos,transform.position)>0.01f)
        {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destinationPos;
        IsMoving = false;
    }
    
    public void MoveLeft()
    {
        Vector3 newPosition = transform.position + new Vector3(-2f, 0f, 0f);
        Move(newPosition,0);
    }

    public void MoveRight()
    {
        Vector3 newPosition = transform.position + new Vector3(2f, 0f, 0f);
        Move(newPosition, 0);
    }

    public void MoveForward()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 0f, 2f);
        Move(newPosition, 0);
    }

    public void MoveBackward()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 0f, -2f);
        Move(newPosition, 0);
    }    

}