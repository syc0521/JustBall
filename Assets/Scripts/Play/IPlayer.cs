using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { A, B, C, D }
public abstract class IPlayer : MonoBehaviour
{
	public InputDevice Device { get; set; }
	public int PlayerType { get; set; }
    public int PlayerIndex { get; set; }

    public void CopyPlayer(IPlayer other)
    {
        other.Device = Device;
        other.PlayerType = PlayerType;
        other.PlayerIndex = PlayerIndex;
    }

}
