using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}

