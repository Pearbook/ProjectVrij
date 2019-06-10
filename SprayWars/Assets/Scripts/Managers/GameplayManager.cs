using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    public AudioSource BGM;

    [Header("Score")]
    public int BlueScore;
    public int RedScore;
    //[HideInInspector]
    public float averageScore;
    public List<PlayerCanvas> AllCanvas = new List<PlayerCanvas>();

    [Header("Timer")]
    public int MaxTime;
    [HideInInspector]
    public float timer = 0;

    [HideInInspector]
    public bool timerIsOn;

    public bool GameHasStarted;


    private void Awake()
    {
        SpawnPlayer(1);
        SpawnPlayer(2);

        Cursor.visible = false;
    }

    private void Update()
    {
        if(timerIsOn)
            Timer();
        
    }

    void Timer()
    {
        if(timer <= MaxTime)
        {
            timer += Time.deltaTime;
            //seconds = (int)timer % 60;
        }
        else
        {
            timerIsOn = false;
            EndMatch();
        }
    }

    public void StartMatch()
    {
        GameManager.Player.EnableControl();
        GameHasStarted = true;
        timerIsOn = true;

        if(!BGM.isPlaying)
            BGM.Play();
    }

    void EndMatch()
    {
        GameManager.Player.DisableContol();
        
        for(int i = 0; i < AllCanvas.Count; i++)
        {
            RedScore += (int)AllCanvas[i].CheckForPixels().x / 100;
            BlueScore += (int)AllCanvas[i].CheckForPixels().y / 100;
        }
        
        averageScore = (RedScore + BlueScore) / 2;

        GameManager.UI.ShowScoreScreen();
    }

    public void SpawnPlayer(int id)
    {
        GameObject player = (GameObject)Instantiate(GameManager.Player.PlayerPrefab, SpawnPoints[id - 1].position, Quaternion.identity);

        player.name = "ent_player_" + id.ToString();

        GameManager.Player.AllPlayers.Add(player.GetComponent<PlayerProperties>());
        GameManager.Player.AllPlayers[id - 1].PlayerID = id;

        GameManager.Player.PlayerAudio.Add(player.GetComponent<PlayerAudio>());

        player.GetComponent<PlayerProperties>().PlayerGraphic[id - 1].SetActive(true);
    }
}
