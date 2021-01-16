using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScript : MonoBehaviour {
    // Sprite Renderer
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void FlipX() {
        sprite.flipY = !sprite.flipY;
    }

    public void FlipY() {
        sprite.flipY = !sprite.flipY;
    }
}
