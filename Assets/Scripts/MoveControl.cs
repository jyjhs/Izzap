using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
public class MoveControl : MonoBehaviour
{
    [Header("SpeedRange")]
    [Range(0f,100f)]
    public float moveSpeed = 5f;
    [Range(0f, 15000f)]
    public float shootSpeed = 800f;
    [Range(0f, 1f)]
    public float reLoadTime = 0.5f;
    [Range(0f, 1f)]
    public int maxHealth = 6;
    [Header("SetPrefeb")]
    public GameObject bubbleprefeb;
    private Transform thisTransform;
    private Animator headAnimator;
    private Transform body;
    private Animator bodyAni;
    private float moveX;
    private float moveY;
    private float time;
    public int health;
    private bool isAttack = false;
    private bool isInvincible = false;
    public bool moveStop = false;
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
        health = maxHealth;
    }
    private void Update()
    {
        if (!moveStop)
        {
            time += Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!isInvincible)
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
                if (!isInvincible)
                    headAnimator.Play("Attack_Left");
                isAttack = true;
                if (time > reLoadTime)
                {
                    GameObject newGo = Instantiate(bubbleprefeb, thisTransform.position, thisTransform.rotation);
                    Bubble bb = newGo.GetComponent<Bubble>();
                    bb.shoot(new Vector3(-shootSpeed, -(moveY * 800), 0f));
                    time = 0f;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!isInvincible)
                    headAnimator.Play("Attack_front");
                isAttack = true;
                if (time > reLoadTime)
                {
                    GameObject newGo = Instantiate(bubbleprefeb, thisTransform.position, thisTransform.rotation);
                    Bubble bb = newGo.GetComponent<Bubble>();
                    bb.shoot(new Vector3(moveX * 10, -shootSpeed, 0f));
                    time = 0f;
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!isInvincible)
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
            if (!isInvincible)
            {
                if (moveY < 0f)
                {
                    if (!isAttack)
                        headAnimator.Play("idle");
                    bodyAni.Play("Move_front");
                }
                else if (moveX > 0f)
                {
                    if (!isAttack)
                        headAnimator.Play("Move_Right_Head");
                    bodyAni.Play("Move_Right");
                }
                else if (moveX < 0f)
                {
                    if (!isAttack)
                        headAnimator.Play("Move_Left_Head");
                    bodyAni.Play("Move_Left");
                }
                else if (moveY > 0f)
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
        }
    }
    public void StopAttackEvent()
    {
        isAttack = false;     
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Enemy"||other.tag == "EnermyAttack")&&!isInvincible)
        {

            Debug.Log("trigger");
            headAnimator.Play("Damage");
            bodyAni.Play("DamageBody");
            health--;
            StartCoroutine(OnDamage());
            //headAnimator.Play("Damage"); //타격 애니
            //bodyAni.Play("Damagebody");                       //체력 감소
        }
        if(other.tag=="EnterDoor")
        {
            int x = 1;
            int y = 1;
            if (other.transform.position.x < transform.position.x)
                x *= -1;
            StartCoroutine(CamMove(x));
            //var door = GameObject.FindGameObjectsWithTag("EnterDoor");
            //for(int i=0;i<door.Length;i++)
            //{
            //    var open = door[i].GetComponent<CircleCollider2D>();
            //    open.isTrigger = false;
            //}
        }
    }
    public IEnumerator OnDamage()
    {
        
        isInvincible = true;
        yield return new WaitForSeconds(3f);
        isInvincible = false;
    }
    public IEnumerator CamMove(int x,int y=0)
    {
        moveStop = true;
        Vector3 startVector = Camera.main.transform.position;
        float camMoveY = 0f;
        float camMoveX = 0f;
        if (x != 0)
        {
            camMoveX = (startVector.x + ((Camera.main.orthographicSize * Screen.width / Screen.height)) * 2) * x;
            camMoveY = startVector.y;
            transform.Translate(moveSpeed * x, 0f, 0f);
        }
        if(y!=0)
        {
            camMoveX = startVector.x;
            camMoveY = ((startVector.x + ((Camera.main.orthographicSize * Screen.width / Screen.height)) * 2) * Screen.height / Screen.width)*y;
            transform.Translate(0f, moveSpeed * y, 0f);
        }
        Vector3 newVctor = new Vector3(camMoveX, camMoveY,-10f);
        Camera.main.transform.position = Vector3.Lerp(startVector, newVctor, 0.001f);
        
        yield return null;
        Camera.main.transform.position = newVctor;
        moveStop = false;
    }
}
