using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_info : MonoBehaviour
{
    public float camera_size = 13f; // how big the camera size should be for this level
    public Vector2 player_spawn_pos;
    private camera_transitions cam;
    private bool level_beaten = false;

    private void Start()
    {
        cam = Camera.main.GetComponent<camera_transitions>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !level_beaten)
        {
            Debug.Log("how many times you getting called");
            movement.movement_enabled = false; // re enabled in camera transition
            cam.level_complete();
            level_beaten = true;
        }
    }
}
