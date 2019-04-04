using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 星を置く処理を管理する
/// </summary>
public class StarPlaceManager : MonoBehaviour
{
    [System.Serializable]
    class Line
    {
        [SerializeField]
        public GameObject StarPlace1 = null;
        [SerializeField]
        public GameObject StarPlace2 = null;
        [System.NonSerialized]
        public bool DrawEnd = false;
    }
    [System.Serializable]
    class Triangle
    {
        [SerializeField]
        public Line[] TriangleLine = new Line[3];
        public bool DrawEnd = false;
    }
    [SerializeField, Header("プレイヤー")]
    GameObject Player = null;               // プレイヤー
    [SerializeField, Header("星")]
    GameObject Star = null;                 // 星
    Vector3 PlayerPos = Vector3.zero;       // プレイヤーの位置
    [SerializeField]
    List<Line> LineList = new List<Line>();
    [SerializeField]
    List<Triangle> TriangleList = new List<Triangle>();
    [SerializeField, Header("麻痺時間")]
    float ParalysisTime = 0;                // 麻痺時間
    bool ParalysisFlg = false;              // 麻痺フラグ
    [SerializeField, Header("毒ダメージ")]
    float PoisonDamage = 0;                 // 毒ダメージ
    [SerializeField, Header("毒時間")]
    float PoisonTime = 0;                   // 毒時間
    bool PoisonFlg = false;                 // 毒フラグ
    List<StarPlace> StarPlaceList = new List<StarPlace>();      // 星を置く場所のリスト
    [SerializeField, Header("星が置けるようになる距離")]
    float ActiveDistance = 0;               // 星を置けるようになる距離
    int StarSelectPlaceNum = 0;
    public static bool StarSelect = false;  // 星の色を選択中か
    public bool AllPlaceSet = false;               // 星が全てセットされているかのフラグ
    public bool StarPut = true;             // 星をセットした
    [SerializeField]
    StarSlect StarSelectController = null;
    [SerializeField]
    GameObject RedStar = null;
    [SerializeField]
    GameObject GreenStar = null;
    [SerializeField]
    GameObject BlueStar = null;
    [SerializeField]
    Pause Pause = null;
    [SerializeField]
    WaveController GetWaveController = null;

    [System.NonSerialized]
    public int RedStarNum = 0;
    [System.NonSerialized]
    public int GreenStarNum = 0;
    [System.NonSerialized]
    public int BlueStarNum = 0;
    [System.NonSerialized]
    public int StarNum = 0;

    void Start()
    {
        int num = 0;
        foreach (Transform child in transform)
        {
            if (null != child.GetComponent<StarPlace>())
            {
                StarPlaceList.Add(child.GetComponent<StarPlace>());
                //最初からセットしているかどうか
                if (child.GetComponent<StarPlace>().IsAwakeSet)
                {
                    ++StarNum;
                    switch (child.GetComponent<StarPlace>().StarColor)
                    {
                        case HaveStarManager.StarColorEnum.Red:
                            ++RedStarNum;
                            PlayerController.PlayerStatus.HpUp(1);
                            PlayerController.PlayerStatus.AttackUp(5);
                            StarPlaceList[num].Star = Instantiate(RedStar,
                                child.transform.position + new Vector3(0, 1, 0),
                                Quaternion.identity);
                            break;
                        case HaveStarManager.StarColorEnum.Blue:
                            ++BlueStarNum;
                            PlayerController.PlayerStatus.HpUp(1);
                            PlayerController.PlayerStatus.DefenseUp(5);
                            StarPlaceList[num].Star = Instantiate(BlueStar,
                                child.transform.position + new Vector3(0, 1, 0),
                                Quaternion.identity);
                            break;
                        case HaveStarManager.StarColorEnum.Green:
                            ++GreenStarNum;
                            PlayerController.PlayerStatus.HpUp(1);
                            PlayerController.PlayerStatus.SpeedUp(2);
                            StarPlaceList[num].Star = Instantiate(GreenStar,
                                child.transform.position + new Vector3(0, 1, 0),
                                Quaternion.identity);
                            break;
                        default:
                            StarPlaceList[num].Star = Instantiate(Star,
                                child.transform.position + new Vector3(0, 1, 0),
                                Quaternion.identity);
                            break;
                    }
                }
            }
            ++num;
        }
        PlayerController.PlayerStatus.ResetStatus();
        LineCheck();
    }

