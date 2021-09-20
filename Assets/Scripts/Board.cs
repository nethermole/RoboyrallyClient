using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DTOBoard
{
    public List<Player> players;
    public Dictionary<int, Dictionary<int, Tile>> squares;
}
