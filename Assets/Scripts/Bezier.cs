using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bezier : MonoBehaviour
{
    [SerializeField] protected GameObject Cube;
    [Range(0f, 1f)]
    [SerializeField] protected float mTime;
    [SerializeField] protected bool AutoTime;
    [Range(0.1f, 10f)]
    [SerializeField] protected float timeScale;
    [SerializeField] protected List<GameObject> Checkpoints = new List<GameObject>();

    protected Vector3 myPosition;
    protected bool pointsSet;
    public abstract void GetBezierFromCheckpoints(out Vector3 pos, List<GameObject> Checkpoints, float time);

 
    public IEnumerator TimeCount()
    {
        while (pointsSet)
        {
            if (AutoTime)
            {
                mTime += Time.deltaTime * timeScale;

                if (timeScale <= 0f)
                {
                    timeScale = 0.1f;
                }

                if (mTime >= 1f)
                {
                    mTime = 0f;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

