using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonInteraction : MonoBehaviour
{
    public OilPaintEngine oilpaintengine;
    public GameObject colorButtons;
    public GameObject saveAndLoadButtons;
    public GameObject scrollButtons;
    private LineRenderer _line;

    private Color paint_color;
    private Color_ current_color;
    private float current_pressure;
    private void Start()
    {
        Invoke(nameof(DisableAtStart), 0.1f);
        
        GetChildren();
        float current_pressure = oilpaintengine.Config.InputConfig.RakelPressure.Value;
        
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString());
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        
        TextMeshProUGUI text1, text2, text3;
        text1 = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();
        text2 = GameObject.Find("Text2").GetComponent<TextMeshProUGUI>();
        text3 = GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();

        if (text1.text == "New Text") { text1.text = "Empty"; }
        if (text2.text == "New Text") { text2.text = "Empty"; }
        if (text3.text == "New Text") { text3.text = "Empty"; }
    }

    private void Update()
    {
        current_pressure = oilpaintengine.Config.InputConfig.RakelPressure.Value;
    }
    

    void DisableAtStart()
    {
        saveAndLoadButtons.SetActive(false);
    }

    public void SaveImg(int imgNum)
    {
        string textObject = "Text" + imgNum;
        //Texture2D texture = Resources.Load($"CanvasPNG{imgNum}") as Texture2D;
        Debug.Log("Save - Button Pressed");
        oilpaintengine.SaveImg(imgNum);
        //GameObject.Find($"CanvasImg{imgNum}").GetComponent<Renderer>().material.mainTexture = texture;
       // Resources.Load($"CanvasPNG{imgNum}");
       string message = "Image " + imgNum + " saved successfully";
       GameObject.Find(textObject).GetComponent<TextMeshProUGUI>().SetText(message);
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
        
        current_color = oilpaintengine.GetCurrentColor();
        Vector3 colorVector = Colors.GetColor(current_color);
        paint_color = new Color(colorVector.x, colorVector.y, colorVector.z, 1.0f);
        
        _line.startColor = paint_color;
        _line.endColor = paint_color;
    }
    
    private float _step = 0.84f;
    private static float _startY = -245.77f;
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
    
    int tab = 0;
    public void TabSelection()
    {
        if (tab == 0)
        {
            tab = 1;
        }
        else
        {
            tab = 0;
        }

        if (tab == 0)
        {
            colorButtons.SetActive(true);
            saveAndLoadButtons.SetActive(false);
            scrollButtons.SetActive(true);
            
        }
        
        if (tab == 1)
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
            if (2f > g.transform.position.y &&  g.transform.position.y > -5f) 
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

    public void IncreasePressure()
    {
        if (current_pressure < 1)
        {
            current_pressure += 0.1f;
            current_pressure = Mathf.Round(current_pressure * 10f) * 0.1f;
        }
        else
        {
            current_pressure = 1;
        }
        oilpaintengine.UpdateRakelPressure(current_pressure);
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString());
    }
    
    public void DecreasePressure()
    {
        if (current_pressure > 0)
        {
            current_pressure -= 0.1f;
            current_pressure = Mathf.Round(current_pressure * 10f) * 0.1f;
        }
        else
        {
            current_pressure = 0;
        }
        oilpaintengine.UpdateRakelPressure(current_pressure);
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString()); 
    }
}


