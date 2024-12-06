using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorInteraction : MonoBehaviour
{
    public Rigidbody rb;
    public bool StartInactive = false;
    GameObject currentColObject;
    Effector effector;
    private void OnTriggerEnter(Collider other)
    {
        if (!StartInactive)
        {
            currentColObject = other.gameObject;
            effector = other.gameObject.GetComponent<Effector>();
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (effector != null)
        {
            effector.Interaction(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentColObject)
        {
            effector.ExitInteraction(rb);
            currentColObject = null;
            effector = null;
        }
    }
}