    void Update()
    {
        // 全て星がセットされているか
        if (!AllPlaceSet)
        {
            if (!StarSelect)
            {
                for (int i = 0; i < StarPlaceList.Count; ++i)
                {
                    // 星がセットされているか
                    if (!StarPlaceList[i].isSet)
                    {
                        PlayerPos = Player.transform.position;

                        float distance = Vector3.Distance(StarPlaceList[i].Pos, PlayerPos);
                        // 距離が範囲内か
                        if (distance < ActiveDistance)
                        {
                            // 星を持っていたら
                            //if (PlayerController.StarPieceHave >= Constant.ConstNumber.StarConversion)
                            if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue) >= 1 ||
                               HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green) >= 1 ||
                               HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red) >= 1)
                            {
                                StarPlaceList[i].isActive = true;
                            }
                            // 星を持っていない
                            else
                            {
                                Debug.Log("====星が無いよ====");
                            }
                        }
                        else if (distance > ActiveDistance)
                        {
                            StarPlaceList[i].isActive = false;
                        }

                        // 範囲内にいるとき
                        if (StarPlaceList[i].isActive && !Pause.GetPauseFlg()&&GetWaveController.Tutorial)
                        {
                            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.F))
                            {
                                if (GetWaveController.WaveStop) { return; }
                                StarSelectPlaceNum = i;
                                StarSelectActive();
                            }
                        }
                        else if(StarPlaceList[i].isActive && !Pause.GetPauseFlg() && !GetWaveController.Tutorial)
                        {
                            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.F))
                            {
                                
                                StarSelectPlaceNum = i;
                                StarSelectActive();
                            }
                        }
                    }
                }
            }
        }
        // 全ての星がセットされている
        else if (AllPlaceSet)
        {

        }
    }

    void StarSelectActive()
    {
        StarSelect = true;
        StarSelectController.StartSelect();
        //Time.timeScale = 0;
        //StarSelectUI.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(StartButton);
    }

    public void StarSelectCancel()
    {
        StarSelect = false;
        //Time.timeScale = 1.0f;
        //StarSelectUI.SetActive(false);
    }

    /// <summary>
    /// 星の配置
    /// </summary>
    public void StarSet(HaveStarManager.StarColorEnum starColor)
    {
        //StarSelect = false;
        //Time.timeScale = 1.0f;
        //StarSelectUI.SetActive(false);
        StarPlaceList[StarSelectPlaceNum].isSet = true;
        StarPlaceList[StarSelectPlaceNum].StarColor = starColor;
        if (starColor == HaveStarManager.StarColorEnum.Red)
        {
            ++RedStarNum;
            PlayerController.PlayerStatus.AttackUp(5);
        }
        if (starColor == HaveStarManager.StarColorEnum.Blue)
        {
            ++BlueStarNum;
            PlayerController.PlayerStatus.DefenseUp(5);
        }
        if (starColor == HaveStarManager.StarColorEnum.Green)
        {
            ++GreenStarNum;
            PlayerController.PlayerStatus.SpeedUp(2);
        }
        ++StarNum;
        //PlayerController.StarPieceHave -= Constant.ConstNumber.StarConversion;
        GenerateStar(StarSelectPlaceNum, starColor);
        StarPut = true;
        LineCheck();
    }

    void GenerateStar(int n, HaveStarManager.StarColorEnum starColor)
    {
        StarPlaceList[n].Star =
            Instantiate((starColor == HaveStarManager.StarColorEnum.Red ? RedStar :
            starColor == HaveStarManager.StarColorEnum.Green ? GreenStar : BlueStar),
            StarPlaceList[n].gameObject.transform.position + new Vector3(0, 1, 0),
            Quaternion.identity);
        var colliderList = StarPlaceList[n].GetComponents<SphereCollider>();
        foreach (var collider in colliderList)
        {
            collider.enabled = false;
        }
        AllPlaceSet = AllPlaceSetCheck();
    }

    /// <summary>
    /// 星が全て配置されたかをチェックする
    /// </summary>
    bool AllPlaceSetCheck()
    {
        for (int i = 0; i < StarPlaceList.Count; ++i)
        {
            if (StarPlaceList[i].isSet == false)
            {
                return false;
            }
        }
        Debug.Log("====星のセット完了====");
        return true;
    }

    /// <summary>
    /// 星が配置されて線を描く
    /// </summary>
    public void LineCheck()
    {
        // 線
        for (int i = 0; i < LineList.Count; ++i)
        {
            if (!LineList[i].DrawEnd)
            {
                DorwLine(LineList[i]);
            }
        }
        // 三角形
        for (int i = 0; i < TriangleList.Count; ++i)
        {
            if (!TriangleList[i].DrawEnd)
            {
                for (int j = 0; j < TriangleList[i].TriangleLine.Length; ++j)
                {
                    if (!TriangleList[i].TriangleLine[j].DrawEnd)
                    {
                        DorwLine(TriangleList[i].TriangleLine[j]);
                    }
                }
                for (int j = 0; j < TriangleList[i].TriangleLine.Length; ++j)
                {
                    if (TriangleList[i].TriangleLine[j].DrawEnd)
                    {
                        if (j + 1 == TriangleList[i].TriangleLine.Length)
                        {
                            TriangleList[i].DrawEnd = true;
                            if (Random.Range(0, 2) == 0)
                            {
                                Debug.Log("毒");
                                PoisonFlg = true;
                            }
                            else
                            {
                                Debug.Log("麻痺");
                                ParalysisFlg = true;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    void DorwLine(Line line)
    {
        if (line.StarPlace1.GetComponent<StarPlace>().isSet &&
            line.StarPlace2.GetComponent<StarPlace>().isSet)
        {
            LineRenderer lineRendererStarPlace1 = line.StarPlace1.GetComponent<LineRenderer>();
            lineRendererStarPlace1.positionCount = lineRendererStarPlace1.positionCount + 2;

            lineRendererStarPlace1.SetPosition(
                lineRendererStarPlace1.positionCount - 2,
                line.StarPlace1.GetComponent<StarPlace>().Star.transform.position);

            lineRendererStarPlace1.SetPosition(
                lineRendererStarPlace1.positionCount - 1,
                line.StarPlace2.GetComponent<StarPlace>().Star.transform.position);
            line.DrawEnd = true;

            // 線同色ボーナス
            if (line.StarPlace1.GetComponent<StarPlace>().StarColor == line.StarPlace2.GetComponent<StarPlace>().StarColor)
            {
                switch (line.StarPlace1.GetComponent<StarPlace>().StarColor)
                {
                    case HaveStarManager.StarColorEnum.Red:
                        PlayerController.PlayerStatus.AttackUp(5);
                        break;
                    case HaveStarManager.StarColorEnum.Blue:
                        PlayerController.PlayerStatus.DefenseUp(5);
                        break;
                    case HaveStarManager.StarColorEnum.Green:
                        PlayerController.PlayerStatus.SpeedUp(2);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
