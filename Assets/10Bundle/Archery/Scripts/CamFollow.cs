using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamFollow : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ground;

    //xoff: If 0 the character will be on center of screen, less than 0 next to the left corner, more
    // than 0 next to right corner, player around in the inspector to see how this works
    [SerializeField] private float xoff = -15;

    //yoff is same idea of xoff but in the y axis
    //yoffHigh is same off yoff but only apple if the character is too far from the ground
    [SerializeField] private float yoff, yoffHigh;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private float maxDistanceFromGround, minDistanceFromGround;

    Vector3 playerPos;

    private float xPos; //x position of the camera
    private float ypos; //y position of the camera

    private Vector3 nextPos;
    private Vector3 newPos;

    private void Start()
    {
        playerPos = player.transform.position;

        xPos = playerPos.x + xoff;
        ypos = playerPos.y - yoff;

        if (Application.isMobilePlatform) UpdateYoffOnMoble();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        FollowOnYAxis();
    }

    private void UpdateYoffOnMoble()
    {
        yoff += 8;
    }

    private void FollowPlayer()
    {
        playerPos = player.transform.position;
        nextPos = new Vector3(playerPos.x + xoff, ypos, -10);
        newPos = Vector3.Lerp(transform.position, nextPos, cameraSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    /// <summary>
    /// If The player be too far of the ground the camera will follow him in the y axis, otherwise 
    /// the camera will follow only in x axis
    /// </summary>
    private void FollowOnYAxis()
    {
        float distanceFromGround = player.transform.position.y - ground.transform.position.y;
 
        if (distanceFromGround > maxDistanceFromGround)
        {
            ypos = playerPos.y - yoffHigh;
        }

        if (distanceFromGround < minDistanceFromGround)
        {
            ypos = playerPos.y - yoff;
        }
    }
}
                                                                                                                                                                                                                                                      