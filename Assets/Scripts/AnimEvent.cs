using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public AnimationClip[] anims;
    public Animator anim;
    public BossAI bossCtrl;

    public GameObject bullet;
    public GameObject player;
    public GameObject boss;
    public Transform bulletSpawn;
    public RectTransform rect;
    public GameObject shadow;
    public SpriteRenderer sprite;

    private void Start()
    {
        var rnd = Random.Range(0, 3);

        anim.Play(anims[rnd].name);
        AttackState(rnd);
    }

    public void ChooseAttack()
    {
        var playerPos = bossCtrl.target.transform.position;
        var startPos = bulletSpawn.transform.position;

        var dist = Vector3.Distance(playerPos, startPos);

        var rnd = 0;
        if (dist > 120f)
        {
            rnd = Random.Range(1, 3);
        }
        else
        {
            rnd = Random.Range(0, 2);
        }

        anim.Play(anims[rnd].name);

        AttackState(rnd);
    }

    private void AttackState(int rnd)
    {
        switch (rnd)
        {
            case 0:
                bossCtrl.AttackBullet();
                break;
            case 1:
                bossCtrl.HighJump();
                break;
            case 2:
                bossCtrl.MoveAttack();
                break;
        }
    }

    private void BulletControl()
    {
        var rnd = Random.Range(14, 18);
        GameObject[] goes = new GameObject[rnd];

        for (var i = 0; i < rnd; ++i)
        {
            var playerPos = bossCtrl.target.transform.position;
            var startPos = bulletSpawn.transform.position;

            var dist = Vector3.Distance(playerPos, startPos);

            var newX = (dist > 120f) ? (playerPos.x - startPos.x) * 0.8f : playerPos.x;
            var newY = (dist > 120f) ? (playerPos.y - startPos.y) * 0.8f : playerPos.y;

            var ranX = Random.Range(-60f, 60f);
            var ranY = Random.Range(-60f, 60f);

            var dest = new Vector3(newX + ranX, newY + ranY);

            goes[i] = Instantiate(bullet);
            goes[i].transform.position = bulletSpawn.position;

            var ctrl = goes[i].GetComponent<BulletCtrl>();
            ctrl.startPos = startPos;
            ctrl.endPos = dest;

            var layerNum = goes[i].GetComponentInChildren<SpriteRenderer>();
            layerNum.sortingOrder = i;
        }
    }

    public void DirectionCheck()
    {
        if (player.transform.position.x - bulletSpawn.transform.position.x > 0)
        {
            rect.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else
        {
            rect.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    public void MoveUp()
    {
        StartCoroutine(Up());
    }

    public IEnumerator Up()
    {
        float timeTemp = 0f;

        while (timeTemp < 0.6f)
        {
            shadow.transform.localScale = Vector3.Lerp(shadow.transform.localScale, new Vector3(0.7f, 0.7f, 1f), Time.deltaTime);
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, bossCtrl.target.transform.position, Time.deltaTime * 40f);

            yield return null;
            timeTemp += Time.deltaTime;
        }

        timeTemp = 0f;
        while (timeTemp < 0.4f)
        {
            shadow.transform.localScale = Vector3.Lerp(shadow.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime);
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, bossCtrl.target.transform.position, Time.deltaTime * 40f);

            yield return null;
            timeTemp += Time.deltaTime;
        }
        shadow.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void HighJump()
    {
        var rnd = Random.Range(14, 18);
        GameObject[] goes = new GameObject[rnd];

        for (var i = 0; i < rnd; ++i)
        {
            goes[i] = Instantiate(bullet);
            goes[i].transform.position = bulletSpawn.position;

            var ranX = Random.Range(-100f, 100f);
            var ranY = Random.Range(-100f, 100f);

            var newPos = new Vector3(bulletSpawn.position.x + ranX, bulletSpawn.position.y + ranY);

            var ctrl = goes[i].GetComponent<BulletCtrl>();
            ctrl.startPos = bulletSpawn.position;
            ctrl.endPos = newPos;

            var layerNum = goes[i].GetComponentInChildren<SpriteRenderer>();
            layerNum.sortingOrder = i;
        }
    }

    public void HighJumpMove()
    {
        boss.transform.position = new Vector2(bossCtrl.target.transform.position.x, bossCtrl.target.transform.position.y);
    }

    public IEnumerator ShadowSize()
    {
        float timeTemp = 0f;

        while (timeTemp < 0.8f)
        {
            shadow.transform.localScale = Vector3.Lerp(shadow.transform.localScale, new Vector3(0.5f, 0.5f, 1f), Time.deltaTime);

            yield return null;
            timeTemp += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.3f);
        timeTemp = 0f;
        while (timeTemp < 0.7f)
        {
            shadow.transform.localScale = Vector3.Lerp(shadow.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime);

            yield return null;
            timeTemp += Time.deltaTime;
        }
        shadow.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("bubble"))
        {
            bossCtrl.DecreaseHp();
            Destroy(other.gameObject);
            StartCoroutine(Hited());
        }
    }

    public void Die()
    {
        Destroy(boss.gameObject);
    }

    public IEnumerator Hited()
    {
        var originColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = originColor;
    }
}
