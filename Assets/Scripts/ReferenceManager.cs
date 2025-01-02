using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
#region // REFERENCES
    public GameObject Goo;
    public GameObject Hole;
    public RuntimeAnimatorController[] Characters;
    public Item[] Items;
    public StrangeItem[] StrangeItems;
    public ItemReference[] FirstItemPool;
    public ItemReference[] LastItemPool;
    public ItemReference[] NeitherFirstNorLastItemPool;

#endregion
#region // SINGLETON
    private static ReferenceManager instance = null;
    
    private ReferenceManager(){}

    public static ReferenceManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ReferenceManager();
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
    }
#endregion

    public RuntimeAnimatorController GetCharacter(int charIndex)
    {
        return Characters[charIndex % Characters.Length];
    }

    public Item GetRandomItem(bool isFirst, bool isLast)
    {
        Item itemToGive;

        if (isFirst)
        {
            if (GameManager.IsStrangeItemInPlay)
            {
                itemToGive = Instantiate(FirstItemPool[Random.Range(0, 1)].item);
            } else {
                itemToGive = Instantiate(FirstItemPool[Random.Range(0, FirstItemPool.Length)].item);
            }
        }
        else if (isLast)
        {
            if (GameManager.IsStrangeItemInPlay)
            {
                itemToGive = Instantiate(LastItemPool[Random.Range(0, 2)].item);
            } else {
                itemToGive = Instantiate(LastItemPool[Random.Range(0, LastItemPool.Length)].item);
            }
        }
        else
        {
            if (GameManager.IsStrangeItemInPlay)
            {
                itemToGive = Instantiate(NeitherFirstNorLastItemPool[Random.Range(0, 2)].item);
            } else {
                itemToGive = Instantiate(NeitherFirstNorLastItemPool[Random.Range(0, NeitherFirstNorLastItemPool.Length)].item);
            }
        }
        print("Item ID: "+itemToGive.ID);
        return itemToGive;
    }
}

[System.Serializable]
public struct ItemReference
{
    public Item item;
    public float weight;
}