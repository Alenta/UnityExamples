using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{
    public virtual void Interaction(Rigidbody rbToAffect) { }

    public virtual void ExitInteraction(Rigidbody rbToAffect) { }
}
