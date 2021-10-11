using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public AnimationAsset animationAsset;
    public GameObject playerPrefab;
	public static List<Player> playerList = new List<Player>();
    public Transform[] playerPositions;
    private int[] positionFlag = { 0, 0, 0, 0 };
    public GameObject boat;
    public PlayerScoreAsset playerScore;
    public static int[] players = { 1, 1, 1, 1 };
    public static int playerCount = IPlayerManager.debugPlayers;//TODO 4
    public LevelAsset levelAssets;
    public static int currentStage = 0;
    public Slider[] playerScoreSlider;
    public Slider[] playerLife;
    public GameObject gameCanvas, winCanvas;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject countDown;
    public static bool isFirst = true;
    public static bool GameStart { get; set; }
    private bool gameEnd = false;

    public Sprite[] selectHeadSprite;
    public Image[] headSprite;

    private void Start()
    {
        currentStage++;
        InitializeLevel();
        InitializePlayers();
        StartCoroutine(StartGame());
        gameEnd = false;
        GameStart = false;
        playerCount = IPlayerManager.debugPlayers;// TODO 4
        for (int i = 0; i < 4; i++)
        {
            playerScoreSlider[i].value = playerScore.playerScore[i];
            playerLife[i].value = playerScore.playerHealth[i];

            headSprite[i].sprite = selectHeadSprite[playerList[i].PlayerType];
        }
    }
    private void Update()
    {
        if (GameStart)
        {
            for (int i = 0; i < 4; i++)
            {
                playerScoreSlider[i].value = playerScore.playerScore[i];
                playerLife[i].value = playerScore.playerHealth[i];
            }
        }
        if (playerCount <= 1 && GameStart)
        {
            GameEnd();
            GameStart = false;
            gameEnd = true;
        }
        if (gameEnd)
        {
            if (IPlayerManager.players[0].Device.Command.WasPressed)
            {
                int totalWinner = CheckTotalWinner();
                if (totalWinner == -1)
                {
                    LoadingManager.nextScene = "Play";
                    SceneManager.LoadScene("Loading");
                }
                else
                {
                    LoadingManager.nextScene = "Result";//TODO Result
                    SceneManager.LoadScene("Loading");
                    ResultManager.totalWinner = playerList[totalWinner].PlayerType;
                }
               
            }
        }
    }

    private void InitializeLevel()
    {
        Instantiate(levelAssets.levels[LoadingManager.nextLevel]);
        for (int i = 0; i < playerScore.playerHealth.Length; i++)
        {
            playerScore.playerHealth[i] = 5;//todo 5
        }
    }
    private void InitializePlayers()
    {
        playerList.Clear();
        for (int i = 0; i < IPlayerManager.players.Count; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, 4);
            } while (positionFlag[index] == 1);
            positionFlag[index] = 1;
            IPlayer player = PlayerManager.players[i];
            var playerObj = Instantiate(playerPrefab, playerPositions[index].position, Quaternion.identity).GetComponent<Player>();
            player.CopyPlayer(playerObj);
            playerObj.gameController = this;
            if (isFirst)
            {
                playerList.Add(playerObj);
            }
            else { Debug.Log(playerList.Count); }
        }
    }

    public void ShakeCamera()
	{
        virtualCamera.transform.DOKill(true);
        virtualCamera.transform.DOShakePosition(0.4f, strength: 1.1f);
        //boat.transform.DOKill(true);
        //boat.transform.DOShakePosition(.2f, strength: .3f);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3.5f);
        countDown.SetActive(false);
        GameStart = true;
    }

    private void GameEnd()
    {
        //winCanvas.SetActive(true);
        int winner = CheckWinner();
        if (winner != -1)
        {
            Debug.Log(playerList[winner].transform);
            virtualCamera.Follow = playerList[winner].transform;
            
            playerScore.playerScore[winner]++;
            Debug.Log(winner);
        }
    }

    private int CheckWinner()
    {
        /*for (int i = 0; i < players.Length; i++)
        {
            if (i == 1)
            {
                return i;
            }
        }*/
        for (int i = 0; i < playerScore.playerHealth.Length; i++)
        {
            if (playerScore.playerHealth[i] > 0)
            {
                return i;
            }
        }
        return -1;
    }

    private int CheckTotalWinner()
    {
        for (int i = 0; i < playerScore.playerScore.Length; i++)
        {
            if (playerScore.playerScore[i] >= 3)
            {
                return i;
            }
        }
        return -1;
    }
}
