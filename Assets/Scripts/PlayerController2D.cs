using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Rewind))]
[RequireComponent(typeof(Animator))]
public class PlayerController2D : MonoBehaviour
{
    public float defaultMovementSpeed = 5f;
    private float currMovementSpeed;
    private float speedMult = 1f;
    private RigidbodyConstraints2D originalConstraint;

// COMPONENTS
    private Rigidbody2D rb;
    private Rewind rewind;
    private Animator anim;

// INPUT
    private Vector2 movementDirection;
    
// INSTANTIATION
    void Start()
    {
        // Set up refs
        rb = GetComponent<Rigidbody2D>();
        rewind = GetComponent<Rewind>();
        anim = GetComponent<Animator>();

        // Set default speed
        currMovementSpeed = defaultMovementSpeed;

        // Turn off movement
        originalConstraint = rb.constraints;
        SetRigidbodyEnabled(false);

        // Set players starting loc
        transform.position = transform.position + (Vector3.right * PlayerInputManager.instance.playerCount);
    }

// EVENTS
    public void SetRigidbodyEnabled(bool isStarted = true)
    {
        if(isStarted)
        {
            rb.constraints = originalConstraint;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!rewind.IsRewinding)
            movementDirection = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.velocity = movementDirection * (currMovementSpeed * speedMult);
        anim.SetFloat("movingUpDown", movementDirection.y);
        anim.SetFloat("movingLeftRight", movementDirection.x);
    }

#region// Getters n' Setters
    public void SetMovementSpeed(float s)
    {
        currMovementSpeed = s;
    }
    public void ResetMovementSpeed()
    {
        currMovementSpeed = defaultMovementSpeed;
    }
#endregion
#region // MODIFIER EFFECTS
    public void StartPause(float pauseTime)
    {
        StartCoroutine(PauseCoroutine(pauseTime));
    }

    private IEnumerator PauseCoroutine(float waitTime)
    {
        speedMult = 0;
        yield return new WaitForSeconds(waitTime);
        speedMult = 1;
        GameHUDManager.Instance.ChangeRemoteHUD("Play");
        GameManager.IsGamePaused = false;
    }
#endregion
}