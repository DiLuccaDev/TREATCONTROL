using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public string itemName;
    public float itemDropOffset = 1;
    public TrickOrTreater owner;
    public int ID;

    /*public Item(Item item)
    {
        item.itemSprite = itemSprite;
        item.itemName = itemName;
        item.itemDropOffset = itemDropOffset;
        item.owner = owner;
    }*/
    void Start()
    {
        print("STARTED ITEM");
        ID = Random.Range(0,99999);
    }

    public virtual void Use()
    {
        print(owner);
        owner.CurrentItem = null;
        owner = null;
        Drop();
    }

    public virtual void Use(Vector2 v)
    {
        Use();
    }

    public virtual void Drop()
    {
        Destroy(gameObject);
    }
}