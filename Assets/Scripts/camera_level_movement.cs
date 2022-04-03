using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_level_movement : MonoBehaviour
{
    private Transform player;
    private float left_bound, right_bound;
    private Vector3 lerp_target;
    private float lerp_multiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        left_bound = transform.localPosition.x - 4f;
        right_bound = transform.localPosition.x + 4f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(new Vector3(left_bound, transform.localPosition.y, 0f), Vector2.up * 10f, Color.green);
        //Debug.DrawRay(new Vector3(right_bound, transform.localPosition.y, 0f), Vector2.up * 10f, Color.green);
        //Debug.DrawRay(new Vector3(lerp_target.x, transform.localPosition.y, 0f), Vector2.up * 10f, Color.red);

        if (player.position.x > right_bound)
        {
            lerp_target = new Vector3(right_bound + 8f, transform.position.y, -10f);
            right_bound = player.position.x;
            left_bound = right_bound - 8f;
            lerp_multiplier = 0.75f;
        }
        else if (player.position.x < left_bound)
        {
            lerp_target = new Vector3(left_bound - 8f, transform.position.y, -10f);
            left_bound = player.position.x;
            right_bound = left_bound + 8f;
            lerp_multiplier = 0.75f;
        }
        else
        {
            //lerp_target = new Vector3(right_bound - 4f, transform.position.y, -10f);
            //lerp_multiplier = 0.5f;
            lerp_target = Vector3.Lerp(lerp_target, new Vector3(right_bound - 4f, transform.position.y, -10f), Time.deltaTime * 2f);
        }
    }

    private void FixedUpdate()
    {
        Vector3 smoothed_position = Vector3.Lerp(transform.position, lerp_target, Time.fixedDeltaTime * 0.5f);
        transform.position = smoothed_position;
    }
}
