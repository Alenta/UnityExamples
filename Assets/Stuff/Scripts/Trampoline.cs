using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CenterMode { RigidBody, Transform, Zeroed }
public class Trampoline : Effector
{
    public float _BounceStrength = 5f;
    public bool _KeepWithinBoundaries = false;
    public CenterMode _CenterMode;
    public bool HeightAndWeightCalculation;
    public Transform _BounceOrigin;
    public bool UseTriggerTime;
    public float RetriggerTime;
    bool triggerReady = true;
    
    public override void Interaction(Rigidbody rbToAffect)
    {
        if (UseTriggerTime) 
        {
            if (!triggerReady)
            {
                StartCoroutine(Retrigger());
                return;
            }
            else triggerReady = false;
        }
        switch (_CenterMode)
        {
            case CenterMode.RigidBody:
                Vector3 dir = transform.position - rbToAffect.transform.position;
                rbToAffect.velocity = new Vector3(dir.x,0,dir.z) + (transform.up * (_BounceStrength));
                break;
            case CenterMode.Transform:
                rbToAffect.transform.position = _BounceOrigin.position;
                rbToAffect.velocity = (transform.up * (_BounceStrength));
                break;
            case CenterMode.Zeroed:
                rbToAffect.velocity = (transform.up * (_BounceStrength));
                break; 
        }
    }

    IEnumerator Retrigger()
    {
        yield return new WaitForSeconds(RetriggerTime);
        triggerReady = true;
    }
}
