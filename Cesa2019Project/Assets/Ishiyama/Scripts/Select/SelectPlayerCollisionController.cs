using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが衝突している季節の制御
/// </summary>
public class SelectPlayerCollisionController : MonoBehaviour
{
    [SerializeField, Header("季節を表示するUI")]
    Text SeasonText = null;
    SelectSeasonInfo.Season CurrentCollisionSeason = SelectSeasonInfo.Season.None;

    private void Start()
    {
        SeasonText.enabled = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0.0f) return;
        if (SeasonText.enabled &&
         (Input.GetKeyDown("joystick button 1") ||
          Input.GetKeyDown(KeyCode.Return)))
        {
            string sceneName = "GameScene";
            switch (CurrentCollisionSeason)
            {
                case SelectSeasonInfo.Season.Spring:
                    sceneName += "1";
                    break;
                case SelectSeasonInfo.Season.Summer:
                    sceneName += "2";
                    break;
                case SelectSeasonInfo.Season.Autumn:
                    sceneName += "3";
                    break;
                case SelectSeasonInfo.Season.Winter:
                    sceneName += "4";
                    break;
                case SelectSeasonInfo.Season.Extra:
                    sceneName += "5";
                    break;
            }
            FadeController.FadeOut(sceneName + "-1");
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