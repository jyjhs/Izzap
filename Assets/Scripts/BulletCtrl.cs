using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public Vector2 endPos;
    public Vector2 startPos;
    private Vector2 rand;
    private float time = 0f;
    private float randY = 0f;

    private float ranTime = 0f;
    public Animator anim;

    private void Start()
    {
        time = 0f;
        if (endPos.y > startPos.y)
            randY = Random.Range(endPos.y + 5f, endPos.y + 40f);
        else
        {
            randY = Random.Range(startPos.y + 5f, startPos.y + 40f);
        }

        rand = new Vector2(Random.Range(endPos.x, startPos.x), randY);
        ranTime = Random.Range(0.8f, 1.3f);
    }

    private void FixedUpdate()  
    {
        if (time < ranTime)
        {
            transform.position = BezierCurve(time, startPos, rand, endPos);
            time += Time.deltaTime * 0.7f;
        }
        else
        {
            anim.Play("Bullet");
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    private Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1)
    {
        return ((1 - t) * p0) + ((t) * p1);
    }

    private Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 pa = BezierCurve(t, p0, p1);
        Vector3 pb = BezierCurve(t, p1, p2);
        return BezierCurve(t, pa, pb);
    }
}
