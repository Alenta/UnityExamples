using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public float Speed;
    public float CheckTime;
    Rigidbody rb;
    bool move;
    bool turn;
    bool inMaze = true;
    int steps = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (move)
        {
            rb.velocity = transform.forward * Speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !turn)
        {
            StartCoroutine(Turn());
        }
        if (collision.gameObject.CompareTag("Floor")) { Floor floor = collision.gameObject.GetComponentInChildren<Floor>();
            if (floor && !floor.HasBeenWalkedOn())
            {
                steps++;
                floor.WalkedOn();
            }
            else print(floor);
        }
    }
    public void Move()
    {
        StartCoroutine(BoundaryCheck());
        move = true;
    }

    IEnumerator Turn()
    {
        if (turn) yield break;
        move = false;
        turn = true;
        rb.velocity = Vector3.zero;
        //rb.freezeRotation = false;
        transform.Rotate(0, 90, 0);
        rb.freezeRotation = true;
        Move();
        yield return new WaitForSeconds(0.1f);
        turn = false;
    }

    IEnumerator BoundaryCheck()
    {
        while (inMaze)
        {
            inMaze = IsWithinTheMaze();
            yield return new WaitForSeconds(CheckTime);         
        }
        print("I left the maze at " + (steps-1) + " unique steps");
    }

    bool IsWithinTheMaze()
    {
        if (transform.position.x < 0 || transform.position.x > 128 ||
            transform.position.z < 0 || transform.position.z > 135)
        {
            return false;
        }
        else return true;
    }
}
