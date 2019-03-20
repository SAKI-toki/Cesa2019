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
    //[SerializeField]
    //private Star StarScript = null;
    [SerializeField]
    GameObject SelectColor = null;
    [SerializeField, Header("赤")]
    GameObject SelectRed = null;
    [SerializeField, Header("緑")]
    GameObject SelectGreen = null;
    [SerializeField, Header("青")]
    GameObject SelectBlue = null;
    [SerializeField]
    StarPlaceManager StarPlaceController = null;
    private int Select;

    bool SelectFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        Select = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 2"))
        //{
        //    StartSelect();
        //}

        if (!SelectFlg) return;


        //0:緑 1:赤 2:青
        switch (Select)
        {
            case 0:
                EventSystem.current.SetSelectedGameObject(SelectGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    //if (StarScript.BigStarGreen >= 1)
                    //{
                    //    StarScript.AddBigStarGreen(--StarScript.BigGreen);
                    //}
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green) >= 1)
                    {
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Green);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Green);
                    }
                    DeleteSelect();
                }
                break;
            case 1:
                EventSystem.current.SetSelectedGameObject(SelectRed);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    //if (StarScript.BigStarRed >= 1)
                    //{
                    //    StarScript.AddBigStarRed(--StarScript.BigRed);
                    //}
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red) >= 1)
                    {
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Red);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Red);
                    }
                    DeleteSelect();
                }
                break;
            case 2:
                EventSystem.current.SetSelectedGameObject(SelectBlue);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    //if (StarScript.BigStarBlue >= 1)
                    //{
                    //    StarScript.AddBigStarBlue(--StarScript.BigBlue);
                    //}
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue) >= 1)
                    {
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Blue);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Blue);
                    }
                    DeleteSelect();
                }
                break;
        }


        //色の選択
        if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Select++;
            if (Select > 2)
                Select = 0;
        }
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.LeftArrow))
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
        Time.timeScale = 1;
        SelectColor.SetActive(false);
        Select = 0;
        StarPlaceController.StarSelectCancel();
        SelectFlg = false;
    }

    public void StartSelect()
    {
        Time.timeScale = 0;
        SelectColor.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SelectColor);
        SelectFlg = true;
    }

    public bool GetSelectFlg()
    {
         return SelectFlg; 
    }
}
