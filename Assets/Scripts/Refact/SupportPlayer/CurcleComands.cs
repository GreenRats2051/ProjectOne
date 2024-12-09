using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurcleComands : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field:SerializeField] public bool istrue {  get;private set; }
    [field:SerializeField] public Comand comand {  get;private set; }

    public void Reset()
    {
        istrue = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        istrue = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        istrue = false;
    }

 
}
