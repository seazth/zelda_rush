using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDie : MonoBehaviour
{
    public CanvasGroup canvas;
    public float lerpSpeed = 1f;

    private void Start()
    {
        canvas.alpha = 0;
    }
    private void Update()
    {
        canvas.alpha = Mathf.Lerp(canvas.alpha, 1, lerpSpeed);
    }
}
