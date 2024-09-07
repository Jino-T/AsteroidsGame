using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab1;
    [SerializeField] private Asteroid asteroidPrefab2;

    public int asteroidCount = 0;

    // Asteroid spawning is based on level
    // 1 = 4, 2 = 6, 3 = 8, 4 = 10 ...
    public int level = 0;

    // Update is called once per frame
    void Update()
    {
        // spawn more asteroids if none are left
        if (asteroidCount == 0) {
            level ++;
            int numAsteroids = 2 + (2*level);

            for (int i = 0; i < numAsteroids; i++) {
                SpawnAsteroid();
            }
        }
    }

    private void SpawnAsteroid() {
        // How far along the edge
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;

        // Which edge
        int edge = Random.Range(0,4);

        if (edge == 0) {
            viewportSpawnPosition = new Vector2(offset, 0);
        } else if (edge == 1) {
            viewportSpawnPosition = new Vector2(offset, 1);
        } else if (edge == 2) {
            viewportSpawnPosition = new Vector2(0, offset);
        } else if (edge == 3) {
            viewportSpawnPosition = new Vector2(1, offset);
        }

        // Create the asteroid
        Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);

        // vary asteroids randomly
        int astColor = Random.Range(0,2);
        Debug.Log(astColor);
        if (astColor == 1) {
            Asteroid asteroid = Instantiate(asteroidPrefab1, worldSpawnPosition, Quaternion.identity);
            asteroid.gameManager = this;
        } else {
            Asteroid asteroid = Instantiate(asteroidPrefab2, worldSpawnPosition, Quaternion.identity);
            asteroid.gameManager = this;
        }

    }

    public void GameOver() {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart() {
        Debug.Log("Game Over");

        // wait before restarting
        yield return new WaitForSeconds(2f);

        //Restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }
}
