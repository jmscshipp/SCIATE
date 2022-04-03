using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grounded : MonoBehaviour
{
    private movement move_script; // change back to regular movement
    private int collision_count; // number of ground surfaces currently colliding with
    
    public TrailRenderer board_trail;
    private bool trail_active = false;
    private float trail_size_lerp = 0f;

    // Start is called before the first frame update
    void Start()
    {
        move_script = gameObject.GetComponentInParent<movement>(); // change back to regular movement
    }

    private void Update()
    {
        if (trail_active && trail_size_lerp <= 1f)
        {
            trail_size_lerp += Time.deltaTime * 2f;
            board_trail.startWidth = Mathf.Lerp(0f, 0.6f, trail_size_lerp);
            board_trail.endWidth = Mathf.Lerp(0f, 0.3f, trail_size_lerp);
        }
        else if (!trail_active && trail_size_lerp >= 0f)
        {
            trail_size_lerp -= Time.deltaTime * 5f;
            board_trail.startWidth = Mathf.Lerp(0f, 0.6f, trail_size_lerp);
            board_trail.endWidth = Mathf.Lerp(0f, 0.3f, trail_size_lerp);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tile_surface")
        {
            move_script.set_grounded_state(true);
            trail_active = false;
            collision_count++;
            Debug.Log("touched ground at " + Time.time);
        }
            
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tile_surface")
        {
            collision_count--;
            if (collision_count == 0)
            {
                trail_active = true;
                move_script.set_grounded_state(false);
                Debug.Log("left ground at " + Time.time);
            }
        }     
    }
}
