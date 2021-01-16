using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {
    // Sounds
    public AudioSource dashSound;
    public AudioSource teleportSound;
    public AudioSource shootSound;
    public AudioSource attackSound;
    public AudioSource jumpSound;
    public AudioSource hitGroundSound;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Dash() {
        dashSound.Play();
    }
    public void Warp() {
        teleportSound.Play();
    }
    public void Shoot() {
        shootSound.Play();
    }

    public void Attack() {
        attackSound.Play();
    }

    public void Jump() {
        jumpSound.Play();
    }

    public void Ground() {
        hitGroundSound.Play();
    }
}
