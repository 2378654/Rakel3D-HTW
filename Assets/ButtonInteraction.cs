using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonInteraction : MonoBehaviour
{
    public OilPaintEngine oilpaintengine;
    private GameObject _cadmiumGreen;
    public GameObject colorButtons;
    public GameObject saveAndLoadButtons;
    public GameObject scrollButtons;

    private void Start()
    {
        Invoke(nameof(DisableAtStart), 0.1f);
        _cadmiumGreen = GameObject.Find("0CadmiumGreen");
        GetChildren();
    }

    void DisableAtStart()
    {
        saveAndLoadButtons.SetActive(false);
    }

    public void SaveImg(int imgNum)
    {
        Texture2D texture = Resources.Load($"CanvasPNG{imgNum}") as Texture2D;
        Debug.Log("Save - Button Pressed");
        oilpaintengine.SaveImg(imgNum);
        GameObject.Find($"CanvasImg{imgNum}").GetComponent<Renderer>().material.mainTexture = texture;
        Resources.Load($"CanvasPNG{imgNum}");
    }

    public void LoadImg(int imgNum)
    {
        Debug.Log("Load - Button Pressed");
        oilpaintengine.ClearCanvas();
        oilpaintengine.LoadImg(imgNum);
    }
    public void Color(int color)
    {
        oilpaintengine.UpdateFillColor((Color_)color);
        Debug.Log("Color Selected: " + (Color_)color);
        oilpaintengine.FillApply();
    }
    
    private float _step = 0.84f;
    private static float _startY = -244.15f;
    private float _posY;
    
    public void Scroll(string direction)
    {
        if (direction == "Up")
        {
            _posY  = _startY - _step;
        
            GameObject.Find("ColorButtons").transform.localPosition = new Vector3(-494.8062f, _posY, 0f);
            _startY -= _step;
            GetChildren();
        }
        else
        {
            _posY = _startY + _step;
        
            GameObject.Find("ColorButtons").transform.localPosition = new Vector3(-494.8062f, _posY, 0f);
            _startY += _step;
            GetChildren();
        }

    }
    
    
    public void TabSelection(string tab)
    {
        if (tab == "Colors")
        {
            colorButtons.SetActive(true);
            saveAndLoadButtons.SetActive(false);
            scrollButtons.SetActive(true);
        }
        else
        {
            colorButtons.SetActive(false);
            saveAndLoadButtons.SetActive(true);
            scrollButtons.SetActive(false);
        }
    }

    public void GetChildren()
    {
        GameObject parent = GameObject.Find("ColorButtons");
        foreach (Transform child in parent.transform)
        {
            GameObject g = child.GameObject();
            
            // Check if Color Button are between scroll buttons
            if (4.5f > g.transform.position.y &&  g.transform.position.y > -3f) 
            {
                //Debug.Log(g.name);
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }
}


