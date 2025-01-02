using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
#region // SINGLETON
    private static PlayerManager instance = null;
    
    private PlayerManager(){}

    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerManager();
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
    }
#endregion
    public GameObject[] players = new GameObject[4];
    private int joinIndex = 0;
    [SerializeField] private PlayerJoin[] playerJoin;

    private void JoinPlayer(PlayerInput player)
    {
        players[joinIndex] = player.gameObject;
        playerJoin[joinIndex].Join();
        joinIndex++;
    }

    public void JoinPlayer(InputAction.CallbackContext context)
    {
        
    }
}