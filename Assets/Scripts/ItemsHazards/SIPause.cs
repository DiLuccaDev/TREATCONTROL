using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIPause : StrangeItem
{
    [SerializeField] private float pauseTime = 5f;

    public override void Use()
    {
        if (owner.IsStrange)
        {
            GameManager.IsGamePaused = true;
            GameHUDManager.Instance.ChangeRemoteHUD("Pause");
            GameObject[] currPlayers = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < currPlayers.Length; i++)
            {
                if (owner.gameObject != currPlayers[i])
                {
                    currPlayers[i].GetComponent<PlayerController2D>().StartPause(pauseTime);
                }
            }
            
            base.Use();
        }
    }
}