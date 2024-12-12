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

        Vector2 startPos = new Vector2(0, 0);
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {

                Instantiate(Floor, new Vector3(j, 0, i), Quaternion.identity);
                chars[i, j] = lines[i][j];
                if (chars[i, j] == '#') {
                    Instantiate(Wall, new Vector3(j, 0, i), Quaternion.identity);
                }
                else if(spawnedGuards < NumberOfGuards)
                {
                    if (UnityEngine.Random.Range(0, 100) + attempts == 100)
                    {
                        Instantiate(Guard, new Vector3(j, 0, i), Quaternion.identity);

                    }
                    else
                    {
                        attempts++;
                    }
                }
                if (chars[i, j] == '^') {
                    GameObject guardObject = Instantiate(Guard, new Vector3(i, 0 , j), Quaternion.identity);
                    guard = guardObject.GetComponent<Guard>();
                    startPos.x = i;
                    startPos.y = j;
                    // Console.WriteLine(startPos.Y + ", "+startPos.X);
                }
            }
        }
        guard.Move();
    }
}
