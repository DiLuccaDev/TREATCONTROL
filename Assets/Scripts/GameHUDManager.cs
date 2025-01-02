using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class GameHUDManager : MonoBehaviour
{
#region // SINGLETON
    private static GameHUDManager instance = null;
    
    private GameHUDManager(){}

    public static GameHUDManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameHUDManager();
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
        startingCard.SetActive(true);
        blackout.SetActive(true);
    }
#endregion
    [SerializeField] private GameObject remoteGO;
    [SerializeField] private GameObject trackGO;
    [SerializeField] private GameObject vertbarGO;
    [SerializeField] private GameObject cornerGO;
    [SerializeField] private Transform[] HUDPositions1P;
    [SerializeField] private Transform[] HUDPositions2P;
    [SerializeField] private Transform[] HUDPositions3P;
    [SerializeField] private Transform[] HUDPositions4P;
    [SerializeField] private GameObject blackout;
    [SerializeField] private GameObject startingCard;

    // Game Time
    [SerializeField] private float gameTime = 180f;
    [SerializeField] private RectTransform trackTimerRect;
    private float trackRectStartingX;
    private IEnumerator gameTimer;

    // Remote Buttons
    [SerializeField] private Image ImagePlay;
    [SerializeField] private Image ImagePause;
    [SerializeField] private Image ImageFastForward;
    [SerializeField] private Image ImageRewind;
    [SerializeField] private Image ImageStop;
    [SerializeField] private Color defaultButtonColor;
    [SerializeField] private Color activeButtonColor;
    [SerializeField] private Color defaultIconColor;
    [SerializeField] private Color activeIconColor;

    // Notification
    [SerializeField] private GameObject GONotification;
    [SerializeField] private TMP_Text textNotification; 
    [SerializeField] private float notificationDuration = 3f;
    [SerializeField] private IEnumerator notificationCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        if (gameTimer != null)
            StopCoroutine(gameTimer);

        if (PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined += 
            player =>
            {
                AdjustHUD(PlayerInputManager.instance.playerCount);
                startingCard.SetActive(false);
                blackout.SetActive(false);
            };
        }

        defaultButtonColor = ImagePlay.color;
        ChangeRemoteHUD("Play");
    }
    
    private void AdjustHUD(int playerCount)
    {
        switch (playerCount)
        {
            case 1:
                remoteGO.transform.position = HUDPositions1P[0].position;
                trackGO.transform.position = HUDPositions1P[1].position;
                vertbarGO.SetActive(false);
                cornerGO.SetActive(false);
                break;
            case 2:
                remoteGO.transform.position = HUDPositions2P[0].position;
                trackGO.transform.position = HUDPositions2P[1].position;
                vertbarGO.SetActive(false);
                cornerGO.SetActive(false);
                break;
            case 3:
                remoteGO.transform.position = HUDPositions3P[0].position;
                trackGO.transform.position = HUDPositions3P[1].position;
                vertbarGO.SetActive(true);
                cornerGO.SetActive(true);
                break;
            case 4:
                remoteGO.transform.position = HUDPositions4P[0].position;
                trackGO.transform.position = HUDPositions4P[1].position;
                vertbarGO.SetActive(true);
                cornerGO.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void StartTimer()
    {
        trackRectStartingX = trackTimerRect.offsetMax.x;
        gameTimer = GameTimerCoroutine(gameTime);
        StartCoroutine(gameTimer);
    }

    private IEnumerator GameTimerCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(2f);
        float normalizedTime = 0;
        
        while(normalizedTime <= 1f)
        {
            if (!GameManager.IsGamePaused)
            {
                if (!Rewind.GameIsRewinding)
                {
                    normalizedTime += Time.deltaTime / waitTime;
                } else {
                    normalizedTime -= Time.deltaTime / waitTime;
                } 
            }
            // y = -(c*x)+c
            trackTimerRect.offsetMax = new Vector2(-(trackRectStartingX*normalizedTime) + trackRectStartingX, trackTimerRect.offsetMax.y);
            yield return null;
        }

        GameManager.Instance.EndGame();
    }

    public void ChangeRemoteHUD(string effect)
    {
        // Reset all graphics
        Image tempImage;
        foreach (Transform child in ImagePlay.transform.parent)
        {
            foreach (Transform gchild in child)
            {
                if (tempImage = gchild.GetComponent<Image>())
                {
                    tempImage.color = defaultIconColor;
                }
            }
            if (tempImage = child.GetComponent<Image>())
            {
                tempImage.color = defaultButtonColor;
            }
        }

        switch (effect)
        {
            case "Play":
                ImagePlay.color = activeButtonColor;
                ImagePlay.transform.GetChild(0).GetComponent<Image>().color = activeIconColor;
            break;
            case "Pause":
                TriggerNotification("PAUSE");
                ImagePause.color = activeButtonColor;
                ImagePause.transform.GetChild(0).GetComponent<Image>().color = activeIconColor;
                ImagePause.transform.GetChild(1).GetComponent<Image>().color = activeIconColor;
            break;
            case "FastForward":
                TriggerNotification("FAST FORWARD");
                ImageFastForward.color = activeButtonColor;
                ImageFastForward.transform.GetChild(0).GetComponent<Image>().color = activeIconColor;
                ImageFastForward.transform.GetChild(1).GetComponent<Image>().color = activeIconColor;
            break;
            case "Rewind":
                TriggerNotification("REWIND");
                ImageRewind.color = activeButtonColor;
                ImageRewind.transform.GetChild(0).GetComponent<Image>().color = activeIconColor;
                ImageRewind.transform.GetChild(1).GetComponent<Image>().color = activeIconColor;
            break;
            case "Stop":
                ImageStop.color = activeButtonColor;
                ImageStop.transform.GetChild(0).GetComponent<Image>().color = activeIconColor;
            break;
            default:
            break;
        }
    }

    public void TriggerNotification(string notification)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        textNotification.text = notification;
        GONotification.SetActive(true);
        notificationCoroutine = NotificationCoroutine();
        StartCoroutine(notificationCoroutine);
    }

    private IEnumerator NotificationCoroutine()
    {
        
        yield return new WaitForSeconds(notificationDuration); 
        GONotification.SetActive(false);
    }
}
