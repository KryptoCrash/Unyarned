using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnTriggerScript : MonoBehaviour {
    // Darker area
    private AudioSource darkerArea;

    // Start is called before the first frame update
    void Start() {
        darkerArea = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            darkerArea.Play();
        }
    }
}
