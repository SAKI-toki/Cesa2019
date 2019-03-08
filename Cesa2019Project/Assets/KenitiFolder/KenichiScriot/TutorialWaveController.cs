using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaveController : MonoBehaviour
{
    [SerializeField]
    GameObject[] Waves = null;// Waveプレハブを格納する
    [SerializeField,Header("StarPlaceManagerを入れる")]
    StarPlaceManager StarPlaceManager = null;
    [SerializeField, Header("現在のwave確認用")]
    GameObject wave = null;

    bool WaveStop = false;
    private int CurrentWave = 0;// 現在のWave
    int Child = 0;
    GameObject ChildCount = null;

    /// <summary>
    /// Waveの生成の管理
    /// </summary>
    private void Update()
    {
        //格納されているWaveを全て実行したら終了する
        if (Waves.Length <= CurrentWave) { return; }

        if (CurrentWave <= 1 && WaveStop == false)
        {
            // Waveを作成する
            wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);

            // WaveをWaveController の子要素にする
            wave.transform.parent = transform;
            Child += 1;
            WaveStop = true;
            StarPlaceManager.StarPut = false;
        }

        if (StarPlaceManager.StarPut == true && WaveStop == false)
        {
            // Waveを作成する
            wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);

            // WaveをWaveController の子要素にする
            wave.transform.parent = transform;
            Child += 1;
            WaveStop = true;
            StarPlaceManager.StarPut = false;
        }

        if (this.transform.childCount >= 1)
        {
            //敵がRemainingEnemy分残ったら次のWaveを生成
            if (wave.transform.childCount == 0) { WaveStop = false; CurrentWave += 1; }

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
}
