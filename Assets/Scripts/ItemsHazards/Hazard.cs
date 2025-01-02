using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;

    void Start()
    {
        if (lifeTime > 0)
            StartCoroutine(DeathTimer(lifeTime));
    }
    private IEnumerator DeathTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
