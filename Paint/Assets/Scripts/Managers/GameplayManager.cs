using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    public int ScorePerSpray;
    public int ScorePerDeath;

    private void Awake()
    {
        SpawnPlayer(1);
        SpawnPlayer(2);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void SpawnPlayer(int id)
    {
        GameObject player = (GameObject)Instantiate(GameManager.Player.PlayerPrefab, SpawnPoints[id - 1].position, Quaternion.identity);

        player.name = "ent_player_" + id.ToString();

        GameManager.Player.AllPlayers.Add(player.GetComponent<PlayerProperties>());
        GameManager.Player.AllPlayers[id - 1].PlayerID = id;

        player.GetComponent<PlayerProperties>().PlayerGraphic[id - 1].SetActive(true);
    }
}
