using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public bool movementOn; //player can move?

    // Start is called before the first frame update
    void Start()
    {
        movementOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        //check for being within borders
        if (transform.position.x >= 7.5f)
            transform.position = new Vector2(7.5f, transform.position.y);
        else if(transform.position.x <= -7.5f)
            transform.position = new Vector2(-7.5f, transform.position.y);
        if (transform.position.y >= 4.5f)
            transform.position = new Vector2(transform.position.x, 4.5f);
        else if (transform.position.y <= -4.5f)
            transform.position = new Vector2(transform.position.x, -4.5f);

        //update player position by current speed
        //up
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        //down
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        //left
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        //right
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}
