using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // this script controls the player jumping and moving left to right
    public static bool movement_enabled = true;
    private float move_speed = 7f;
    [HideInInspector]
    public int move_dir = 1;
    private bool can_flip = true; // these vars for when multiple wall collisions happen at the same time
    private float last_flip_time = 0f;

    public bool in_air = false; // for intangible script
    public bool on_ground = false;
    public bool jumping = false;
    private bool can_jump = true;
    private float last_jump_time = 0f;

    private player_sprite_manager sprite_manager;
    private Rigidbody2D physics;
    private float fallApexHeight = 0f;
    public bool heightCheck = false;
    private bool fall = false; // for when player falls off edge instead of jumping

    private void Start()
    {
        sprite_manager = gameObject.GetComponentInParent<player_sprite_manager>();
        physics = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && on_ground && can_jump && movement_enabled)
        {
            jump();
            sprite_manager.jumping();
        }

        if (physics.velocity.y < 0f && heightCheck)
        {
            fallApexHeight = transform.position.y;
            heightCheck = false;
        }
        
        if (last_jump_time + 0.1f <= Time.time)
            can_jump = true;

        if (last_flip_time + 0.02f <= Time.time)
            can_flip = true;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * Time.fixedDeltaTime * move_speed * move_dir);  
    }

    private IEnumerator set_bools() // for intangible script
    {
        yield return new WaitForSeconds(0.1f);
        on_ground = false;
        in_air = true;
    }

    private void jump()
    {
        move_speed = 5f; // NEW
        physics.AddForce(Vector2.up * 600f);
        last_jump_time = Time.time;
        jumping = true;
        can_jump = false;
        heightCheck = true;
        StartCoroutine(set_bools());
    }

    public void flip_dir()
    {
        if (can_flip)
        {
            move_dir *= -1;
            last_flip_time = Time.time;
            sprite_manager.flip(false, 1, on_ground); // graphics
            can_flip = false;
        } 
    }

    public void set_grounded_state(bool is_grounded)
    {
        if (!in_air && !fall && physics.velocity.y < 0f)
        {
            fallApexHeight = transform.position.y;
            StartCoroutine(set_fall());
        }
        if (can_jump)
        {
            if (in_air == true || (fall && on_ground))
            {
                sprite_manager.landed(Mathf.Abs(transform.position.y - fallApexHeight)); 
                fallApexHeight = transform.position.y;
                fall = false;
            }
            on_ground = is_grounded;
            in_air = false;
            move_speed = 7f; // NEW
        }
    }

    IEnumerator set_fall()
    {
        yield return new WaitForSeconds(0.05f);
        fall = true;
    }
}