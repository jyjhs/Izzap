using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    private Rigidbody2D rigid;
    private Animator thisAnimator;
    [Range(0f, 1f)]
    public float destroyTime=0.5f;
    // private ParticleSystem particle;
    // Use this for initialization
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        thisAnimator = GetComponentInChildren<Animator>();
    }
    public void shoot(Vector3 force)
    {
        rigid.AddForce(force,ForceMode2D.Force);
        Invoke("PlayeAni", destroyTime);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
    private void PlayeAni()
    {
        rigid.simulated = false;
        thisAnimator.Play("Attack");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            PlayeAni();
        }
    }
}
