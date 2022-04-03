using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flip_direction : MonoBehaviour
{
    private movement move_script; // change back to regular movement

    // Start is called before the first frame update
    void Start()
    {
        move_script = gameObject.GetComponentInParent<movement>(); // change back to regular movement
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tile_side")
            move_script.flip_dir();
    }
}
