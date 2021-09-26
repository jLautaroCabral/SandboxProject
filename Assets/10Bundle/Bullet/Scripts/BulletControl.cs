 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour {

    [Header("Settings")]
    public float velocity;
    public float startBulletHealth = 100;
    public float force;

    [Space(10)]

    [Header("Unity")]
    public Image healthBar;

    public AudioSource
        bodySound,
        barricadeSound,
        sniperSound;

    public Text 
        showScore,
        displayHeadshots,
        displayKills,
        tutorial;

    [HideInInspector]
    public bool isGameOver;

    private float bulletHealth;
    private float rotation;

    Rigidbody2D rg2d;
    private bool isPressed;

    [HideInInspector]
    public int
        headshots,
        kills,
        score;

    BManager manager;

    

    private void Awake()
    {
        manager = FindObjectOfType<BManager>();

        rg2d = GetComponent<Rigidbody2D>();
        rg2d.bodyType = RigidbodyType2D.Static;

        bulletHealth = startBulletHealth;
    }

    private void Update()
    {
        Moviment();
		
        BulletRotation();

        Quit();
    }

    private void FixedUpdate()
    {
        if (rg2d.velocity.x < velocity && !isGameOver)
        {
            rg2d.AddForce(Vector2.right * velocity);
        }
    }

    void Moviment()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;

            if (rg2d.bodyType == RigidbodyType2D.Static)
            {
                rg2d.bodyType = RigidbodyType2D.Dynamic;
                sniperSound.Play();
                tutorial.gameObject.SetActive(false);
            } 
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }

        if (isPressed && !isGameOver)
        {
            rg2d.AddForce(Vector2.up * force, ForceMode2D.Force);
        }
    }
	
	void BulletRotation()
	{
        if (rg2d.velocity.y != 0) rotation = rg2d.velocity.y * 2;
        transform.eulerAngles = new Vector3(0, 0, rotation + 180);
	}
		
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "head" || coll.gameObject.tag == "body")
        {
            kills++;
            displayKills.text = "Kills: " + kills;
            bodySound.Play();
        }

        if (coll.gameObject.tag == "head")
        {
            score += 100;
            headshots++;
            
            displayHeadshots.text = "Headshoots: " + headshots;
            
            showScore.text = score.ToString();
            bulletHealth -= 2;
        }

        if (coll.gameObject.tag == "body")
        {
            score += 50;
            bulletHealth -= 5;
            showScore.text = score.ToString();
            displayHeadshots.text = "Headshoots: " + headshots;
        }

        if (coll.gameObject.tag == "barricade")
        {
            bulletHealth -= 20;
            barricadeSound.Play();
        }

        UpdateBulletHeath();
    }

    void UpdateBulletHeath()
    {
        healthBar.fillAmount = bulletHealth / startBulletHealth;

        if (bulletHealth <= 0 && !isGameOver) GameOver();
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "tank")
        {
            bulletHealth -= 5;
            UpdateBulletHeath();
        }
        
    }

    public void GameOver()
    {
        isGameOver = true;
        rg2d.velocity = Vector2.zero;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        manager.GameOverUI();
    }

    void Quit()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }
}
