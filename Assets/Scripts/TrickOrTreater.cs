using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Runtime.CompilerServices;

public class TrickOrTreater : MonoBehaviour
{
#region // PROPERTIES
    // The players current score
    private int score = 0;
    public int Score
    {
        get {return score;}
        set 
        {
            score = value;
            scoreText.text = "CANDY: " + score.ToString();
        }
    }
    [SerializeField] private Item currentItem;
    public Item CurrentItem
    {
        get {return currentItem;}
        set 
        {
            currentItem = value;

            if (currentItem)
            {
                itemImage.sprite = currentItem.itemSprite;
                itemImage.transform.parent.gameObject.SetActive(true);
            } else {
                itemImage.sprite = null;
                itemImage.transform.parent.gameObject.SetActive(false);
            }
        }
    }

// COMPONENTS
    private Rewind rewind;
    private Animator anim;
    private SpriteRenderer rend;

// INPUT
    private bool interacted = false;
    private IInteractable currInteract;
    private bool used = false;
    private bool dropped = false;
    private bool started = false;
    private bool swapped = false;

// HUD
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject winnerHUD;
    [SerializeField] private GameObject identifierArrow;
    
// 
    public bool IsStrange = false;
    // This is an offset for the player sprite sorting order (based on y value, in-game)
    [SerializeField] private int spriteOrderOffset = 200;
    private int characterIndex = 0;

#endregion
#region // EVENTS
    private void Start()
    {
        rewind = GetComponent<Rewind>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        anim.runtimeAnimatorController = ReferenceManager.Instance.GetCharacter(characterIndex);

        name = "Player " + PlayerInputManager.instance.playerCount;
        identifierArrow.SetActive(true);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!rewind.IsRewinding)
            interacted = context.action.triggered;
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (!rewind.IsRewinding)
            used = context.action.triggered;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (!rewind.IsRewinding)
            dropped = context.action.triggered;
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        started = context.action.triggered;
    }

    public void OnSwap(InputAction.CallbackContext context)
    {
        swapped = context.action.triggered;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        currInteract = col.gameObject.GetComponent<IInteractable>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (currInteract == col.gameObject.GetComponent<IInteractable>())
            currInteract = null;
    }
#endregion
#region // INPUTS (UPDATE)
    void Update()
    {

        // Set the players order in layer based on y
        rend.sortingOrder = (int)Mathf.Floor(transform.position.y*-10) + spriteOrderOffset;

        // Did the player INTERACT with a valid interactable?
        if (interacted)
        {
            Interact();
            interacted = false;
        }
        if (used)
        {
            UseItem();
            used = false;
        }
        if (dropped)
        {
            DropItem();
            dropped = false;
        }
        if (started)
        {
            if (!GameManager.IsGameStarted)
            {
                GameManager.Instance.StartGame();
            }
            started = false;
        }
        if (swapped)
        {
            if (!GameManager.IsGameStarted)
            {
                SwapCharacter();
            }
            swapped = false;
        }
    }
#endregion
#region // ACTIONS
    private void Interact()
    {
        if (currInteract != null)
        {
            anim.SetTrigger("trickOrTreat");
            currInteract.Interact(this);
        }
    }

    private void UseItem()
    {
        if (currentItem)
        {
            Vector2 offset = GetComponent<Rigidbody2D>().velocity.normalized;
            Vector2 charPos = GetComponent<CircleCollider2D>().bounds.center;

            if (currentItem is StrangeItem)
            {
                GameManager.IsStrangeItemInPlay = false;
            }

            if (currentItem.itemDropOffset > 0)
            {
                currentItem.Use(charPos-(offset*currentItem.itemDropOffset));
            } else {
                currentItem.Use();
            }
        }
    }

    private void DropItem()
    {
        if (currentItem)
        {
            currentItem.Drop();
            CurrentItem = null;
            if (currentItem is StrangeItem)
            {
                GameManager.IsStrangeItemInPlay = false;
            }
        }
    }

    public void ReceiveCandy(int candy)
    {
        Score = score + candy;
    }

    public void ReceiveItem(Item item)
    {
        print(name + " Receive " + item + " original owner:" + item.owner);
        item.owner = this;
        CurrentItem = item;
        if (item is StrangeItem)
        {
            GameManager.IsStrangeItemInPlay = true;
        }
    }

#endregion
    
    public void SetWinner()
    {
        winnerHUD.SetActive(true);
    }

    public void StartGame()
    {
        GetComponent<PlayerController2D>().SetRigidbodyEnabled(true);
        identifierArrow.SetActive(false);
    }

    private void SwapCharacter()
    {
        anim.runtimeAnimatorController = ReferenceManager.Instance.GetCharacter(++characterIndex);
    }
}