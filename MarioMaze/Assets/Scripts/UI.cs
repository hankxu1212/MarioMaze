using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private int coinsCount = 0;
    
    private void Start()
    {
        Player.Instance.OnCoinCollected += InstanceOnOnCoinCollected;
    }

    private void InstanceOnOnCoinCollected()
    {
        coinsCount++;
        text.text = coinsCount.ToString();
    }
}