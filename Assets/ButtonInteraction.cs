using Unity.VisualScripting;
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
    private int _oldVolume;
    private float _oldLength,_newLength;
    
    private float current_pressure;
    private int tab, uiState;
    private TextMeshProUGUI _pressureText;
  
    private LineRenderer _line;
    private Color paint_color;
    private Color_ current_color;
    private void Start()
    {
        _pressureText = GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        _ui = GameObject.Find("UIForVR");
        _uiCover = GameObject.Find("UICover");
        _colorButtons = GameObject.Find("ColorButtons");
        _canvasObj = GameObject.Find("Canvas");
        _saveAndLoadButtons = GameObject.Find("SaveAndLoad");
        _scrollButtons = GameObject.Find("ScrollButtons");
        _rakelLength = GameObject.Find("RakelLength").GetComponent<Slider>();
        _paintVolume = GameObject.Find("RakelVolume").GetComponent<Slider>();
        _parent = GameObject.Find("ColorButtons");
        _toggle = GameObject.Find("Checkbox").GetComponent<UnityEngine.UI.Toggle>();
        _oldVolume = 0;
        _oldLength = 0;
        _toggle.isOn = false;
        
        GetChildren();
        
        float current_pressure = _oilpaintengine.Config.InputConfig.RakelPressure.Value;
        
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString());
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        
        _text1 = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();
        _text2 = GameObject.Find("Text2").GetComponent<TextMeshProUGUI>();
        _text3 = GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();

        if (_text1.text == "New Text") { _text1.text = "Empty"; }
        if (_text2.text == "New Text") { _text2.text = "Empty"; }
        if (_text3.text == "New Text") { _text3.text = "Empty"; }
        
        Invoke(nameof(DisableAtStart), 0.1f);
    }

    //not needed i think
    private void Update()
    {
        current_pressure = _oilpaintengine.Config.InputConfig.RakelPressure.Value;
    }
    

    void DisableAtStart()
    {
            _saveAndLoadButtons.SetActive(false);
            _ui.SetActive(false);
            _uiCover.SetActive(false);
    }
    
    public void ClearRakel()
    {
        _oilpaintengine.ClearRakel();
    }

    public void SaveImg(int imgNum)
    {
        string textObject = "Text" + imgNum;
        Debug.Log("Save - Button Pressed");
        _oilpaintengine.SaveImg(imgNum);
        string message = "Image " + imgNum + " saved successfully";
        GameObject.Find(textObject).GetComponent<TextMeshProUGUI>().SetText(message);
    }

    public void LoadImg(int imgNum)
    {
        Debug.Log("Load - Button Pressed");
        _oilpaintengine.ClearCanvas();
        _oilpaintengine.LoadImg(imgNum);
    }

    
    public void Color(int color)
    {
        _oilpaintengine.UpdateFillColor((Color_)color);
        Debug.Log("Color Selected: " + (Color_)color);
        _oilpaintengine.FillApply();
        
        current_color = _oilpaintengine.GetCurrentColor();
        Vector3 colorVector = Colors.GetColor(current_color);
        paint_color = new Color(colorVector.x, colorVector.y, colorVector.z, 1.0f);
        
        _line.startColor = paint_color;
        _line.endColor = paint_color;
    }
    
    public void RakelLength(float length)
    {
        //_newLength = (int)rakelLength.GetComponent<Slider>().value;
        if (length != _oldLength)
        {
            if (length >2)
            {
                _oilpaintengine.UpdateRakelLength(length);
                _oldLength = length;
            }
        }
    }

    public void DeleteBuffer(bool state)
    {
        
        if (state == false)
        {
            state = true;
        }
        else
        {
            state = false;
        }
        
        if (!state)
        {
            _oilpaintengine.UpdateDeletePickedUpFromCSB(false);
            _toggle.isOn = false;

        }
        else
        {
            _oilpaintengine.UpdateDeletePickedUpFromCSB(true);
            _toggle.isOn = true;
        }
        
        Debug.Log("Current State: " + state);
        
    }
    
    public void PaintVolume(int volume)
    {
        if (volume != _oldVolume)
        {
            if (volume >60)
            {
                _oilpaintengine.UpdateFillVolume(volume);
                _oldVolume = volume;
            }
        }
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
        
            GameObject.Find("ColorButtons").transform.localPosition = new Vector3(PosX, _posY, PosZ);
            _startY -= Step;
            GetChildren();
        }
        else
        {
            _posY = _startY + Step;
        
            GameObject.Find("ColorButtons").transform.localPosition = new Vector3(PosX, _posY, PosZ);
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
        GameObject parent = GameObject.Find("ColorButtons");
        foreach (Transform child in parent.transform)
        {
            GameObject g = child.GameObject();
            
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


