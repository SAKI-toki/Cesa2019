using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waveの生成管理
/// </summary>
public class WaveController : MonoBehaviour
{
    [SerializeField, Header("チュートリアルならtrue")]
    bool Tutorial = false;
    [SerializeField, Header("このステージで置く星の数を入れる")]
    int StarPut = 0;
    [SerializeField]
    GameObject[] Waves = null;// Waveプレハブを格納する
    [SerializeField]
    GameObject BossWave = null;
    [SerializeField, Header("StarPlaceManagerを入れる")]
    StarPlaceManager StarPlaceManager = null;
    [SerializeField, Header("現在のwave確認用")]
    public GameObject wave = null;

    bool WaveStop = false;
    bool ResultFirst = false;
    bool BossWaveFirst = false;
    private int CurrentWave = 0;// 現在のWave
    int StarPutCount = 0;
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
        if (wave.transform.childCount == 0 && BossWaveFirst == false) { WaveStop = false; CurrentWave += 1; }

        if (StarPut == StarPutCount && BossWaveFirst == false)
        {
            WaveStop = true;
            wave = (GameObject)Instantiate(BossWave, transform.position, Quaternion.identity);
            wave.transform.parent = transform;
            BossWaveFirst = true;
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

        // Wave生成
        if (!WaveStop)
        {
            WaveGenerat();
            StarPlaceManager.StarPut = false;
        }

        // 次のWaveへ（Wave1のみ)
        if (CurrentWave == 0 && wave.transform.childCount == 0)
        {
            Destroy(wave);
            WaveStop = false;
            CurrentWave += 1;
        }
        // 次のWaveへ（Wave1以外）
        if (CurrentWave > 0 && StarPlaceManager.StarPut == true && wave.transform.childCount == 0)
        {
            Destroy(wave);
            WaveStop = false;
            CurrentWave += 1;
        }
    }

    /// <summary>
    /// Waveの生成
    /// </summary>
    void WaveGenerat()
    {
        // Waveを作成する
        wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);
        // WaveをWaveController の子要素にする
        wave.transform.parent = transform;
        // Waveの生成を止める
        WaveStop = true;
    }
}


