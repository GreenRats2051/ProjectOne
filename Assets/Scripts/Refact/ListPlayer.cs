using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListPlayer
{
    public List<GameObject> _players { get; private set; } = new List<GameObject>();
    private static ListPlayer inst;
    public static ListPlayer Inst 
    { 
        get 
        {
            if (inst == null)
            {
                inst = new ListPlayer();
            }

            return inst;
        } 
    }
    public void addPlayer(GameObject Player)
    {
        _players.Add(Player);
    }
    public void removePlayer(GameObject Player)
    {
        _players.Remove(Player);
    }

}
