using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSeasonInfo : MonoBehaviour
{
    public enum Season { Spring, Summer, Autumn, Winter, Extra, None };

    [SerializeField, Header("季節")]
    public Season ThisSeason = Season.None;
}
