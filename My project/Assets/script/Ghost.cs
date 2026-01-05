using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    GameObject player;
    public int speed;
    public float range = 5f;
    float distance;

    public bool isPlayerEnter;

    Rigidbody2D rigid;
    SpriteRenderer rend;
    Monster mons;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        mons = GetComponent<Monster>();
    }

    void FixedUpdate()
    {
        if (!mons.isLive || mons.isHit || GameManager.Instance.isHide)
            return;

        distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance <= range)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        if (distance >= range || !mons.isLive || mons.isHit)
            return;

        rend.flipX = player.transform.position.x < rigid.position.x;
    }
}
