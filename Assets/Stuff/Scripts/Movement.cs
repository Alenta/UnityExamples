using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    public float Speed;
    public Transform Cam;
    public bool FlyingCamera;
    Rigidbody _rigidbody;
    Vector3 movementDir;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            movementDir = Cam.transform.forward;
        }
        else if (Input.GetKey("s"))
        {
            movementDir = -Cam.transform.forward;
        }
        else if (Input.GetKey("d"))
        {
            movementDir = Cam.transform.right;
        }
        else if (Input.GetKey("a"))
        {
            movementDir = -Cam.transform.right;
        }
        else
        {
            movementDir = Vector3.zero;
        }
        if (FlyingCamera) transform.position += movementDir * ((Speed * 5) * Time.deltaTime);
        else _rigidbody.velocity = movementDir * ((Speed * 50) * Time.deltaTime);
    }
}
