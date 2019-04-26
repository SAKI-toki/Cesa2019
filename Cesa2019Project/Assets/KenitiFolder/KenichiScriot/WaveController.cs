using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waveの生成管理
/// </summary>
public class WaveController : MonoBehaviour
{
    [SerializeField, Header("チュートリアルならtrue")]
    public bool Tutorial = false;
    [SerializeField, Header("このステージで置く星の数を入れる")]
    int StarPut = 0;
    [SerializeField]
    GameObject[] Waves = null;// Waveプレハブを格納する
    [SerializeField]
    GameObject BossWave = null;
    [SerializeField, Header("StarPlaceManagerを入れる")]
    StarPlaceManager StarPlaceManager = null;
    [SerializeField, Header("現在のwave確認用")]
    public GameObject Wave = null;

    [HideInInspector]
    public bool WaveStop = false;
    [HideInInspector]
    public bool BossWaveFlag = false;
    bool BossWaveFirst = false;
    bool ResultFirst = false;

    GameObject BossWaveObj = null;
    private int CurrentWave = 0;// 現在のWave
    int StarPutCount = 0;
    int Child = 0;
    static public int WaveCount = 0;
    GameObject ChildCount = null;
    BGM BGM = null;

    private void Start()
    {
        BGM = this.GetComponent<BGM>();
    }

    /// <summary>
    /// Waveの生成の管理
    /// </summary>
    private void Update()
    {


        if (StarPlaceManager.AllPlaceSet && ResultFirst == false)
        {
            Destroy(Wave);
            Destroy(BossWaveObj);
            BGM.Result();
            ResultFirst = true;
        }

        if (Tutorial)
        {
            TutorialWaveControl();
            return;
        }

        if (WaveStop == false && BossWaveFirst == false)
        {
            WaveGenerat();
        }
        //敵を全て倒したら次のWaveを生成
        if (Wave.transform.childCount == 0 && BossWaveFirst == false) { WaveStop = false; CurrentWave += 1; }

        //ボス敵の生成
        if (StarPut == StarPutCount && BossWaveFirst == false)
        {
            WaveStop = true;
            BossWaveObj = (GameObject)Instantiate(BossWave, transform.position, Quaternion.identity);
            BossWaveObj.transform.parent = transform;
            BossWaveFlag = true;
            BossWaveFirst = true;
        }

        if (BossWaveFirst)
        {
            if (Wave.transform.childCount == 0) { BossWaveFlag = false; }
        }

        //格納されているWaveを全て実行したらCurrentWaveを0にする
        if (Waves.Length == CurrentWave) { CurrentWave = 0; }

        if (StarPlaceManager.StarPut == true)
        {
            StarPutCount++;
            StarPlaceManager.StarPut = false;
        }
    }


    /// <summary>
    /// チュートリアル用のWave生成
    /// </summary>
    void TutorialWaveControl()
    {
        //格納されているWaveを全て実行したら終了する
        if (Waves.Length <= CurrentWave) { return; }

        if (CurrentWave <= 1 && WaveStop == false)
        {
            WaveGenerat();
            StarPlaceManager.StarPut = false;
        }

        if (StarPlaceManager.StarPut == true && WaveStop == false)
        {
            WaveGenerat();
            StarPlaceManager.StarPut = false;
        }

        if (this.transform.childCount >= 1)
        {

            //敵全滅で次のWaveを生成
            if (Wave.transform.childCount == 0) { WaveStop = false; CurrentWave += 1; }

            //Waveの中の敵が全て削除されたらWaveそのWaveを消す
            for (int i = 0; i < Child; i++)
            {
                ChildCount = transform.GetChild(i).gameObject;
                if (ChildCount.transform.childCount == 0)
                {
                    Child -= 1;
                    Destroy(ChildCount);
                }
            }
        }
    }

    /// <summary>
    /// Waveの生成
    /// </summary>
    void WaveGenerat()
    {
        Destroy(Wave);
        // Waveを作成する
        Wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);

        // WaveをWaveController の子要素にする
        Wave.transform.parent = transform;
        Child += 1;
        WaveCount += 1;
        WaveStop = true;
    }
}


