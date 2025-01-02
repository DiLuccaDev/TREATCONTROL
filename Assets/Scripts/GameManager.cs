using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
#region // SINGLETON
    private static GameManager instance = null;
    
    private GameManager(){}

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
    }
#endregion
    
    public static bool IsGameStarted = false;
    public static bool IsGamePaused = false;
    public static bool IsStrangeItemInPlay = false;

    private void Start()
    {
        IsGameStarted = false;
        PlayerInputManager.instance.EnableJoining();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        StartCoroutine(CountdownCoroutine());
    }

    public void EndGame()
    {
        GameHUDManager.Instance.TriggerNotification("STOP");
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController2D currPlayer;
            if (currPlayer = player.GetComponent<PlayerController2D>())
            {
                currPlayer.SetRigidbodyEnabled(false);
            }
        }
        StartCoroutine(DetermineWinner());
    }

    public static int GetPlace(TrickOrTreater player)
    {
        int place = 1;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p.gameObject != player.gameObject)
            {
                if (p.GetComponent<TrickOrTreater>().Score > player.Score)
                {
                    place++;
                }
            }
        }
        return place;
    }

    private IEnumerator CountdownCoroutine()
    {
        GameHUDManager.Instance.TriggerNotification("3");
        yield return new WaitForSeconds(1f);
        GameHUDManager.Instance.TriggerNotification("2");
        yield return new WaitForSeconds(1f);
        GameHUDManager.Instance.TriggerNotification("1");
        yield return new WaitForSeconds(1f);
        GameHUDManager.Instance.StartTimer();
        GameHUDManager.Instance.TriggerNotification("PLAY");
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            TrickOrTreater currPlayer;
            if (currPlayer = player.GetComponent<TrickOrTreater>())
            {
                currPlayer.StartGame();
            }
        }
        IsGameStarted = true;
        PlayerInputManager.instance.DisableJoining();
    }

    private IEnumerator DetermineWinner()
    {
        yield return new WaitForSeconds(2f);

        List<TrickOrTreater> leaders = new List<TrickOrTreater>();
        int leaderScore = 0;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            TrickOrTreater currPlayer = player.GetComponent<TrickOrTreater>();
            if (currPlayer.Score > leaderScore)
            {
                leaderScore = currPlayer.Score;
                leaders.Clear();
                leaders.Add(currPlayer);
            }
            else if (currPlayer.Score == leaderScore)
            {
                leaders.Add(currPlayer);
            }
        }
        
        string winnerNames = "";
        foreach (TrickOrTreater winner in leaders)
        {
            winner.SetWinner();
            winnerNames += winner.name + " AND ";
        }
        // "Player 1 AND Player 2 AND"
        winnerNames = winnerNames.Substring(0, winnerNames.Length-5);
        print(winnerNames);
        GameHUDManager.Instance.TriggerNotification(winnerNames + " WINS!");

        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}