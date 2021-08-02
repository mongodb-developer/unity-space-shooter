using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float movementSpeed = 5.0f;

    private Animator _animator;

    void Start() {
        _animator = GetComponent<Animator>();
    }

    void OnEnable() {
        float randomPositionY = Random.Range(-4.0f, 4.0f);
        transform.position = new Vector3(10.0f, randomPositionY, 0);
    }

    void Update() {
        transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        if(transform.position.x < -10.0f) {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Weapon") {
            _animator.SetBool("doExplode", true);
        }
    }

    void OnExplosionAnimationFinished() {
        _animator.SetBool("doExplode", false);
        gameObject.SetActive(false);
        RealmController.Instance.IncreaseScore();
    }

}
