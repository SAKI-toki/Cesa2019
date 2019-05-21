using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンをすぐ遷移するスクリプト
/// </summary>
public class DebugSceneTransition : MonoBehaviour
{
#if UNITY_EDITOR
    //季節の番号(1:春、2:夏、3:秋、4:冬)
    int SeasonNum = 0;
    //ステージの番号
    int StageNum = 0;
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (SeasonNum == 0)
            {
                InputSeason();
            }
            else if (StageNum == 0)
            {
                InputStage();
            }
        }
        if (Input.GetKeyDown(KeyCode.T) && SeasonNum != 0 && StageNum != 0)
        {
            string sceneName = "GameScene";
            sceneName += SeasonNum.ToString() + "-" + StageNum.ToString();
            SceneManager.LoadScene(sceneName);
        }
    }


    void InputSeason()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SeasonNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SeasonNum = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SeasonNum = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SeasonNum = 4;
        }
    }

    void InputStage()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StageNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StageNum = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StageNum = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StageNum = 4;
        }
    }
#endif
}
