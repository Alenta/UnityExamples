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
    Vector3 movementDir = Vector3.zero;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            movementDir.z = Input.GetAxis("Vertical");
        }
        else movementDir.y = 0;

        if (Input.GetAxis("Horizontal") != 0)
        {
            movementDir.x = Input.GetAxis("Horizontal");
        }
        else movementDir.x = 0;

        if (FlyingCamera) transform.position += (transform.forward * movementDir.z)  * ((Speed * 5) * Time.deltaTime); 
        else _rigidbody.velocity = movementDir * ((Speed * 50) * Time.deltaTime);
    }
}
