using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterfaceManager : MonoBehaviour
{
    [Header("Height Slider")]
    public Slider HeightSlider;

    [Header("Score")]
    public List<Text> PlayerScore;

    private void Update()
    {
        UpdateSliderValue();
        UpdateScoreText();
    }

    void UpdateSliderValue()
    {
        //HeightSlider.value = Custom.map(GameManager.Level.LevelContainer.position.y, 0, -GameManager.Level.Behaviour.TowerHeight, 0, 100);
    }

    void UpdateScoreText()
    {
        for(int i = 0; i < 2; ++i)
        {
            //PlayerScore[i].text = GameManager.Player.AllPlayers[i].Score.ToString();
        }
    }
}
