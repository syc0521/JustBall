using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public AnimationAsset animationAsset;
    public PlayerScoreAsset playerInformation;
    public SpriteRenderer[] selectSprite;
    public static int state = 0;
    public SpriteRenderer map;
    public int mapIndex = 0;

    private void Start()
    {
        LoadingManager.nextLevelIndex = 0;
        state = 0;
        mapIndex = 0;
        for (int i = 0; i < playerInformation.playerHealth.Length; i++)
        {
            playerInformation.playerHealth[i] = 0;
        }
        for (int i = 0; i < playerInformation.playerScore.Length; i++)
        {
            playerInformation.playerScore[i] = 0;
        }
    }

    private void Update()
    {
        
    }

    public void SetSprite(int index, int type)
    {
        selectSprite[index].sprite = animationAsset.sprites[type];
    }

    public void SetMap(int index)
    {
        map.sprite = animationAsset.maps[index];
    }
}
