using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowControllerScript : MonoBehaviour {
    // Imported GameObjects
    public GameObject player;

    // Animator
    private Animator playerAnim;
    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");

        playerAnim = player.GetComponent<Animator>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // Set shadow position to inverted player position
        transform.position = Vector3.Scale(player.transform.position, new Vector3(1, -1, 1));
        transform.localScale = new Vector3(player.transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
