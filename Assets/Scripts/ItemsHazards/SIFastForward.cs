using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIFastForward : StrangeItem
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private float speed = 2;
    public override void Use()
    {
        if (owner.IsStrange)
        {
            GameHUDManager.Instance.ChangeRemoteHUD("FastForward");
            GlobalEffectManager.Instance.TriggerFastForward(duration, speed);
            base.Use();
        }
    }
}