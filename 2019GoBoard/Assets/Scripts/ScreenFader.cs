using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Color solidColor = Color.white;
    [SerializeField] private Color clearColor= new Color(1f,1f,1f,0f);

    private float delay = 0.1f;
    private float timeToFade = 1f;
    private iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    private MaskableGraphic graphic;


    private void Awake()
    {
        graphic = GetComponent<MaskableGraphic>();
    }

    private void UpdateColor(Color newColor)
    {
        graphic.color = newColor;
    }

    public void FadeOff()
    {
        iTween.ValueTo(gameObject,iTween.Hash(
            "from",solidColor,
            "to", clearColor,
            "time",timeToFade,
            "delay",delay,
            "easetype",easeType,
            "onupdatetarget",gameObject,
            "onupdate","UpdateColor"
            ));
    }

    public void FadeOn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", clearColor,
            "to", solidColor,
            "time", timeToFade,
            "delay", delay,
            "easetype", easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));
    }


}
