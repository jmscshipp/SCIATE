using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public Transform middle, back;
    private Camera cam;
    private float middle_start, back_start;
    private float middle_distance, back_distance;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        middle_start = middle.position.x;
        back_start = back.position.x;
    }

    private void FixedUpdate()
    {
        middle_distance = cam.transform.position.x * 0.2f;
        middle.position = new Vector3(middle_start + middle_distance, middle.position.y, middle.position.z);

        back_distance = cam.transform.position.x * 0.5f;
        back.position = new Vector3(back_start + back_distance, back.position.y, back.position.z);
    }
}
