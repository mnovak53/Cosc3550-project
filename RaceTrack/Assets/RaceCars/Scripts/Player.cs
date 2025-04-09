using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool _thrusting;
    private float _turnDirection;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody2D;
    public float respawnShieldTime = 3.0f;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("NoCollision");
        this.Invoke(nameof(TurnOnCollision), respawnShieldTime);
    }

    private void TurnOnCollision()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        //Turn Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
        }
        //Turn Right
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _turnDirection = -1.0f;
        }
        else
        {
            _turnDirection = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Drift();
        }
    }

    private void Drift()
    {

    }

    private void FixedUpdate()
    {
        if (_thrusting)
        {
            _rigidbody2D.AddForce(this.transform.right * this.thrustSpeed);
        }
        if (_turnDirection != 0.0f)
        {
            _rigidbody2D.AddTorque(_turnDirection * this.turnSpeed);
        }

    }



}
