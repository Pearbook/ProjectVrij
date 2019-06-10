using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterfaceManager : MonoBehaviour
{
    [Header("Waiting Screen")]
    public CanvasGroup WaitScreen;

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

    public void CloseWaitScreen()
    {
        WaitScreen.alpha = 0;
        WaitScreen.interactable = false;
        WaitScreen.blocksRaycasts = false;
    }

    public void ShowScoreScreen()
    {
        ScoreScreen.alpha = 1;
        ScoreScreen.interactable = true;
        ScoreScreen.blocksRaycasts = true;

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
