using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class House : MonoBehaviour, IInteractable
{
    private bool isActive = true;
    [SerializeField, Range(1, 15)]
    private float isActiveInGame = 1f;
    [SerializeField, Range(1, 15)]
    private float cooldown = 1f;
    [SerializeField, Range(0, 15)]
    private float cooldownRandomVariance = 0f;
    private IEnumerator cooldownCoroutine;

    // House
    [SerializeField] private SpriteRenderer houseLight;
    [SerializeField] private Color lightOnColor = Color.yellow;
    [SerializeField] private Color lightOffColor = Color.black;

    // Items
    [SerializeField] private Item itemToGive;

    // Generation
    [SerializeField] private float partRotationRandomVariance = 0f;
    [SerializeField] private float partPositionRandomVariance = 0f;
    [SerializeField] private float partScaleRandomVariance = 0f;

    private void Awake()
    {
        GenerateHouse();
    }

    private void Start()
    {
        ActivateHouse();
    }

    public void Interact(TrickOrTreater instigator)
    {
        if (isActive)
        {
            DeactivateHouse();
            GiveLoot(instigator);
            cooldownCoroutine = Cooldown(Random.Range(cooldown-cooldownRandomVariance, cooldown+cooldownRandomVariance));
            StartCoroutine(cooldownCoroutine);
        }
    } 

    private IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ActivateHouse();
    }

    private void DeactivateHouse()
    {
        isActive = false;
        houseLight.color = lightOffColor;
    }

    private void ActivateHouse()
    {
        isActive = true;
        houseLight.color = lightOnColor;
    }

    private void GiveLoot(TrickOrTreater instigator)
    {
        // TODO: Put % chance of ITEM or CANDY here.
        if (!instigator.CurrentItem && itemToGive)
        {
            instigator.ReceiveItem(Instantiate(itemToGive));
        }
        if (!instigator.CurrentItem && (Random.value) > 0.7f)
        {
            int place = GameManager.GetPlace(instigator);
            instigator.ReceiveItem(Instantiate(ReferenceManager.Instance.GetRandomItem((place == 1), (place < PlayerInputManager.instance.playerCount))));
        } else if (!instigator.CurrentItem) {
            instigator.ReceiveCandy(Random.Range(2,6));
        } else {
            instigator.ReceiveCandy(Random.Range(1,3));
        }
    }

    private void GenerateHouse()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SpriteRenderer rend; 
            if (rend = transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                rend.sortingOrder = (int)Mathf.Floor(transform.position.y*-10) + rend.sortingOrder;

                Transform childTrans = transform.GetChild(i).transform;

                childTrans.position = new Vector3(
                    childTrans.position.x + Random.Range(-partPositionRandomVariance, partPositionRandomVariance),
                    childTrans.position.y + Random.Range(-partPositionRandomVariance, partPositionRandomVariance),
                    childTrans.position.z);

                childTrans.Rotate(0,0,Random.Range(-partRotationRandomVariance, partRotationRandomVariance), Space.Self);

                childTrans.localScale = new Vector3(
                    childTrans.localScale.x + Random.Range(-partScaleRandomVariance, partScaleRandomVariance),
                    childTrans.localScale.y + Random.Range(-partScaleRandomVariance, partScaleRandomVariance),
                    childTrans.localScale.z);
            }
        }
    }
}