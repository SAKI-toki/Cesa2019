using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClear : IPlayerState
{
    void IPlayerState.Init(Player player)
    {

    }

    IPlayerState IPlayerState.Update(Player player)
    {
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {

    }
}
