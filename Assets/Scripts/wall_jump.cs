using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall_jump : MonoBehaviour
{
    public Transform feet;
    public GameObject popup_prefab;
    private movement move_script;
    private GameObject last_popup;
    private bool appeared = false;
    private float appeared_time = 0f;
    private Rigidbody2D body_physics;
    public bool just_jumped = false; // for intangible script

    private player_sprite_manager sprite_manager;

    // Start is called before the first frame update
    void Start()
    {
        move_script = gameObject.GetComponentInParent<movement>();
        body_physics = gameObject.GetComponentInParent<Rigidbody2D>();
        sprite_manager = gameObject.GetComponentInParent<player_sprite_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (appeared)
        {
            if (Input.GetKeyDown(KeyCode.Space) && movement.movement_enabled)
            {
                perform_wall_jump();
            }
            if (Time.time >= appeared_time + 0.3f)
            {
                Destroy(last_popup);
                Time.timeScale = 1f;
                appeared = false;
            }
        }
        if (Time.time >= appeared_time + 0.1f) // for intangible script
            just_jumped = false;
    }

    private void perform_wall_jump()
    {
        move_script.heightCheck = true;
        if (last_popup.transform.position.x > transform.position.x)
        {
            move_script.move_dir = -1;
            sprite_manager.flip(true, 1); // graphics, opposite because base orientation faces right (-1)
        }
        else
        {
            move_script.move_dir = 1;
            sprite_manager.flip(true, -1); // graphics, opposite because base orientation faces right (-1)
        }
        if (move_script.on_ground)
            body_physics.AddForce(Vector2.up * 300f);
        else
            body_physics.AddForce(Vector2.up * 600f);

        body_physics.velocity = new Vector2(body_physics.velocity.x, Mathf.Clamp(body_physics.velocity.x, -12f, 17.4f));
        last_popup.GetComponent<popup>().activate();
        Time.timeScale = 0.6f;
        StartCoroutine(screen_shake());
        appeared = false;
    }


    private void spawn_popup()
    {
        Destroy(last_popup);
        Vector2 spawn_location = new Vector2(feet.position.x + move_script.move_dir * 0.5f, feet.position.y);
        if (move_script.on_ground)
        {
            last_popup = Instantiate(popup_prefab, spawn_location, Quaternion.Euler(0f, 0f, -move_script.move_dir * 45f));

        }
        else
            last_popup = Instantiate(popup_prefab, spawn_location, Quaternion.identity);
        last_popup.transform.localScale = new Vector2(-move_script.move_dir, 1f);
        appeared_time = Time.time;
        appeared = true;
        just_jumped = true;
    }
    /// <summary>
    /// Trigger that 2D dude.
    /// </summary>
    /// <param name="collision">The collision needs a super cool component</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tile_side" && !appeared && ((feet.position.x - collision.transform.position.x > 0 && move_script.move_dir < 0) || (feet.position.x - collision.transform.position.x < 0 && move_script.move_dir > 0)))
        {
            spawn_popup();
            Time.timeScale = 0.8f;
        }
    }

    IEnumerator screen_shake()
    {
        Vector3 camPos = Camera.main.transform.position;
        for (int i = 0; i < 5; i++)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Random.insideUnitCircle.x * 0.1f, Camera.main.transform.position.y + Random.insideUnitCircle.y * 0.1f, camPos.z);
            yield return new WaitForSeconds(0.01f);
            Camera.main.transform.position = camPos;
        }
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1f;
    }
}
