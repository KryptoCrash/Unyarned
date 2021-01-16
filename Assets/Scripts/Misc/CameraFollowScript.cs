using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {
    // Player GameObject
    public GameObject player;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // Make camera follow player's x position
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
}
