using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class MoveControl : MonoBehaviour
{
    [Header("SpeedRange")]
    [Range(0f,100f)]
    public float moveSpeed = 5f;
    [Range(0f, 15000f)]
    public float shootSpeed = 800f;
    [Range(0f, 1f)]
    public float reLoadTime = 0.5f;
    [Header("SetPrefeb")]
    public GameObject bubbleprefeb;
    private Transform thisTransform;
    private Animator headAnimator;
    private Transform body;
    private Animator bodyAni;
    private float moveX;
    private float moveY;
    private float time;
    private bool isAttack = false;
    private bool isInvincible = false;
    // Use this for initialization
    private void Awake()
    {

    }
    private void Start()
    {
        thisTransform = transform;
        headAnimator = GetComponentInChildren<Animator>();
        body = transform.Find("PlayerBody");
        bodyAni = body.GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            headAnimator.Play("Attack_Right");
            isAttack = true;
            if (time > reLoadTime)
            {
                GameObject newGo = Instantiate(bubbleprefeb, thisTransform.position, thisTransform.rotation);
                Bubble bb = newGo.GetComponent<Bubble>();
                bb.shoot(new Vector3(shootSpeed, -(moveY * 800), 0f));
                time = 0f;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            headAnimator.Play("Attack_Left");
            isAttack = true;
            if (time > reLoadTime)
            {
                GameObject newGo = Instantiate(bubbleprefeb, thisTransform.position, thisTransform.rotation);
                Bubble bb = newGo.GetComponent<Bubble>();
                bb.shoot(new Vector3(-shootSpeed, -(moveY* 800), 0f));
                time = 0f;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            headAnimator.Play("Attack_front");
            isAttack = true;
            if (time > reLoadTime)
            {
                GameObject newGo = Instantiate(bubbleprefeb, thisTransform.position, thisTransform.rotation);
                Bubble bb = newGo.GetComponent<Bubble>() ;
                bb.shoot(new Vector3(moveX * 10, -shootSpeed, 0f));
                time = 0f;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            headAnimator.Play("Attack_Up");
            isAttack = true;
            if (time > reLoadTime)
            {
                GameObject newGo = Instantiate(bubbleprefeb, thisTransform.position, thisTransform.rotation);
                Bubble bb = newGo.GetComponent<Bubble>();
                bb.shoot(new Vector3(moveX * 10, shootSpeed, 0f));
                time = 0f;
            }
        }
        moveX = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        moveY = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        thisTransform.Translate(moveX, moveY, 0f);
        if (moveY < 0f)
        {
            if (!isAttack)
                headAnimator.Play("idle");
            bodyAni.Play("Move_front");
        }
        else if(moveX>0f)
        {
            if (!isAttack)
                headAnimator.Play("Move_Right_Head");
            bodyAni.Play("Move_Right");
        }
        else if(moveX<0f)
        {
            if (!isAttack)
                headAnimator.Play("Move_Left_Head");
            bodyAni.Play("Move_Left");
        }
        else if(moveY>0f)
        {
            if (!isAttack)
                headAnimator.Play("Move_Up_Head");
            bodyAni.Play("Move_Up");
        }
        else
        {
            bodyAni.Play("idle");
            if (!isAttack)
                headAnimator.Play("idle");
        }
       
    }
    public void StopAttackEvent()
    {
        isAttack = false;     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isInvincible&&(other.tag == "Enemy"||other.tag == "EnemyAttack"))
        {
            headAnimator.Play("Damage"); //타격 애니
            bodyAni.Play("Damagebody");                       //체력 감소
        }
    }
}
