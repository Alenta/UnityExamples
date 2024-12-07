using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MazeMaker : MonoBehaviour
{
    public GameObject Floor;
    public GameObject Wall;
    public GameObject Guard;
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
                if (chars[i, j] == '^') {
                    // 2D-Array weirdness, j goes in x, i goes in y.
                    Instantiate(Guard, new Vector3(j, 0 , i), Quaternion.identity);
                    startPos.x = j;
                    startPos.y = i;
                    // Console.WriteLine(startPos.Y + ", "+startPos.X);
                }
            }
        }
    }
}
