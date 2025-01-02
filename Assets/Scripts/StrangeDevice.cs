using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeDevice : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        TrickOrTreater player;
        if (player = col.GetComponent<TrickOrTreater>())
        {
            player.IsStrange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        TrickOrTreater player;
        if (player = col.GetComponent<TrickOrTreater>())
        {
            player.IsStrange = false;
        }
    }
}
