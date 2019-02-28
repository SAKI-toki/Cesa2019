using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 星の選択
/// </summary>
public class StarSlect : MonoBehaviour
{
    [SerializeField]
    private Star StarScript = null;
    [SerializeField]
    GameObject SelectColor = null;
    [SerializeField, Header("選択されている色")]
    GameObject SelectColorButton = null;


    private int Select = 0;

    // Start is called before the first frame update
    void Start()
    {
        Select = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 0;
            SelectColor.SetActive(true);
            EventSystem.current.SetSelectedGameObject(SelectColorButton);
        }

        //0:緑 1:赤 2:青
        switch (Select)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (StarScript.BigStarGreen >= 1)
                    {
                        StarScript.AddBigStarGreen(--StarScript.BigGreen);
                    }
                    DeleteSelect();
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (StarScript.BigStarRed >= 1)
                    {
                        StarScript.AddBigStarRed(--StarScript.BigRed);

                    }
                    DeleteSelect();
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (StarScript.BigStarBlue >= 1)
                    {
                        StarScript.AddBigStarBlue(--StarScript.BigBlue);
                    }
                    DeleteSelect();
                }
                break;
        }

        //色の選択
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Select++;
            if (Select > 2)
                Select = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Select--;
            if (Select < 0)
                Select = 2;
        }
    }

    /// <summary>
    /// 選択画面を消す処理
    /// </summary>
    void DeleteSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1;
        SelectColor.SetActive(false);
        Select = 0;
    }
}
