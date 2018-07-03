using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttack
{
    AttackBullet = 0,
    HighJump,
    MoveAttack,
    Count,
}

public class BossAI : MonoBehaviour
{
    public GameObject target;

    public float speed;
    public GameObject sprite;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    public void AttackBullet()
    {
        Debug.Log("AttackBullet");
    }

    public void HighJump()
    {
        Debug.Log("HighJump");
    }

    public void MoveAttack()
    {
        Debug.Log("MoveAttack");
    }
}
