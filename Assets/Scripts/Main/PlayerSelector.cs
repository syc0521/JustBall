using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : IPlayer
{
    private SelectManager selectManager;


    private void Start()
    {
        selectManager = GameObject.Find("SelectManager").GetComponent<SelectManager>();
    }
    private void Update()
    {
        if (SelectManager.state == 1)
        {
            if (Device.LeftStickRight.WasPressed || Device.DPadRight.WasPressed)
            {
                PlayerType = (PlayerType + 1) % 5;
                selectManager.SetSprite(PlayerIndex, PlayerType);
            }
            if (Device.LeftStickLeft.WasPressed || Device.DPadLeft.WasPressed)
            {
                PlayerType = (PlayerType + 4) % 5;
                selectManager.SetSprite(PlayerIndex, PlayerType);
            }
        }
        else if (SelectManager.state == 2)
        {
            if (Device.LeftStickRight.WasPressed || Device.DPadRight.WasPressed)
            {
                selectManager.mapIndex = (selectManager.mapIndex + 1) % 5;
                selectManager.SetMap(selectManager.mapIndex);
            }
            if (Device.LeftStickLeft.WasPressed || Device.DPadLeft.WasPressed)
            {
                selectManager.mapIndex = (selectManager.mapIndex + 4) % 5;
                selectManager.SetMap(selectManager.mapIndex);
            }
        }
        
    }


}
