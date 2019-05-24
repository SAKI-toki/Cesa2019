using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OphiuchusEnemt : MonoBehaviour
{
    [SerializeField]
    Enemy GetEnemy = null;
    [SerializeField, Header("BOSSを入れる")]
    GameObject[] Waves = null;// Waveプレハブを格納する

    bool WaveStop = false;
    int CurrentWave = 0;
    GameObject Wave = null;
    // Start is called before the first frame update
    void Start()
    {
        GetEnemy.NoDamage = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!WaveStop)
        {
            WaveGenerat();
        }

        if (Wave.transform.childCount == 0)
        {
            GetEnemy.EnemyStatus.CurrentHp -= GetEnemy.EnemyHp / 6;
            WaveStop = false;
        }
    }

    /// <summary>
    /// Waveの生成
    /// </summary>
    void WaveGenerat()
    {
        if (CurrentWave < Waves.Length)
        {
            // Waveを作成する
            Wave = (GameObject)Instantiate(Waves[CurrentWave], transform.position, Quaternion.identity);
            CurrentWave++;
            WaveStop = true;
        }
    }
}
