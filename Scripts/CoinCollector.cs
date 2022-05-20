using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // reference the UI i.e. Canvas

public class CoinCollector : MonoBehaviour
{
    public GameObject ScoreText;
    public int coinPoints = 0; // tally the number of points accumulated by the collecting coins

    void OnTriggerEnter(Collider other)
    {
        ScoreText.GetComponent<Text>().text = "Player Score: " + coinPoints;
    }

}
