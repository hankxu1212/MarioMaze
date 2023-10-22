using System;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI timeLeft;
    [SerializeField] private float totalTime = 60;
    private int coinsCount = 0;
    
    private void Start()
    {
        text.text = "Coins Collected: 0";
        Player.Instance.OnCoinCollected += InstanceOnOnCoinCollected;
        Player.Instance.OnFinish += InstanceOnOnFinish;
    }

    private void Update()
    {
        totalTime -= Time.deltaTime;
        timeLeft.text = $"Time Left: {totalTime:0.00}";
        if (totalTime < 10)
            timeLeft.color = Color.red;

        if (totalTime < 0)
        {
            timeLeft.text = "You have used all your time. Try again next time!";
            Application.Quit();
        }
    }

    private void InstanceOnOnFinish()
    {
        text.text = $"Congratulations! You have finished the maze! Your score is: {coinsCount}.";
        Application.Quit();
    }

    private void InstanceOnOnCoinCollected()
    {
        coinsCount++;
        text.text = "Coins Collected: " + coinsCount;
    }
}