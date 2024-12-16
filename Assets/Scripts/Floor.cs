using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    bool steppedOn = false;
    private SpriteRenderer r;
    void Awake()
    {
        r = GetComponentInChildren<SpriteRenderer>();
    }

    public bool HasBeenWalkedOn()
    {
        return steppedOn;
    }

    public void WalkedOn()
    {
        steppedOn = true;
        r.enabled = true;
    }
}
