using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlayerScoreAsset")]
public class PlayerScoreAsset : ScriptableObject
{
    public int[] playerScore;
    public int[] playerHealth;
}
