using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Levels")]
public class LevelAsset : ScriptableObject
{
    public GameObject[] levels;
    public int[] levelIndex;
}
