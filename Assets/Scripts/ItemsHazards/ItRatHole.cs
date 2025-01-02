using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItRatHole : Item
{    
    [SerializeField] private HazRatHole exit;

    public override void Use(Vector2 dropPos)
    {
        GameObject go = Instantiate(ReferenceManager.Instance.Hole, new Vector3(dropPos.x, dropPos.y, 0), new Quaternion());
        if (!exit)
        {
            go.name = "HOLE (Exit)";
            exit = go.GetComponent<HazRatHole>();
        }
        else
        {
            go.name = "HOLE (Enter)";
            go.GetComponent<HazRatHole>().exit = exit;
            exit = null;
            base.Use();
        }
    }
}