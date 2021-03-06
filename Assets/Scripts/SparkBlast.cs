using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkBlast : MonoBehaviour {

    public float movementSpeed = 5.0f;

    void Update() {
        transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        if(transform.position.x > 10.0f) {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Enemy") {
            gameObject.SetActive(false);
        }
    }

}
