using DG.Tweening;
using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : IPlayerManager
{
	public GameObject playerRoot;
	public GameObject mainScene, choosePlayer, chooseMap, tutorial;
	public SelectManager selectManager;
	public override void OnStart()
	{
		players.Clear();
	}

	public override void OnUpdate()
	{
        if (players.Count == debugPlayers)//TODO == 4
        {
			if (players[0].Device.Command.WasPressed)
			{
                switch (SelectManager.state)
                {
					case 1:
						choosePlayer.SetActive(false);
						chooseMap.SetActive(true);
						SelectManager.state = 2;
						break;
					case 2:
						StartCoroutine(StartGame());
						break;
                    default:
                        break;
                }
                
			}
		}
	}

	private IEnumerator StartGame()
    {
		chooseMap.SetActive(false);
		tutorial.SetActive(true);
		yield return new WaitForSeconds(9.0f);
		LoadingManager.nextScene = "Play";
		SceneManager.LoadScene("Loading");
	}

	public override IPlayer OnCreatePlayer(InputDevice inputDevice)
	{
		var gameObject = Instantiate(playerPrefab);
		gameObject.transform.SetParent(playerRoot.transform);
		var player = gameObject.GetComponent<IPlayer>();
		player.PlayerType = 0;
		player.transform.position = new Vector3((players.Count % 2 == 0 ? -1 : 1) * 4, (players.Count > 1 ? -1 : 1) * 2);
		player.Device = inputDevice;
		player.PlayerIndex = currentPlayer;
		Debug.Log(currentPlayer);
		selectManager.selectSprite[currentPlayer].enabled = true;
		currentPlayer++;
		players.Add(player);
        if (players.Count == 1)
        {
			SelectManager.state = 1;
			mainScene.SetActive(false);
			choosePlayer.SetActive(true);
			playerRoot.SetActive(true);
		}
		return player;
	}

    public override void OnRemovePlayer(IPlayer player)
    {
		players.Remove(player);
		player.Device = null;
		Destroy(player.gameObject);
	}
}
