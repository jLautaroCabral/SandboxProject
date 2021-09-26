using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject enemyPrefab, player;
    public Vector2 range;
    public float spawnRate, minSpawnDistance;

    float time;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > spawnRate)
        {
            time = 0;
            ReleaseEnemy();
        }
    }

    void ReleaseEnemy()
    {
        float x = Random.Range(-range.x, range.x);
        float y = Random.Range(-range.y, range.y);

        Vector2 pos = new Vector2(x, y);

        GameObject enemy = Instantiate(enemyPrefab, pos.normalized * minSpawnDistance, Quaternion.identity);
        enemy.SetActive(true); 
    }

    void TestDistance() 
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(pos, player.transform.position);
        Debug.Log(distance);
    }
}
