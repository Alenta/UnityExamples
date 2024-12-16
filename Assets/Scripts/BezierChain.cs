using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BezierChain : Bezier
{
    
    int _nCheckpoints;
    int _nCurves;
    float subTotal = 0f;
    public LayerMask groundLayer;
    public GameObject pointCube;
    public List<Material> materials;

    //List<GameObject> points = new List<GameObject>();
    
    Vector3 pos;
    GameObject[] curvePoints = new GameObject[3];

    private void Update()
    {
        if (!pointsSet)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                CreatePoint();
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && Checkpoints.Count >= 3) 
            { 
                pointsSet = true;
                StartCoroutine(TimeCount());
            }
        }

        if (pointsSet)
        {
            GetBezierFromCheckpoints(out myPosition, Checkpoints, mTime);
        }
        Cube.transform.position = myPosition;
    }

    public override void GetBezierFromCheckpoints(out Vector3 pos, List<GameObject> Checkpoints, float time)
    {
        _nCheckpoints = Checkpoints.Count;
        _nCurves = _nCheckpoints / (3 - 1);

        if (subTotal == 0f)
        {
            subTotal = 1f / _nCurves;
        }

        float localPercentage = GetLocalPercentage(time);
        int localStartPoint = GetLocalStart(time);

        int index0 = localStartPoint;
        int index1 = localStartPoint + 1;
        int index2 = localStartPoint + 2;

        if (index0 < _nCheckpoints - 2)
        {
            Vector3 p0 = Checkpoints[index0].transform.position;
            Vector3 p1 = Checkpoints[index1].transform.position;
            Vector3 p2 = Checkpoints[index2].transform.position;
            QuadraticBezier.GetCurve(out pos, p0, p1, p2, localPercentage);
        }
        else
        {
            Vector3 start = Checkpoints[index0].transform.position;
            Vector3 end = Checkpoints[_nCheckpoints - 1].transform.position;

            pos = Vector3.Lerp(start, end, localPercentage);
        }
    }

    float GetLocalPercentage(float time)
    {
        float remainder = time % subTotal; // Computes the remainder after dividing its left operand by its right operand
        float percentage = remainder / subTotal;
        return percentage;
    }

    int GetLocalStart(float time)
    {
        // how many subtotals can fit inside passed time?
        int n = (int)(time / subTotal);

        // convert to list index
        int index = n * (3 - 1);

        return index;
    }


    void CreatePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            pos = hit.point;
            GameObject newCube = Instantiate(pointCube, pos, Quaternion.identity);
            newCube.GetComponent<MeshRenderer>().material = materials[UnityEngine.Random.Range(0, materials.Count - 1)];
            Checkpoints.Add(newCube);
        }
    }
    void DrawSegment(List<GameObject> points)
    {

        Debug.DrawLine(points[0].transform.position, points[1].transform.position);
    }
    void DrawLine()
    {

    }

}

