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
    [SerializeField, Header("Waveの敵")]
    int RemainingCount = 0;
    [SerializeField, Header("フィールドに出る敵の最大数")]
    int MaxEnemy = 0;
    [SerializeField, Header("Debug用")]
    GameObject wave = null;
    [SerializeField, Header("Debug用")]
    bool EnemyStop = false;
    [SerializeField, Header("Debug用")]
    bool WaveStop = false;

    // 現在のWave
    private int CurrentWave = 0;
    int Child = 0;
    int RemainingEnemy = 1;
    static public int EnemyCount = 0;
    GameObject ChildCount = null;

    private void Update()
    {
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
        for (int i = 0; i != Child; i++)
        {
            ChildCount = transform.GetChild(i).gameObject;
            if (ChildCount.transform.childCount == 0) { Child -= 1; Destroy(ChildCount); }
        }

        //格納されているWaveを全て実行したらCurrentWaveを0にする
        if (Waves.Length == CurrentWave) { CurrentWave = 0; }
    }
}


