using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public SpriteRenderer win;
    public AnimationAsset animationAsset;
    public static int totalWinner = 0;
    private void Start()
    {
        win.sprite = animationAsset.sprites[totalWinner];
    }
    void Update()
    {
        if (IPlayerManager.players[0].Device.CommandWasPressed)
        {
            LoadingManager.nextScene = "Input";
            SceneManager.LoadScene("Loading");
        }
    }
}
