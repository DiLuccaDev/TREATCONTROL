using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoin : MonoBehaviour
{
    private void Start()
    {
        if (PlayerInputManager.instance)
        {
            print("Hello");
            PlayerInputManager.instance.onPlayerJoined += 
            player =>
            {
                Debug.Log($"Player {player.playerIndex} joined.");
            };
        }
    }
    
    public void Join()
    {
        print("Joined");
    }
}
