﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("弾の移動力")]
    float BulletMove = 2;
    [SerializeField, Header("弾の破壊時間")]
    float DestroyTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            transform.Translate(0, 0, BulletMove * Time.deltaTime);
            Destroy(gameObject, DestroyTime);
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
