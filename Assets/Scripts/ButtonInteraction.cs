using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using TMPro;
using Slider = UnityEngine.UI.Slider;


public class ButtonInteraction : MonoBehaviour
{ 
    private OilPaintEngine _oilpaintengine;
    private GameObject _colorButtons;
    private GameObject _saveAndLoadButtons;
    private GameObject _ui;
    private GameObject _scrollButtons;
    private Slider _rakelLength,_paintVolume;
    private GameObject _uiCover;
    private GameObject _canvasObj;
    private UnityEngine.UI.Toggle _toggle;
    private TextMeshProUGUI _text1, _text2, _text3;
    private GameObject _parent;
    private LineRenderer _line;
    private Color paint_color;
    private Color_ current_color;
    private bool _delete;
    private float current_pressure, PRESSURE;
    private int tab, uiState;
    private TextMeshProUGUI _pressureText;
    private int _saveCount;
    private GameObject _currentColor;
    private GameObject _clearTextObj;
    private TextMeshProUGUI _clearText;
    private float _currentVolume;
    
    //Fill Rakel
    private int _paintvolume, _oldPaintVolume = 0;
    private float _rakellength, _oldRakelLength = 0;
    private int _color, _oldColor = 0;
    
    private void Start()
    {
        _currentColor = GameObject.Find("Color");
        _pressureText = GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        _ui = GameObject.Find("UIForVR");
        _uiCover = GameObject.Find("UICover");
        _colorButtons = GameObject.Find("ColorButtons");
        _saveAndLoadButtons = GameObject.Find("SaveAndLoad");
        _scrollButtons = GameObject.Find("ScrollButtons");
        _rakelLength = GameObject.Find("RakelLength").GetComponent<Slider>();
        _paintVolume = GameObject.Find("RakelVolume").GetComponent<Slider>();
        _parent = GameObject.Find("ColorButtons");
        _delete = false;
        _toggle = GameObject.Find("Checkbox").GetComponent<UnityEngine.UI.Toggle>();
        _saveCount = 0;
        _toggle.isOn = false;
        
        GetChildren();
        
        float current_pressure = _oilpaintengine.Config.InputConfig.RakelPressure.Value;
        
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString());
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        
        
        _text1 = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();
        _text2 = GameObject.Find("Text2").GetComponent<TextMeshProUGUI>();
        _text3 = GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();
        _clearTextObj = GameObject.Find("ClearText");
        _clearText = _clearTextObj.GetComponent<TextMeshProUGUI>();

