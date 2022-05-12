using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CountDownController : MonoBehaviour
{
    public int countDownTime;
    public Text countdownDisplay;

    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countDownTime > 0)
        {
            countdownDisplay.text = countDownTime.ToString();

            yield return new WaitForSeconds(2f);

            countDownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false); // disable count down text
    }
}
