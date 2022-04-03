using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popup : MonoBehaviour
{
    public void activate()
    {
        StartCoroutine(popup_activated());
    }

    private IEnumerator popup_activated()
    {
        transform.localScale *= 2;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