        if (_text1.text == "New Text") { _text1.text = "Empty"; }
        if (_text2.text == "New Text") { _text2.text = "Empty"; }
        if (_text3.text == "New Text") { _text3.text = "Empty"; }
        Invoke(nameof(DisableAtStart), 0.1f);
    }
    private void Update()
    {
        _canvasObj = GameObject.Find("Canvas");
        current_pressure = _oilpaintengine.Config.InputConfig.RakelPressure.Value;
        _oilpaintengine.Rakel.Reservoir.PaintGrid.ReadbackContent();
        _currentVolume = _oilpaintengine.Rakel.Reservoir.PaintGrid.ContentData[0].Volume;
        _line.startColor = UnityEngine.Color.Lerp(UnityEngine.Color.white, paint_color, _currentVolume);
        _line.endColor = UnityEngine.Color.Lerp(UnityEngine.Color.white, paint_color, _currentVolume);
    }
    
    
    void DisableAtStart()
    {
        _ui.SetActive(false);
        _uiCover.SetActive(false);
        _saveAndLoadButtons.SetActive(false);
    }

    public void ClearRakel()
    {
        StartCoroutine(Clearing());
    }

    public void ClearCanvas()
    {
        _oilpaintengine.ClearCanvas();
    }
    
    private IEnumerator Clearing()
    {
        _oilpaintengine.ClearRakel();
        _clearText.SetText("Rakel Cleared");
        _line.startColor = UnityEngine.Color.white;
        _line.endColor = UnityEngine.Color.white;
        yield return new WaitForSeconds(1);
        _clearText.SetText("");
    }
    
    /*public void ChangeSize(int mult)
    {
        int width = (int)_oilpaintengine.Config.CanvasConfig.Width;
        int height = (int)_oilpaintengine.Config.CanvasConfig.Height;
        float currentSize = screenshotCamera.orthographicSize;
        width = _oilpaintengine.Config.CanvasConfig.FormatA * mult;
        height = _oilpaintengine.Config.CanvasConfig.FormatB * mult;
        currentSize = mult;
        screenshotCamera.orthographicSize = currentSize;
        _oilpaintengine.UpdateSize(height,width);
    }
    

    */
    public void UndoLastStroke()
    {
        Debug.Log("UNDO TRIGGERED");
        _oilpaintengine.UndoLastStroke();
    }
    
    public void SaveImg(int imgNum)
    {
        string textObject = "Text" + imgNum;
        Debug.Log("Save - Button Pressed");
        _oilpaintengine.SaveImg(imgNum);
        string message = "Saved";
        GameObject.Find(textObject).GetComponent<TextMeshProUGUI>().SetText(message);
        
        
        //HACK: right now the first load has to be triggered so the SaveAndLoad functions work correctly
        _saveCount++;

        if (_saveCount == 1)
        {
            _oilpaintengine.ClearCanvas();
            _oilpaintengine.LoadImg(imgNum);
        }

    }
    
    public void LoadImg(int imgNum)
    {
        Debug.Log("Load - Button Pressed");
        _oilpaintengine.ClearCanvas();
        _oilpaintengine.LoadImg(imgNum);
    }
    public void Color(int color)
    {
        Debug.Log("Color Selected: " + (Color_)color);
        _color = color;
        
        ApplyRakelSettings();
        
        current_color = _oilpaintengine.GetCurrentColor();
        Vector3 colorVector = Colors.GetColor(current_color);
        paint_color = new Color(colorVector.x, colorVector.y, colorVector.z, 1.0f);
        
        _line.startColor = paint_color;
        _line.endColor = paint_color;
        
    }


    public void RakelLength(float length)
    {
        _rakellength = length;
        ApplyRakelSettings();
    }
    
    public void DeleteBuffer(bool state)
    {
        _oilpaintengine.UpdateDeletePickedUpFromCSB(state);
    }
    

    public void PaintVolume(int volume)
    {
        _paintvolume = volume;
        ApplyRakelSettings();
    }

    public void ApplyRakelSettings()
    {
        Vector3 colorVector = Colors.GetColor((Color_)_color);
        if (_paintvolume != _oldPaintVolume)
        {
            _oilpaintengine.UpdateFillVolume(_paintvolume);
            _oldPaintVolume = _paintvolume;
        }

        if (_rakellength != _oldRakelLength)
        {
            _oilpaintengine.UpdateRakelLength(_rakellength);
            _oldRakelLength = _rakellength;
        }

        if (_color != _oldColor)
        {
            _oilpaintengine.UpdateFillColor((Color_)_color);
            _oldColor = _color;
            _currentColor.GetComponent<Renderer>().material.color = new Color(colorVector.x, colorVector.y, colorVector.z);
        }
        _line.startColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        _line.endColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        
        _oilpaintengine.FillApply();
    }


    private const float Step = 0.84f;
    private const float PosZ = -0.15f;
    private const float PosX = -494.8062f;
    private static float _startY = -245.77f;
    private float _posY;
    
    public void Scroll(string direction)
    {

        if (direction == "Up")
        {
            _posY  = _startY - Step;
        
            _colorButtons.transform.localPosition = new Vector3(PosX, _posY, PosZ);
            _startY -= Step;
            GetChildren();
        }

        if (direction == "Down")
        {
            _posY = _startY + Step;
        
            _colorButtons.transform.localPosition = new Vector3(PosX, _posY, PosZ);
            _startY += Step;
            GetChildren();
        }

    }
    
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
            _colorButtons.SetActive(true);
            _saveAndLoadButtons.SetActive(false);
            _scrollButtons.SetActive(true);
            
        }
        
        if (tab == 1)
        {
            _colorButtons.SetActive(false);
            _saveAndLoadButtons.SetActive(true);
            _scrollButtons.SetActive(false);
        }
    }
    
    public void ShowUI()
    {
        
        if (uiState == 0)
        {
            uiState = 1;
        }
        else
        {
            uiState = 0;
        }

        if (uiState == 0)
        {
            _uiCover.SetActive(false);
            _ui.SetActive(false);
        }
        else
        {
            _uiCover.SetActive(true);
            _ui.SetActive(true);
            
        }
    }

    public void GetChildren()
    {
        foreach (Transform child in _parent.transform)
        {
            GameObject g = child.gameObject;
            
            // Check if Color Button are between scroll buttons
            if (-1f > g.transform.position.y &&  g.transform.position.y > -6f) 
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }
    
    public void Pressure(float pressure)
    {
        pressure = Mathf.Clamp01(pressure/1500);
        //_oilpaintengine.Config.InputConfig.RakelPressure.Value = pressure;
        //_pressureText.SetText(pressure.ToString());
        PRESSURE = pressure;
    }

    public float GetPressure()
    {
        return PRESSURE;
    }
    
    public float GetLength()
    {
        return _rakellength;
    }
    
    public int GetCurrentColor()
    {
        return _color;
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
        _oilpaintengine.UpdateRakelPressure(current_pressure);
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
        _oilpaintengine.UpdateRakelPressure(current_pressure);
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString()); 
    }
}


