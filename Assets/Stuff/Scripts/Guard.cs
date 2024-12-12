using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public float Speed;
    Rigidbody rb;
    [SerializeField]
    bool move;
    [SerializeField]
    bool turn;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (move) {
            rb.velocity = transform.forward * Speed * Time.deltaTime;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") && !turn)
        {
            StartCoroutine(Turn());
        }
        if (collision.gameObject.CompareTag("Floor")) { collision.collider.gameObject.GetComponent<Floor>().WalkedOn(); }
    }
    public void Move()
    {
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
}
