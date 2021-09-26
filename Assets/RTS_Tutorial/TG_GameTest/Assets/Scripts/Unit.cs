using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public GameObject selectionSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ToggleSelectionVisual(bool makeVisible)
    {
        selectionSprite.SetActive(makeVisible);
        Debug.Log("La unidad está seleccionada: " + makeVisible);
    }
}
