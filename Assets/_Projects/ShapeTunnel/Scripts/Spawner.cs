using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ShapeTunnel {
  public class Spawner : MonoBehaviour {
    public GameObject[] obstacles;
    public GameObject token;
    public float timeBetweenSpawns, timeReduce, minTimeBetweenSpawns;
    public int tokenSpawnFrequency = 5; //The lower the frequency is, the most likely to be spawned

    private void Start() => Spawn(); //First spawn

    public void Spawn() {
      Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position,
        Quaternion.identity); //Spawns obstacle to the spawner's position with the same rotation

      if (!(FindObjectOfType<Collision>().gameIsOver)) //Invokes the next spawn only if the game is not over
      {
        Invoke("Spawn", timeBetweenSpawns); //Next spawn after 'timeBetweenSpawns' secs
        if (Random.Range(0, tokenSpawnFrequency) == 0) //If it is time to spawn a token
          Invoke("SpawnToken", timeBetweenSpawns / 2f); //Then calls the function to spawn token
      }

      if ((timeBetweenSpawns - timeReduce) >= minTimeBetweenSpawns)
        timeBetweenSpawns -= timeReduce; //reduces the timeBetweenSpawns after every spawn
    }

    public void SpawnToken() {
      Instantiate(token, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))); 
      //Spawns token to the spawner's position with same rotation
    }
  }
}