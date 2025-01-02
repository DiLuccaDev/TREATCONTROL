using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HazRatHole : Hazard
{
    public HazRatHole exit;
    private bool isActive = false;
    [SerializeField] private SpriteRenderer image;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isActive)
        {
            if (exit)
            {
                col.transform.position = exit.transform.position;
            }
            image.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!isActive)
        {
            isActive = true;
        }
    }
}
