using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MazeMaker : MonoBehaviour
{
    public int NumberOfGuards = 1;
    public GameObject Floor;
    public GameObject Wall;
    public GameObject Guard;
    public GameObject Camera;
    public Transform EnvironmentParent;
    Guard guard;
    int spawnedGuards;
    int attempts;

    void Start() {
        string fileName = "Maze.txt";
        string filePath = Path.Combine(Environment.CurrentDirectory, @"Assets\Text", fileName);
        string[] lines = File.ReadAllLines(filePath);
        int rows = lines[0].Length;
        int cols = lines.Length;
        char[,] chars = new char[cols, rows];
        List<GameObject> guards = new List<GameObject>();

        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {

                Instantiate(Floor, new Vector3(j, 0, i), Quaternion.identity, EnvironmentParent);
                chars[i, j] = lines[i][j];
                if (chars[i, j] == '#') {
                    Instantiate(Wall, new Vector3(j, 0, i), Quaternion.identity, EnvironmentParent);
                }
                else if(spawnedGuards < NumberOfGuards)
                {
                    if (UnityEngine.Random.Range(0, 100) + attempts == 100)
                    {
                        GameObject guard = Instantiate(Guard, new Vector3(j, 0, i), Quaternion.identity);
                        guards.Add(guard);
                        print("made an extra guard, total guards are now " + spawnedGuards);
                    }
                    else
                    {
                        attempts++;
                    }
                }
                if (chars[i, j] == '^') {
                    GameObject guardObject = Instantiate(Guard, new Vector3(i, 0 , j), Quaternion.identity);
                    guards.Add(guardObject);
                }
            }
        }
        
        foreach (var guard in guards)
        {
            
            guard.GetComponent<Guard>().Move();
        }
        print(guards.Count);
    }
}
