using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class k_plyer : MonoBehaviour
{

    [SerializeField] float xmove=0;//X方向の移動力
    [SerializeField] float zmove=0;//Z方向の移動力
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
            transform.position += new Vector3(xmove*Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))//左にすすむ
            transform.position += new Vector3(-xmove*Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))//上にすすむ
            transform.position += new Vector3(0, 0, zmove*Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow))//下に進む
            transform.position += new Vector3(0, 0, -zmove*Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale != 0) { Time.timeScale = 0.3f;}
            
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Time.timeScale == 0.3f) { Time.timeScale = 1; }
        }
    }
}
