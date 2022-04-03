using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intangible : MonoBehaviour
{
    private movement move_script;
    private wall_jump wall_jump_script;
    private List<BoxCollider2D> floating_tile_colliders = new List<BoxCollider2D>();
    private bool tangible = true;
    private int trigger_count = 0; // number of tile triggers currently interacting w/
    private bool inside_tile = false; // check if currently in a platform
    public SpriteRenderer torso_sprite;
    public TrailRenderer board_trail;

    // Start is called before the first frame update
    void Start()
    {
        move_script = gameObject.GetComponentInParent<movement>();
        wall_jump_script = GetComponentInChildren<wall_jump>();
        GameObject[] floating_tiles = GameObject.FindGameObjectsWithTag("floating_tile");
        foreach (GameObject tile in floating_tiles)
        {
            foreach (BoxCollider2D tile_col in tile.GetComponentsInChildren<BoxCollider2D>())
            {
                if (tile_col.gameObject.tag != "floating_tile")
                    floating_tile_colliders.Add(tile_col);
            }     
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (move_script.in_air == true && Input.GetKeyDown(KeyCode.Space) && wall_jump_script.just_jumped == false)
        {
            foreach (BoxCollider2D tile_col in floating_tile_colliders)
                tile_col.enabled = false;
            torso_sprite.color = new Color(1f, 1f, 1f, .5f);
            board_trail.startColor = new Color(board_trail.startColor.r, board_trail.startColor.g, board_trail.startColor.b, 0.5f);
            tangible = false;
        }
        if (tangible == false)
        {
            movement.movement_enabled = false;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartCoroutine(end());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floating_tile")
        {
            inside_tile = true;
            trigger_count++;
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floating_tile")
        {
            trigger_count--;
            if (trigger_count == 0)
                inside_tile = false;
        }       
    }

    private IEnumerator end()
    {
        while(inside_tile)
            yield return null;
        foreach (BoxCollider2D tile_col in floating_tile_colliders)
            tile_col.enabled = true;
        torso_sprite.color = new Color(1f, 1f, 1f, 1f);
        board_trail.startColor = new Color(board_trail.startColor.r, board_trail.startColor.g, board_trail.startColor.b, 1f);
        movement.movement_enabled = true;
        tangible = true;
    }
}
