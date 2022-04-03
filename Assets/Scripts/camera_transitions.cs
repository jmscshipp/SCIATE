using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_transitions : MonoBehaviour
{
    public int currentLevel = 0;
    public level_info[] levels;
    public bool transition = false;
    public float transition_speed = 10f;
    private float lerp_amount = 0f;
    private Camera cam;
    private Vector3 oldPos;
    private Vector3 newPos;
    private float oldSize;
    private float newSize;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // will need to tweak this, but could be easy to make save system by using playerprefs on currentlevel
        transform.position = new Vector3(levels[currentLevel].transform.localPosition.x, levels[currentLevel].transform.localPosition.y, -10); 
        cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            level_complete();
        }*/

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (transition)
        {
            lerp_amount += Time.deltaTime * transition_speed;
            transform.position = Vector3.Lerp(oldPos, newPos, lerp_amount);
            cam.orthographicSize = Mathf.Lerp(oldSize, newSize, lerp_amount);
            if (lerp_amount >= 1f)
            {
                player.transform.position = levels[currentLevel].player_spawn_pos;
                movement.movement_enabled = true;
                transition = false;
            }
        }
    }
    /// <summary>
    /// This is the level complete method.
    /// </summary>
    public void level_complete() // called when player beats a level
    {
        if (currentLevel < levels.Length)
        {
            lerp_amount = 0f;
            oldPos = transform.position;
            newPos = new Vector3(levels[++currentLevel].transform.localPosition.x, levels[currentLevel].transform.localPosition.y, -10); // level incremented here
            oldSize = cam.orthographicSize;
            newSize = levels[currentLevel].camera_size;
            transition = true;
        }
    }
}
