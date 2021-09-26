using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    [SerializeField] private Canvas inGameCanvas_PC;
    [SerializeField] private Canvas inGameCanvas_Mobile;
    [SerializeField] private Canvas gameOverCanvas;

    [SerializeField] private Text displayNumArrows_PC;
    [SerializeField] private Text displayNumArrows_Mobile;

    //THis variable will be be modified on the HeathSystem script
    [HideInInspector] public bool playerDied;

    private BowPlayer bow;

    private void Start()
    {
        bow = FindObjectOfType<BowPlayer>();

        initializeTheRightCanvas();
    }

    private void Update()
    {
        if (playerDied) StartCoroutine(GameOver());

        DisplayNumOfArrows();

        Exit();
    }

    private void initializeTheRightCanvas()
    {
        if (!Application.isMobilePlatform)
        {
            inGameCanvas_PC.gameObject.SetActive(true);
            inGameCanvas_Mobile.gameObject.SetActive(false);
        }
        else
        {
            inGameCanvas_PC.gameObject.SetActive(false);
            inGameCanvas_Mobile.gameObject.SetActive(true);
        }

        gameOverCanvas.gameObject.SetActive(false);
    }

    //Make the game over canvas appear
    IEnumerator GameOver()
    {
        playerDied = false;
        yield return new WaitForSeconds(1);
        gameOverCanvas.gameObject.SetActive(true);
    }

    private void DisplayNumOfArrows()
    {
        if (Application.isMobilePlatform)
        {
            displayNumArrows_Mobile.text = "Arrows: " + bow.numberOfArrows;
        }

        else
        {
            displayNumArrows_PC.text = "Arrows: " + bow.numberOfArrows;
        }
    }

    private void Exit()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    public void Restart()
    {
        Debug.Log("Restarts");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
