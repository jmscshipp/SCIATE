using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public GameObject gate;
    private bool move = false;
    
	private Vector2[] checkpoints = new Vector2[4];
	float counter = 0f;
	Vector2 myPosition;

    private key_background_fx childFx;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePoints(transform.position, gate.transform.position);
        childFx = transform.Find("key_background_grafix").GetComponent<key_background_fx>();
    }

	// Update is called once per frame
	void Update()
    {
        if (move == true)
        {
            counter += Time.deltaTime / 3f;
            GetBezier(out myPosition, checkpoints, counter);

            if (Vector2Equal(transform.position, checkpoints[3]))
            {
                gate.GetComponent<gate>().key_arrived();
                Destroy(gameObject);
            }
            else
                transform.position = myPosition;
        }
    }

    void GeneratePoints(Vector2 start, Vector2 end)
    {
        checkpoints[0] = start;
        checkpoints[3] = end;

        float negative;
        Vector2 randOffset;

        // creating first checkpoint
        Vector2 one = start + (end - start) / 3.0f; // first third
        negative = Random.Range(0f, 1f);
        negative = (negative > 0.5f) ? 1f : -1f;
        randOffset = new Vector2(0f, Random.Range(0.5f, 1f) * negative);
        one += randOffset * Vector2.Distance(one, start);
        checkpoints[1] = one;

        // creating second checkpoint
        Vector2 two = end - (end - start) / 4.0f; // last quarter
        negative = Random.Range(0f, 1f);
        negative = (negative > 0.5f) ? 1f : -1f;
        randOffset = new Vector2(0f, Random.Range(0.5f, 1f) * negative);
        two += randOffset * Vector3.Distance(end, two);
        checkpoints[2] = two;
    }

    void GetBezier(out Vector2 pos, Vector2[] points, float time)
    {
        float tt = time * time;
        float ttt = time * tt;

        float u = 1f - time;
        float uu = u * u;
        float uuu = u * uu;

        pos = uuu * points[0];
        pos += 3f * uu * time * points[1];
        pos += 3f * u * tt * points[2];
        pos += ttt * points[3];
    }

    public bool Vector2Equal(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) < 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            StartCoroutine(TriggeredCoroutine());
        }
    }

    private IEnumerator TriggeredCoroutine()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        childFx.GoInwards();
        yield return new WaitForSeconds(0.2f);
        move = true;
    }
}
