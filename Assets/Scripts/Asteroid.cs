using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] private ParticleSystem destroyedParticles;
    // public bc we will use it when we instantiate asteroids later
    public int size = 3;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // set size
        transform.localScale = 0.5f * size * Vector3.one;

        // add movement, bigger asteroids should be slower
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); 
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);

        // Register creation
        gameManager.asteroidCount++;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) {
            gameManager.asteroidCount--;

            // get rid of bullet
            Destroy(collision.gameObject);

            if (size > 1) {
                for (int i = 0; i < 2; i++) {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                    newAsteroid.size = size - 1;
                    newAsteroid.gameManager = gameManager;
                }
            }

            // particle system
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
