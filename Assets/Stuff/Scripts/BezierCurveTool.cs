using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveTool : MonoBehaviour 
{
    public Transform BezierStart, BezierMiddle, BezierEnd, BezierCurve;
    public float Speed;
    [HideInInspector]
    public Vector3 _bezierCurveOutput;
    float _time;
    bool backwards;
    

    private void Update()
    {
        if (backwards)
        {
            if (_time <= 0) {
                _time = 0;
                backwards = false;
            }
            _time -= Time.deltaTime * Speed;
        }
        else
        {
            _time += Time.deltaTime * Speed;
            if (_time >= 1)
            {
                _time = 1;
                backwards = true;
            }

        }
        QuadraticBezier.GetCurve(out _bezierCurveOutput, BezierStart.position, BezierMiddle.position, BezierEnd.position, _time);
        // Add the BezierChain instead?
        BezierCurve.transform.position = _bezierCurveOutput;
        
    }
}
