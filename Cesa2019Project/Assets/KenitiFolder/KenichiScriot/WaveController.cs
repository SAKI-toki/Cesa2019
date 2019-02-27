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

    // 現在のWave
    private int CurrentWave;

    IEnumerator Start()
    {

        // Waveが存在しなければコルーチンを終了する
        if (Waves.Length == 0)
        {
            yield break;
        }

        while (true)
        {

            // Waveを作成する
            GameObject wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);

            // WaveをEmitterの子要素にする
            wave.transform.parent = transform;

            // Waveの子要素のEnemyが全て削除されるまで待機する
            while (wave.transform.childCount != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            // Waveの削除
            Destroy(wave);

            //格納されているWaveを全て実行したらCurrentWaveを0にする
            if (Waves.Length <= ++CurrentWave)
            {
                CurrentWave = 0;
            }
        }
    }
}
