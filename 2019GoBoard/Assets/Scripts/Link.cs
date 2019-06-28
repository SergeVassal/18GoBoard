using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] private float borderWidth;
    [SerializeField] private float lineThickness;
    [SerializeField] private float scaleTime;
    [SerializeField] private float delay;
    [SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
           

    public void DrawLine(Vector3 startPos,Vector3 endPos)
    {
        transform.localScale = new Vector3(lineThickness, 1f, 0f);

        Vector3 dirVector = endPos - startPos;
        float zScale = dirVector.magnitude - borderWidth * 2f;
        Vector3 newScale = new Vector3(lineThickness, 1f, zScale);

        transform.rotation = Quaternion.LookRotation(dirVector);
        transform.position = startPos + (transform.forward * borderWidth);

        iTween.ScaleTo(gameObject, iTween.Hash(
            "time",scaleTime,
            "scale",newScale,
            "easetype",easeType,
            "delay",delay            
            ));
    }
}
