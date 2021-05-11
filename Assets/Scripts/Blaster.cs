using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour {

    public float movementSpeed = 5.0f;
    public float decayRate = 2.0f;

    private float timeToDecay;

    void OnEnable() {
        timeToDecay = decayRate;
    }

    void Update() {
        timeToDecay -= Time.deltaTime;
        transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        if(transform.position.x > 10.0f || timeToDecay <= 0) {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Enemy") {
            gameObject.SetActive(false);
        }
    }

}
