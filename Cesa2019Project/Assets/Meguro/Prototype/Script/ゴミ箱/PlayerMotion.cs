using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMotion
{
    void Start();
    void Update();
    void End();
    void Next();
}

//public class PlayerMotion : MonoBehaviour
//{
//    IPlayerMotion PlayerM;

//    public PlayerMotion()
//    {
//        PlayerM = new PlayerIdle();
//    }
//}