using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float cardinalMoveSpeed;
    private float diagonalMoveSpeed;
    public bool movementOn; //player can move?

    // Start is called before the first frame update
    void Start()
    {
        movementOn = true;
        cardinalMoveSpeed = 4.5f;
        diagonalMoveSpeed = Mathf.Sqrt(cardinalMoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementOn)
        {
            //check for being within borders
            if (transform.position.x >= 7.5f)
                transform.position = new Vector2(7.5f, transform.position.y);
            else if (transform.position.x <= -7.5f)
                transform.position = new Vector2(-7.5f, transform.position.y);
            if (transform.position.y >= 4.5f)
                transform.position = new Vector2(transform.position.x, 4.5f);
            else if (transform.position.y <= -4.5f)
                transform.position = new Vector2(transform.position.x, -4.5f);

            //update player position by current speed
            //check if moving diagonally to adjust speed accordingly
            bool diagonalMotion = false;
            //temporarily set target pos to curr pos
            Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y);

            //up
            if (Input.GetKey(KeyCode.W))
            {
                targetPosition.y = targetPosition.y + cardinalMoveSpeed;
                diagonalMotion = true;
                
            }
            //down
            if (Input.GetKey(KeyCode.S))
            {
                targetPosition.y = targetPosition.y - cardinalMoveSpeed;
                diagonalMotion = true;

            }
            //left
            if (Input.GetKey(KeyCode.A))
            {
                //if moving diagonally, adjust speed
                if (diagonalMotion)
                    targetPosition.x = targetPosition.x - diagonalMoveSpeed;
                else
                    targetPosition.x = targetPosition.x - cardinalMoveSpeed;

            }
            //right
            if (Input.GetKey(KeyCode.D))
            {
                //if moving diagonally, adjust speed
                if (diagonalMotion)
                    targetPosition.x = targetPosition.x + diagonalMoveSpeed;
                else
                    targetPosition.x = targetPosition.x + cardinalMoveSpeed;

            }

            //apply changes linearly
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
    }
}
