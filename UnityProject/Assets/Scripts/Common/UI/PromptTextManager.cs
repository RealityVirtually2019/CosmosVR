using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptTextManager : MonoBehaviour
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }


    public void setTextVisible()
    {
        float r = text.color.r;
        float g = text.color.g;
        float b = text.color.b;
        float a = 1.0f;
        text.color = new Color(r, g, b, a);
    }

    public void setTextInivisble()
    {
        StartCoroutine(fadeOut());
    }
     
    IEnumerator fadeOut()
    {
        float fadeTime = 3.5f;
        float r = text.color.r;
        float g = text.color.g;
        float b = text.color.b;
        float initialAlpha = text.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            float a = Mathf.Lerp(initialAlpha, 0.0f, t);
            text.color = new Color(r, g, b, a);
            yield return null;
        }
        yield return null;
    }

}
