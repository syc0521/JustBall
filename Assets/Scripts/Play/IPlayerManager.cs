using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerManager : MonoBehaviour
{
	public const int debugPlayers = 4;
	public GameObject playerPrefab;
	public const int maxPlayers = 4;
	public static List<IPlayer> players = new List<IPlayer>(maxPlayers);
	public int currentPlayer = 0;

	public void Start()
	{
		InputManager.OnDeviceDetached += OnDeviceDetached;
		OnStart();
	}

	public abstract void OnStart();

	public void Update()
	{
		var inputDevice = InputManager.ActiveDevice;
		if (JoinButtonWasPressedOnDevice(inputDevice))
		{
			if (ThereIsNoPlayerUsingDevice(inputDevice))
			{
				CreatePlayer(inputDevice);
			}
		}
		OnUpdate();
	}

	public abstract void OnUpdate();
	private bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
	{
		return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
	}

	private IPlayer FindPlayerUsingDevice(InputDevice inputDevice)
	{
		var playerCount = players.Count;
		for (var i = 0; i < playerCount; i++)
		{
			var player = players[i];
			if (player.Device == inputDevice)
			{
				return player;
			}
		}
		return null;
	}

	private bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
	{
		return FindPlayerUsingDevice(inputDevice) == null;
	}

	private void OnDeviceDetached(InputDevice inputDevice)
	{
		var player = FindPlayerUsingDevice(inputDevice);
		if (player != null)
		{
			RemovePlayer(player);
		}
	}

	public IPlayer CreatePlayer(InputDevice inputDevice)
	{
		if (players.Count < maxPlayers)
		{
			return OnCreatePlayer(inputDevice);
		}
		return null;
	}
	public abstract IPlayer OnCreatePlayer(InputDevice inputDevice);

	public void RemovePlayer(IPlayer player)
	{
		OnRemovePlayer(player);
	}

	public abstract void OnRemovePlayer(IPlayer player);

}
