using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed = 5.0f;
    public float respawnSpeed = 8.0f;
    public float weaponFireRate = 0.5f;

    private Animator _animator;
    private float nextBlasterTime = 0.0f;
    private bool isRespawn = true;

    void Start() {
        _animator = GetComponent<Animator>();
    }

    void Update() {
        if(isRespawn == true) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-6.0f, -0.25f), respawnSpeed * Time.deltaTime);
            if(transform.position == new Vector3(-6.0f, -0.25f, 0.0f)) {
                isRespawn = false;
            }
        } else {
            if(_animator.GetBool("doExplode") == false) {
                if(Input.GetKey(KeyCode.UpArrow) && transform.position.y < 4.0f) {
                    _animator.SetInteger("doFly", 1);
                    transform.position += Vector3.up * movementSpeed * Time.deltaTime;
                } else if(Input.GetKey(KeyCode.DownArrow) && transform.position.y > -4.0f) {
                    _animator.SetInteger("doFly", -1);
                    transform.position += Vector3.down * movementSpeed * Time.deltaTime;
                } else {
                    _animator.SetInteger("doFly", 0);
                }
                if(Input.GetKey(KeyCode.Space) && Time.time > nextBlasterTime) {
                    nextBlasterTime = Time.time + weaponFireRate;
                    GameObject blaster = ObjectPool.SharedInstance.GetPooledBlaster();
                    if(blaster != null) {
                        blaster.SetActive(true);
                        blaster.transform.position = new Vector3(transform.position.x + 1, transform.position.y);
                    }
                }
                if(RealmController.Instance.IsCrossBlasterEnabled()) {
                    if(Input.GetKey(KeyCode.B) && Time.time > nextBlasterTime) {
                        nextBlasterTime = Time.time + weaponFireRate;
                        GameObject crossBlast = ObjectPool.SharedInstance.GetPooledCrossBlast();
                        if(crossBlast != null) {
                            crossBlast.SetActive(true);
                            crossBlast.transform.position = new Vector3(transform.position.x + 1, transform.position.y);
                        }
                    }
                }
                if(RealmController.Instance.IsSparkBlasterEnabled()) {
                    if(Input.GetKey(KeyCode.V) && Time.time > nextBlasterTime) {
                        nextBlasterTime = Time.time + weaponFireRate;
                        GameObject sparkBlast = ObjectPool.SharedInstance.GetPooledSparkBlast();
                        if(sparkBlast != null) {
                            sparkBlast.SetActive(true);
                            sparkBlast.transform.position = new Vector3(transform.position.x + 1, transform.position.y);
                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Enemy" && isRespawn == false) {
            _animator.SetBool("doExplode", true);
            RealmController.Instance.ResetScore();
        }
    }

    void OnExplosionAnimationFinished() {
        _animator.SetBool("doExplode", false);
        transform.position = new Vector3(-10.0f, -0.25f, 0.0f);
        isRespawn = true;
    }

}
