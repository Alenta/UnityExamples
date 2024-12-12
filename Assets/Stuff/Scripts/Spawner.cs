using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnMode { Ordered, Same, Random }
public class Spawner : MonoBehaviour
{
    public List<GameObject> ObjectsToSpawn;
    public Transform SpawnPoint;
    public SpawnMode _SpawnMode;
    public float SpawnRate = 10f;
    public bool SpawnFirstImmediately;
    public bool AutoStart;
    public bool Recycle;
    public int PoolSize;
    public int SpawnAmount = 0;
    bool spawning = false;
    List<GameObject> poolObjects = new List<GameObject>();
    int poolIndex = 0;
    int spawnIndex = 0;
    // Starts off at -1 so that case Spawnmode.Same only sets it once, and not on every loop.
    int selector = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (SpawnPoint == null) SpawnPoint = this.transform;
        
        // Populate pool
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject newObject = null;
            switch (_SpawnMode)
            {
                case SpawnMode.Ordered:
                    newObject = Instantiate(ObjectsToSpawn[poolIndex], this.transform.position, Quaternion.identity);
                    break;
                case SpawnMode.Same:
                    if (selector == -1) selector = Random.Range(0, ObjectsToSpawn.Count);
                    newObject = Instantiate(ObjectsToSpawn[poolIndex], this.transform.position, Quaternion.identity);
                    break;
                case SpawnMode.Random:
                    newObject = Instantiate(ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Count)], this.transform.position, Quaternion.identity);
                    break;
            }
            if (newObject == null) { print("This is bad"); break; }
            newObject.transform.parent = this.transform;
            newObject.SetActive(false);
            poolObjects.Add(newObject);
            poolIndex++;
            if (poolIndex >= ObjectsToSpawn.Count) poolIndex = 0;
        }
        if (AutoStart) StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        spawning = true;
        bool isFirstSpawn = true;

        while (spawning)
        {
            // If SpawnAmount is 0 it has not been set, so we spawn forever.
            if (SpawnAmount != 0 && SpawnAmount <= spawnIndex) { spawning = false; break; }
            if (isFirstSpawn && !SpawnFirstImmediately)
            {
                // Skip spawning on the first iteration if SpawnFirstImmediately is false
                isFirstSpawn = false;
            }
            else
            {
                // Reset spawn index if it exceeds the pool size
                if (spawnIndex >= poolObjects.Count) { 
                    if(Recycle) spawnIndex = 0;
                    else { spawning = false; break; };
                }
                poolObjects[spawnIndex].transform.position = SpawnPoint.position;
                // Activate the current object in the pool
                poolObjects[spawnIndex].SetActive(true);

                // Increase index to target next pool object
                spawnIndex++;

                isFirstSpawn = false;
            }

            yield return new WaitForSeconds(SpawnRate);
        }
    }
}
    /*
      IEnumerator Spawn()
    {
        spawning = true;
        while (spawning) 
        {
            // First is true first time this While-loop runs.
            // If SpawnFirst is true, the first round will spawn an object
            // If not, first round does nothing except turning off the first-flag

            if (!isFirstSpawn)
            {
                if(spawnIndex >= poolObjects.Count) spawnIndex = 0;
                poolObjects[spawnIndex].SetActive(true);

                spawnIndex++;
            }
            else if (SpawnFirstImmediately)
            {
                poolObjects[spawnIndex].SetActive(true);
                spawnIndex++;
            }
            isFirstSpawn = false;
            yield return new WaitForSeconds(SpawnRate);
        }

    }
    */


