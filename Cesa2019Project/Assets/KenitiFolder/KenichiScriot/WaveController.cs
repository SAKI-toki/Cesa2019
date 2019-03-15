using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waveの生成管理
/// </summary>
public class WaveController : MonoBehaviour
{
    [SerializeField]
    GameObject[] Waves = null;// Waveプレハブを格納する
    [SerializeField, Header("最初Waveの敵が何体以上で残るか")]
    int RemainingCount = 0;
    [SerializeField, Header("フィールドに出る敵の最大数")]
    int MaxEnemy = 0;
    [SerializeField,Header("現在のwave確認用")]
    GameObject wave = null;

    bool EnemyStop = false;
    bool WaveStop = false;
    private int CurrentWave = 0;// 現在のWave
    int Child = 0;
    int RemainingEnemy = 1;
    static public int EnemyCount = 0;
    GameObject ChildCount = null;

    /// <summary>
    /// Waveの生成の管理
    /// </summary>
    private void Update()
    {
       //敵が一定数いたらWaveの生成を止める
        if (EnemyCount >= MaxEnemy) { EnemyStop = true; }
        else { EnemyStop = false; }

        if (EnemyStop == false && WaveStop == false)
        {
            // Waveを作成する
            wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);

            // WaveをWaveController の子要素にする
            wave.transform.parent = transform;

            //次のWaveに進ませるための条件を決める
            if (wave.transform.childCount <= RemainingCount) { RemainingEnemy = 1; }
            else { RemainingEnemy = 2; }

            Child += 1;
            WaveStop = true;
        }
        //敵がRemainingEnemy分残ったら次のWaveを生成
        if (wave.transform.childCount == RemainingEnemy) { WaveStop = false; CurrentWave += 1; }

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
        //格納されているWaveを全て実行したらCurrentWaveを0にする
        if (Waves.Length == CurrentWave) { CurrentWave = 0; }
    }
}


