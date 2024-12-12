using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private SpriteRenderer r;
    // Start is called before the first frame update
    void Awake()
    {
        r = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WalkedOn()
    {
        r.enabled = true;
    }
}
