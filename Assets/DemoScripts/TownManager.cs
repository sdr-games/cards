using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownManager : MonoBehaviour
{
    public void Back()
    {
        MapManager.Instance.LoadMap(false);
    }
}
