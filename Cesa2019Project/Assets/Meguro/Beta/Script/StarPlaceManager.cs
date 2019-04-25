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

    [SerializeField, Header("プレイヤー")]
    GameObject Player = null;               // プレイヤー
    [SerializeField, Header("星")]
    GameObject Star = null;                 // 星
    Vector3 PlayerPos = Vector3.zero;       // プレイヤーの位置
    [SerializeField]
    List<Line> LineList = new List<Line>();
    HaveStarManager.StarColorEnum[] StarPutMemory = new HaveStarManager.StarColorEnum[3] { HaveStarManager.StarColorEnum.None, HaveStarManager.StarColorEnum.None, HaveStarManager.StarColorEnum.None };
    List<StarPlace> StarPlaceList = new List<StarPlace>();      // 星を置く場所のリスト
    [SerializeField, Header("WaveController")]
    WaveController EnemyWave = null;
    [SerializeField, Header("麻痺範囲")]
    float ParalysisDis = 0;
    [SerializeField, Header("星が置けるようになる距離")]
    float ActiveDistance = 0;               // 星を置けるようになる距離
    int StarSelectPlaceNum = 0;
    public static bool StarSelect = false;  // 星の色を選択中か
    public static bool AllPlaceSet = false;        // 星が全てセットされているかのフラグ
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
                            if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue) >= 1 ||
                               HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green) >= 1 ||
                               HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red) >= 1)
                            {
                                StarPlaceList[i].isActive = true;
                            }
                            // 星を持っていない
                            else
                            {
                                //Debug.Log("====星が無いよ====");
                            }
                        }
                        else if (distance > ActiveDistance)
                        {
                            StarPlaceList[i].isActive = false;
                        }

                        // 範囲内にいるとき
                        if (StarPlaceList[i].isActive && !Pause.GetPauseFlg())
                        {
                            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.F))
                            {
                                StarSelectPlaceNum = i;
                                StarSelectActive();
                            }
                        }
                    }
                }

                // Debug用
                /* ============================================================= */
                // 麻痺
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ParalysisBonus();
                }
                // 毒
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    PoisonBonus();
                }
                /* ============================================================= */
            }
        }
    }

    void StarSelectActive()
    {
        StarSelect = true;
        StarSelectController.StartSelect();
    }

    public void StarSelectCancel()
    {
        StarSelect = false;
    }

    /// <summary>
    /// 星の配置
    /// </summary>
    public void StarSet(HaveStarManager.StarColorEnum starColor)
    {
        if (StarPutMemory[StarPutMemory.Length - 1] == HaveStarManager.StarColorEnum.None)
        {
            for (int i = 0; i < StarPutMemory.Length; ++i)
            {
                if (StarPutMemory[i] == HaveStarManager.StarColorEnum.None)
                {
                    StarPutMemory[i] = starColor;
                    if (i == StarPutMemory.Length - 1)
                    {
                        ColorCheck();
                    }
                    break;
                }
            }
        }
        else
        {
            for (int j = 1; j < StarPutMemory.Length; ++j)
            {
                StarPutMemory[j - 1] = StarPutMemory[j];
            }
            StarPutMemory[StarPutMemory.Length - 1] = starColor;
            ColorCheck();
        }

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

    void ColorCheck()
    {
        bool bonusflg = true;
        for (int i = 0; i < StarPutMemory.Length - 1; ++i)
        {
            if (StarPutMemory[i] == StarPutMemory[i + 1])
            {
                bonusflg = false;
                break;
            }
        }
        if (bonusflg)
        {
            if (StarPutMemory[0] != StarPutMemory[StarPutMemory.Length - 1])
            {
                // 敵がいないときはボーナスなし
                if (EnemyWave.wave.transform.childCount > 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        // 麻痺
                        ParalysisBonus();
                    }
                    else
                    {
                        // 毒
                        PoisonBonus();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 麻痺ボーナス
    /// </summary>
    void ParalysisBonus()
    {
        Debug.Log("麻痺");
        foreach (Transform child in EnemyWave.wave.transform)
        {
            float dis = Vector3.Distance(PlayerPos, child.transform.position);

            if (dis < ParalysisDis)
            {
                child.GetComponent<Enemy>().EnemyAbnormalState.ParalysisStart();
            }
        }
        // 範囲debug
        Ray ray = new Ray(PlayerPos, transform.forward);
        for (int i = 0; i < 16; ++i)
        {
            int angle = 360 - ((360 / 16) * i);
            Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
            Debug.DrawRay(ray.origin, dir * ParalysisDis, Color.red, 5);
        }
    }

    /// <summary>
    /// 毒ボーナス
    /// </summary>
    void PoisonBonus()
    {
        Debug.Log("毒");
        foreach (Transform child in EnemyWave.wave.transform)
        {
            child.GetComponent<Enemy>().EnemyAbnormalState.PoisonStart();
        }
    }
}