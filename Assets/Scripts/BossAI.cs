using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image hpBar;
    public int hp;
    public int dmg;
    public int maxHp;
    public GameObject hpWindow;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        hp = 200;
        maxHp = hp;
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

    public void DecreaseHp()
    {
        if (hp > 0)
        {
            hp -= dmg;
            hpBar.fillAmount = (float)hp / maxHp;
            StartCoroutine(Alpha());
        }
        else
        {
            var anim_ = GetComponentInChildren<AnimEvent>();
            hpWindow.SetActive(false);
            anim_.anim.Play("Die");
        }
    }
    
    public IEnumerator Alpha()
    {
        hpBar.color = new Color(hpBar.color.r, hpBar.color.g, hpBar.color.b, 0f);
        yield return new WaitForSeconds(0.1f);
        hpBar.color = new Color(hpBar.color.r, hpBar.color.g, hpBar.color.b, 1f);
    }
}
