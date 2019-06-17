using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterfaceManager : MonoBehaviour
{
    [Header("Waiting Screen")]
    public CanvasGroup WaitScreen;
    public CanvasGroup TimesUp;

    [Header ("Score Screen")]
    public CanvasGroup ScoreScreen;
    public Text PlayerOneScore;
    public Text PlayerTwoScore;
    public RectTransform PlayerOnePanel;
    public RectTransform PlayerTwoPanel;
    public GameObject RedPlayerText;
    public GameObject BluePlayerText;

    public bool ScoreSceenActive;

    private int maxValue;
    private int minValue;
    List<int> allScore = new List<int>();

    [Header("Match")]
    public CanvasGroup MatchScreen;
    public List<Image> RedDots;
    public List<Image> BlueDots;
    [HideInInspector]
    public bool matchIsOpen;

    public void CloseWaitScreen()
    {
        WaitScreen.alpha = 0;
        WaitScreen.interactable = false;
        WaitScreen.blocksRaycasts = false;
    }

    public void ShowTimesUp()
    {
        TimesUp.alpha = 1;
    }

    public void ShowMatchScreen()
    {
        if (!matchIsOpen)
        {
            matchIsOpen = true;

            ScoreScreen.alpha = 0;
            ScoreScreen.interactable = false;
            ScoreScreen.blocksRaycasts = false;

            MatchScreen.alpha = 1;

            // Red wins
            if (GameManager.Gameplay.RedScore > GameManager.Gameplay.BlueScore)
            {
                if (GameManager.Gameplay.RedMatchPoints < 3)
                    GameManager.Gameplay.RedMatchPoints++;
            }
            // Blue wins
            if (GameManager.Gameplay.RedScore < GameManager.Gameplay.BlueScore)
            {
                if (GameManager.Gameplay.BlueMatchPoints < 3)
                    GameManager.Gameplay.BlueMatchPoints++;
            }

            for (int i = 0; i < GameManager.Gameplay.RedMatchPoints; ++i)
            {
                RedDots[i].color = new Color(RedDots[i].color.r, RedDots[i].color.g, RedDots[i].color.b, 255);
            }

            for (int i = 0; i < GameManager.Gameplay.BlueMatchPoints; ++i)
            {
                BlueDots[i].color = new Color(BlueDots[i].color.r, BlueDots[i].color.g, BlueDots[i].color.b, 255);
            }

            if (GameManager.Gameplay.RedMatchPoints == 3)
            {
                GameManager.Gameplay.redWins = true;
                return;
            }

            if (GameManager.Gameplay.BlueMatchPoints == 3)
            {
                GameManager.Gameplay.blueWins = true;
                return;
            }

        }
    }

    public void ShowScoreScreen()
    {
        ScoreScreen.alpha = 1;
        ScoreScreen.interactable = true;
        ScoreScreen.blocksRaycasts = true;

        TimesUp.alpha = 0;

        ScoreSceenActive = true;

        PlayerOneScore.text = GameManager.Gameplay.RedScore.ToString();
        PlayerTwoScore.text = GameManager.Gameplay.BlueScore.ToString();

        allScore.Add(GameManager.Gameplay.RedScore);
        allScore.Add(GameManager.Gameplay.BlueScore);

        maxValue = Mathf.Max(allScore.ToArray());
        minValue = Mathf.Min(allScore.ToArray());

        float panelSize = Custom.map(maxValue-minValue, -maxValue, maxValue, -850, 850);

        StartCoroutine(UpdateScorePanels(panelSize, 5));

        // Red wins
        if (GameManager.Gameplay.RedScore > GameManager.Gameplay.BlueScore)
            RedPlayerText.SetActive(true);
        // Blue wins
        if (GameManager.Gameplay.RedScore < GameManager.Gameplay.BlueScore)
            BluePlayerText.SetActive(true);

    }

    IEnumerator UpdateScorePanels(float target, float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            if (allScore[0] == maxValue)
            {
                PlayerOnePanel.offsetMax = Vector2.Lerp(PlayerOnePanel.offsetMax, new Vector2(-target, 0), (elapsedTime / time));
                PlayerTwoPanel.offsetMin = Vector2.Lerp(PlayerOnePanel.offsetMax, new Vector2(-target, 0), (elapsedTime / time));
            }

            if (allScore[1] == maxValue)
            {
                PlayerOnePanel.offsetMax = Vector2.Lerp(PlayerOnePanel.offsetMax, new Vector2(target, 0), (elapsedTime / time));
                PlayerTwoPanel.offsetMin = Vector2.Lerp(PlayerOnePanel.offsetMax, new Vector2(target, 0), (elapsedTime / time));
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
