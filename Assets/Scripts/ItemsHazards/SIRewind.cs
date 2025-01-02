using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIRewind : StrangeItem
{
    public override void Use()
    {
        if (owner.IsStrange)
        {
            GameHUDManager.Instance.ChangeRemoteHUD("Rewind");
            GameObject[] currPlayers = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < currPlayers.Length; i++)
            {
                if (owner.gameObject != currPlayers[i])
                {
                    print(currPlayers[i].name);
                    currPlayers[i].GetComponent<Rewind>().StartRewind();
                }
            }
            base.Use();
        }
    }
}