using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCam : MonoBehaviour
{
    static private FollowingCam S;

    [Header("Inscribed")]
    public Transform player;  // Reference to the player's car transform
    public Vector3 offset = new Vector3(0, 0, 0);  // Camera offset from the player's car

    [Header("Dynamic")]
    public float camZ;

    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            // Follow the player's position with a constant offset
            Vector3 destination = player.position;

            // Maintain the original Z position of the camera
            destination.z = camZ;

            // Set the camera's position to the calculated destination
            transform.position = destination;
        }
    }
}
