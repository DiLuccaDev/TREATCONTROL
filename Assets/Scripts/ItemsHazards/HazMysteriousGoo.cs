using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HazMysteriousGoo : Hazard
{
    [SerializeField] private float goovementSpeed = 1f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<PlayerController2D>().SetMovementSpeed(goovementSpeed);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        col.GetComponent<PlayerController2D>().ResetMovementSpeed();
    }
    
    //Lunar made me do this do to lewd comments... 
    //ended up being a pretty decent mechanic...
    //ffs.
    //smh.
    void Update()
    {
        transform.localScale = new Vector3
            (transform.localScale.x + 0.0001f,
            transform.localScale.y + 0.0001f,
            transform.localScale.z + 0.0001f);
    }
}
