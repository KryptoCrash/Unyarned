using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour {
    // Stats
    private StatsManager stats;

    // Particle manager
    private PlayerParticleManager particleManager;

    // Game Manager
    public GameObject gameManager;
    private GMScript gameManagerScript;

    // Keys
    public int greenKeyCount;
    public int redKeyCount;

    // Sounds
    public AudioSource useKey;
    public AudioSource getKey;

    // Start is called before the first frame update
    void Start() {
        stats = GetComponent<StatsManager>();

        particleManager = GetComponent<PlayerParticleManager>();

        gameManagerScript = gameManager.GetComponent<GMScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Getting a key increases key count
        // Using keys to open doors decreases key count
        // Keys can only unlock doors of the same color
        if (collision.gameObject.tag == "GreenKey") {
            greenKeyCount++;
            particleManager.GetGreenKey(collision.transform.position);
            getKey.Play();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "GreenDoor") {
            if (greenKeyCount > 0) {
                greenKeyCount--;
                particleManager.UseGreenKey(collision.transform.position);
                useKey.Play();
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "RedKey") {
            redKeyCount++;
            particleManager.GetRedKey(collision.transform.position);
            getKey.Play();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "RedDoor") {
            if (redKeyCount > 0) {
                redKeyCount--;
                particleManager.UseRedKey(collision.transform.position);
                useKey.Play();
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "BossStart") {
            gameManagerScript.StartBoss();
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Flag") {
            gameManagerScript.GameWon();
        }
    }
}
