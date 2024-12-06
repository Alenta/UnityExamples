using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramGlitcher : MonoBehaviour
{
    public float glitchChance = .1f;
    public float rotationSpeed = 1f;
    private Renderer holoRenderer;
    private WaitForSeconds glitchLoopWait = new WaitForSeconds(.1f);
    private WaitForSeconds glitchDuration = new WaitForSeconds(.1f);

    private void Awake()
    {
        holoRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }

    IEnumerator Start()
    {
        while (true)
        {
            float glitchTest = Random.Range(0f, 1f);
            if (glitchTest <= glitchChance) {
                StartCoroutine(Glitch());
            }
            yield return glitchLoopWait;
        }    
    }

    IEnumerator Glitch()
    {
        glitchDuration = new WaitForSeconds(Random.Range(.05f, .25f));
        holoRenderer.material.SetFloat("_Amount", 1f);
        holoRenderer.material.SetFloat("_CutoutThresh", .29f);
        holoRenderer.material.SetFloat("_Speed", Random.Range(1, 10));
        yield return glitchDuration;
        holoRenderer.material.SetFloat("_Amount", 0f);
        holoRenderer.material.SetFloat("_CutoutThresh", 0f);
    }
}
