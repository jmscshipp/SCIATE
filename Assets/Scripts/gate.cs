using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gate : MonoBehaviour
{
    public int num_keys_required; // number of keys needed to open this gate
    private int num_keys_obtained;
    public List<GameObject> key_graphics = new List<GameObject>();

    // can go back later and make unlocking anims look pretty and stuff !!

    private IEnumerator close_gate()
    {
        GetComponent<Animator>().SetBool("gate_opened", true);
        yield return new WaitForSeconds(0.37f);
        Destroy(gameObject);
    }

    public void key_arrived()
    {
        GameObject tempKey = key_graphics[0];
        key_graphics.Remove(tempKey);
        Destroy(tempKey);
        num_keys_obtained++;
        if (num_keys_obtained == num_keys_required)
            StartCoroutine(close_gate());
    }
}
