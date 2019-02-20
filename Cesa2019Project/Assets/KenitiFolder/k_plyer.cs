using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class k_plyer : MonoBehaviour
{

    [SerializeField] float xmove;//X方向の移動力
    [SerializeField] float zmove;//Z方向の移動力
    // Start is called before the first frame update
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))//右に進む
            transform.position += new Vector3(xmove, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))//左にすすむ
            transform.position += new Vector3(-xmove, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))//上にすすむ
            transform.position += new Vector3(0, 0, zmove);
        if (Input.GetKey(KeyCode.DownArrow))//下に進む
            transform.position += new Vector3(0, 0, -zmove);
    }
}
