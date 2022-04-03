using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_sprite_manager : MonoBehaviour
{
    public Sprite[] idle_sprite;
    public Sprite landing_sprite;
    public Sprite jumping_sprite;
    public Sprite[] falling_sprite;
    private Player_state current_state;
    public SpriteRenderer this_renderer;
    private movement move_script;

    private float idle_timer = 0f;
    private int idle_num = 0; // start at first frame
    private int idle_addition_num = 1; // switches between 1 and -1 to go back and forth between two frames

    private bool interruption = false;


    private IEnumerator fallQueue;

    private enum Player_state
    {
        idle,
        landing,
        jumping, // maybe add idle frame between jumping and falling
        falling_low,
        falling_high,
        intangible // using pass thru ability
    }

    private void Start()
    {
        move_script = GetComponent<movement>();
        change_state(Player_state.idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_state == Player_state.idle)
        {
            idle_timer += Time.deltaTime;
            if (idle_timer >= 0.5f)
            {
                idle_num += idle_addition_num;
                idle_addition_num *= -1;
                this_renderer.sprite = idle_sprite[idle_num];
                idle_timer = 0f;
            }
        }
    }

    private void change_state(Player_state new_state)
    {
        current_state = new_state;
        // do other stuff
        switch (new_state)
        {
            case Player_state.idle: // idle
                idle_timer = 0f;
                idle_num = 0;
                idle_addition_num = 1;
                this_renderer.sprite = idle_sprite[idle_num];
                break;
            case Player_state.landing: // call landed() when the apex of the jump and landing y are at least a certain number. wait until jumping and falling are in probably.
                this_renderer.sprite = landing_sprite;
                StartCoroutine(delayed_change_state(0.5f, Player_state.idle));
                interruption = false;
                break;
            case Player_state.jumping: // jumping
                this_renderer.sprite = jumping_sprite;
                if (fallQueue != null)
                {
                    StopCoroutine(fallQueue);
                    fallQueue = null;
                }
                StartCoroutine(delayed_change_state(0.5f, Player_state.falling_low));
                interruption = false;
                break;
            case Player_state.falling_low: // falling
                this_renderer.sprite = falling_sprite[0];
                fallQueue = delayed_change_state(0.5f, Player_state.falling_high);
                StartCoroutine(fallQueue);
                break;
            case Player_state.falling_high: // falling
                this_renderer.sprite = falling_sprite[1];
                break;
            default:
                break;
        }
    }
    private IEnumerator delayed_change_state(float delay_length, Player_state _new_state)
    {
        yield return new WaitForSeconds(delay_length);
        // put interruption check bool here
        if (interruption)
            yield break;
        change_state(_new_state);
    }

    public void flip(bool set_val = false, int val = 1, bool grounded = false) // called by collider and wall jump when time to reverse graphics (and directions)
    {
        if (set_val)
            this_renderer.transform.localScale = new Vector3(val, 1f, 1f);
        else if (move_script.move_dir == this_renderer.transform.localScale.x)
            this_renderer.transform.localScale = new Vector3(this_renderer.transform.localScale.x * -1f, 1f, 1f);
    }
    public void landed(float fall_distance) // called by movement when played hits the ground
    {
        if (fallQueue != null)
        {
            StopCoroutine(fallQueue);
            fallQueue = null;
        }
        interruption = true;
        if (fall_distance >= 4.2f)
            change_state(Player_state.landing);
        else
            change_state(Player_state.idle);
    }

    public void jumping()
    {
        interruption = true;
        change_state(Player_state.jumping);
    }
}
