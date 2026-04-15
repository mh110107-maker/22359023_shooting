using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI nowScoreUI;
    public int nowScore;

    public TextMeshProUGUI bestScoreUI;
    public int bestScore;

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("bestscore");
        bestScoreUI.text = "Best score : " + bestScore;
    }
}
