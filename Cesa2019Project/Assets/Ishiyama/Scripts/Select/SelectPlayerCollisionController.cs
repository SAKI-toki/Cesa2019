using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが衝突している季節の制御
/// </summary>
public class SelectPlayerCollisionController : MonoBehaviour
{
    [SerializeField, Header("季節を表示するUI")]
    Text SeasonText = null;
    //衝突している季節
    SelectSeasonInfo.Season CurrentCollisionSeason = SelectSeasonInfo.Season.None;

    private void Start()
    {
        //テキストは最初表示しない
        SeasonText.enabled = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0.0f) return;
        //テキストが表示され、決定ボタンを押したらシーン遷移
        if (SeasonText.enabled &&
        SelectSceneObjectManager.SeasonUnlock[(int)CurrentCollisionSeason] &&
         (Input.GetKeyDown("joystick button 1") ||
          Input.GetKeyDown(KeyCode.Return)))
        {
            string sceneName = "GameScene";
            //列挙型から季節を取得
            switch (CurrentCollisionSeason)
            {
                case SelectSeasonInfo.Season.Spring:
                    sceneName += "1";
                    SelectSceneObjectManager.Select = true;
                    break;
                case SelectSeasonInfo.Season.Summer:
                    sceneName += "2";
                    SelectSceneObjectManager.Select = true;
                    break;
                case SelectSeasonInfo.Season.Autumn:
                    sceneName += "3";
                    SelectSceneObjectManager.Select = true;
                    break;
                case SelectSeasonInfo.Season.Winter:
                    sceneName += "4";
                    SelectSceneObjectManager.Select = true;
                    break;
                case SelectSeasonInfo.Season.Extra:
                    FadeController.FadeOut("ExtraScene");
                    break;
            }
            //FadeController.FadeOut(sceneName + "-1");
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        var seasonComponent = other.gameObject.GetComponent<SelectSeasonInfo>();
        if (seasonComponent)
        {
            SeasonText.enabled = true;
            CurrentCollisionSeason = seasonComponent.ThisSeason;
            string seasonName = "???";
            if (SelectSceneObjectManager.SeasonUnlock[(int)CurrentCollisionSeason])
            {
                switch (seasonComponent.ThisSeason)
                {
                    case SelectSeasonInfo.Season.Spring:
                        seasonName = "春";
                        break;
                    case SelectSeasonInfo.Season.Summer:
                        seasonName = "夏";
                        break;
                    case SelectSeasonInfo.Season.Autumn:
                        seasonName = "秋";
                        break;
                    case SelectSeasonInfo.Season.Winter:
                        seasonName = "冬";
                        break;
                    case SelectSeasonInfo.Season.Extra:
                        seasonName = "エクストラ";
                        break;
                }

            }
            SeasonText.text = seasonName;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var seasonComponent = other.gameObject.GetComponent<SelectSeasonInfo>();
        if (seasonComponent)
        {
            SeasonText.enabled = false;
        }
    }
};