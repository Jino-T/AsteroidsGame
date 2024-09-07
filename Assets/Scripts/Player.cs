using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //header adds text and separation between variables in the inspector
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object refernces")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private ParticleSystem thrusterParticles;

    private Rigidbody2D shipRigidbody;

    //HandleShipAcceleratioin and HandleShipRotation only occur when player is alive
    private bool isAlive = true;
    private bool isAccelerating = false;
    private bool devMode = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get reference to Rigidbody 2d
        shipRigidbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {
            HandleShipAcceleration();
            HandleShipRotation();
            HandleShooting();
            HandleDevMode();
        }
        
    }

    private void FixedUpdate() {
        if ( isAlive && isAccelerating) {
            // Increase velocity but cap out at maximum
            shipRigidbody.AddForce(shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);

            //thruster particle system
            Instantiate(thrusterParticles, transform.position, Quaternion.identity);
        }
    }

    private void HandleShipAcceleration() {
        // Are we accelerating?
            isAccelerating = Input.GetKey(KeyCode.UpArrow);
    }

    private void HandleDevMode() {
        // Are we accelerating?
        if (Input.GetKey(KeyCode.Alpha7)) {
            Debug.Log("devMode");
            devMode = true;
        } else if (Input.GetKey(KeyCode.Alpha8)) {
            Debug.Log("Out of devMode");
            devMode = false;
        }
    }

    private void HandleShipRotation() {
        // Ship roatation
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Debug.Log("left pressed");
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            Debug.Log("right pressed");
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
        }
    }

    private void HandleShooting() {
        if (Input.GetKeyDown(KeyCode.Space)) {

            Debug.Log("space pressed");

            // instatiate takes rigidbody position and rotation
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // inherit velocity from only the forward velocity of the ship
            Vector2 shipVelocity = shipRigidbody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);

            // Don't want to inherit opposite direction velocity bc it may cause bullets to be stationary
            if (shipForwardSpeed < 0) {
                shipForwardSpeed = 0;
            }

            bullet.velocity = shipDirection * shipForwardSpeed;

            // propel bullet in direction player is facing
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Asteroid") && !devMode) {
            isAlive = false;

            GameManager gameManager = FindAnyObjectByType<GameManager>();

            gameManager.GameOver();

            //particle system
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}