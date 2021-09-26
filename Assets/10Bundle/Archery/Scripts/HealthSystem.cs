using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the health system of the player and all the enemies, it's responsable for for the damage
/// and switch the player to ragdoll mode
/// </summary>

public class HealthSystem : MonoBehaviour {

    [SerializeField] private int startHealth;
    [SerializeField] private int headDamage;
    [SerializeField] private int bodyDamage;
    [SerializeField] private string objName;

    //this variable is needed only on the player's instance
    [SerializeField] private GameObject ragdoll;

    private float health;

    //This variable will be used on the scripts that check collisions (HeadColl and BodyColl)
    [HideInInspector] public bool died;

    private UIController uiController;

    private void Awake()
    {
        uiController = FindObjectOfType<UIController>();

        health = startHealth;
    }

    private void Start()
    {
        if (gameObject.tag == "Player") ragdoll.SetActive(false);
    }

    private void FixedUpdate()
    {
        PlayerRagdollPos();
    }

    public void OnHeadDamage()
    {
        health -= headDamage;
        if (health <= 0) Die();
    }

    public void OnBodyDamage()
    {
        health -= bodyDamage;
        if (health <= 0) Die();
    }

    void Die()
    {
        if (!died)
        {
            died = true;
            if (gameObject.tag == "enemy")
            {
                //If the enemy die, he will stop of shoot
                gameObject.GetComponentInChildren<EnemyBow>().gameObject.SetActive(false);
                //To indicate that he is dead, his head will become red
                gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
            else
            {
                //If the player die, the Player game object will be set false, and the ragdoll object will be true
                //So when died the player became a ragdoll
                gameObject.SetActive(false);
                ragdoll.SetActive(true);

                uiController.playerDied = true;

            }
        }   
    }

    void PlayerRagdollPos()
    {
        if (gameObject.tag == "Player")
            ragdoll.transform.position = transform.position;
    }
}
