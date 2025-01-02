using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIStop : StrangeItem
{
    public override void Use()
    {
        if (owner.IsStrange)
        {
            GameManager.Instance.EndGame();
            GameHUDManager.Instance.ChangeRemoteHUD("Stop");
            base.Use();
        }
    }
}