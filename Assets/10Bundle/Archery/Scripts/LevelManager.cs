using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [SerializeField] private int nextLevelIndex;

    void LoadNewLevel()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
            LoadNewLevel();
    }
}
