using TMPro;
using UnityEngine;

public class Fps : MonoBehaviour
{
    [SerializeField]
    private TMP_Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1 / deltaTime;
        fpsText.text = string.Format("FPS: {0:0}", fps);
        if (fps < 15)
        {
            fpsText.color = Color.red;
        }
        else if (fps >= 15 && fps < 30)
        {
            fpsText.color = Color.yellow;
        }
        else if (fps >= 30)
        {
            fpsText.color = Color.green;
        }
    }
}
