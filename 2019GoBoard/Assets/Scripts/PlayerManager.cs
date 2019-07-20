using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour 
{
    public PlayerInput playerInput;

    private PlayerMover playerMover;


    private void Awake()
    {
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.InputEnabled = true;
    }

    private void Update()
    {
        if (playerMover.IsMoving)
        {
            return;
        }

        playerInput.GetKeyInput();

        if (playerInput.Vertical == 0)
        {
            if (playerInput.Horizontal < 0)
            {
                playerMover.MoveLeft();
            }
            else if (playerInput.Horizontal > 0)
            {
                playerMover.MoveRight();
            }
        }
        else if (playerInput.Horizontal== 0)
        {
            if (playerInput.Vertical < 0)
            {
                playerMover.MoveBackward();
            }
            else if (playerInput.Vertical > 0)
            {
                playerMover.MoveForward();
            }
        }
    }

}
