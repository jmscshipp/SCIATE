using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_background_fx : MonoBehaviour
{
    Vector3 rot = Vector3.zero;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 70f;
    }

    // Update is called once per frame
    void Update()
    {
        rot.z += Time.deltaTime * speed;
        if (rot.z >= 360f)
            rot.z = 0f;
        transform.rotation = Quaternion.Euler(rot);
    }

    // called by key, triggers animation
    public void GoInwards()
    {
        speed = 0f;
        GetComponent<Animator>().SetBool("go_inward", true);
    }
}
