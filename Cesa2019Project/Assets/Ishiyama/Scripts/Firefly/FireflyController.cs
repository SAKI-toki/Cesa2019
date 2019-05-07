using UnityEngine;

/// <summary>
/// 蛍の制御クラス
/// </summary>
public class FireflyController : MonoBehaviour
{
    //移動速度
    const float MoveSpeed = 1.0f;
    //向きを変えるランダムな時間の最小値、最大値
    const float RandomMin = 3.0f;
    const float RandomMax = 5.0f;
    //ランダムな時間
    float RandomTime = 0.0f;
    //時間を計測する
    float CountTime = 0.0f;
    //角度を格納
    Quaternion PrevQuaternion = new Quaternion();
    Quaternion NextQuaternion = new Quaternion();

    void Start()
    {
        RandomFunction();
    }

    void Update()
    {
        CountTime += Time.deltaTime;
        if (CountTime > RandomTime)
        {
            RandomFunction();
            CountTime = 0.0f;
        }
        transform.rotation = Quaternion.Lerp(PrevQuaternion, NextQuaternion, CountTime / RandomTime);
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// ランダムな角度や時間をセットする
    /// </summary>
    void RandomFunction()
    {
        //現在の角度を格納
        PrevQuaternion = transform.rotation;
        //新しい時間を格納
        RandomTime = Random.Range(RandomMin, RandomMax);
        //次に向く向きベクトル
        Vector3 vector =
            new Vector3(
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f)).normalized;
        //ゼロはダメなのでxを1.0fにして回避する
        if (vector == Vector3.zero)
        {
            vector.x = 1.0f;
        }
        NextQuaternion = Quaternion.LookRotation(vector);
    }
}
