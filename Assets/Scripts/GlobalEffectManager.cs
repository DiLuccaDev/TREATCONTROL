using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Android;

public class GlobalEffectManager : MonoBehaviour
{
#region // SINGLETON
    private static GlobalEffectManager instance = null;
    
    private GlobalEffectManager(){}

    public static GlobalEffectManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GlobalEffectManager();
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
    }
#endregion

    private IEnumerator fastForwardCoroutine;

    public void TriggerFastForward(float waitTime, float speed)
    {
        if (fastForwardCoroutine != null)
        {
            StopCoroutine(fastForwardCoroutine);
        }
        fastForwardCoroutine = FastForwardTimer(waitTime, speed);
        StartCoroutine(fastForwardCoroutine);
    }

    private IEnumerator FastForwardTimer(float waitTime, float speed)
    {
        Time.timeScale = speed;
        yield return new WaitForSeconds(waitTime*speed);
        Time.timeScale = 1;
        GameHUDManager.Instance.ChangeRemoteHUD("Play");
    }
}
