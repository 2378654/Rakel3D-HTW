using JetBrains.Annotations;
using System.Collections;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEditor;
using Slider = UnityEngine.UI.Slider;
using UnityEngine.Experimental.Rendering;

public class ButtonInteraction : MonoBehaviour
{ 
    public Camera screenshotCamera;
    private OilPaintEngine _oilpaintengine;
    private GameObject _colorButtons;
    private GameObject _saveAndLoadButtons;
    private GameObject _ui;
    private GameObject _scrollButtons;
    private GameObject _uiCover;
    private GameObject _canvasObj;
    private UnityEngine.UI.Toggle _toggle;
    private TextMeshProUGUI _text1, _text2, _text3;
    private GameObject _parent;
    private LineRenderer _line;
    private Color paint_color;
    private Color_ current_color;
    private float current_pressure, PRESSURE;
    private int tab, uiState;
    private TextMeshProUGUI _pressureText;
    private GameObject _currentColor;
    private GameObject _clearTextObj;
    private TextMeshProUGUI _clearText, _clearTextOnWall;
    private float _currentVolume;
    private GameObject _settingsAfterSize;
    private GameObject _sizeObj;
    private Light _light;

    private bool _delete;
    //Fill Rakel
    private int _paintvolume, _oldPaintVolume = 0;
    private float _rakellength, _oldRakelLength = 0;
    private int _color, _oldColor = 0;
    
    private void Start()
    {
        _sizeObj = GameObject.Find("Size");
        _currentColor = GameObject.Find("Color");
        _pressureText = GameObject.Find("PressureTextOnWall").GetComponent<TextMeshProUGUI>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        _ui = GameObject.Find("UIForVR");
        _uiCover = GameObject.Find("UICover");
        _colorButtons = GameObject.Find("ColorButtons");
        _saveAndLoadButtons = GameObject.Find("SaveAndLoad");
        _scrollButtons = GameObject.Find("ScrollButtons");
        _parent = GameObject.Find("ColorButtons");
        _toggle = GameObject.Find("Checkbox").GetComponent<UnityEngine.UI.Toggle>();
        _toggle.isOn = false;
        _settingsAfterSize = GameObject.Find("SettingsAfterSize");
        _light =  GameObject.Find("Directional Light").GetComponent<Light>();
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        _text1 = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();
        _text2 = GameObject.Find("Text2").GetComponent<TextMeshProUGUI>();
        _text3 = GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();
        _clearTextObj = GameObject.Find("ClearText");
        _clearText = _clearTextObj.GetComponent<TextMeshProUGUI>();
        _clearTextOnWall = GameObject.Find("ClearTextOnWall").GetComponent<TextMeshProUGUI>();
        
        GetChildren();
        
        if (_text1.text == "New Text") { _text1.text = "Empty"; }
        if (_text2.text == "New Text") { _text2.text = "Empty"; }
        if (_text3.text == "New Text") { _text3.text = "Empty"; }
        Invoke(nameof(DisableAtStart), 0.1f);
    }
    private void Update()
    {
        //current_pressure = _oilpaintengine.Config.InputConfig.RakelPressure.Value;
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
        _settingsAfterSize.SetActive(false);
    }

    public void ClearRakelOnWall()
    {
        StartCoroutine(ClearingOnWall());
    }

    //Coroutine to show Rakel was cleared as an Indicator
    private IEnumerator ClearingOnWall()
    {
        _oilpaintengine.ClearRakel();
        _clearTextOnWall.SetText("Rakel Cleared");
        _line.startColor = UnityEngine.Color.white;
        _line.endColor = UnityEngine.Color.white;
        yield return new WaitForSeconds(1);
        _clearTextOnWall.SetText("");
    }
    
    
    public void ClearRakel()
    {
        StartCoroutine(Clearing());
    }

    //Coroutine to show Rakel was cleared as an Indicator
    private IEnumerator Clearing()
    {
        _oilpaintengine.ClearRakel();
        _clearText.SetText("Rakel Cleared");
        _line.startColor = UnityEngine.Color.white;
        _line.endColor = UnityEngine.Color.white;
        yield return new WaitForSeconds(1);
        _clearText.SetText("");
    }
    
    public void ClearCanvas()
    {
        _oilpaintengine.ClearCanvas();
    }
    
    
    
    public void UndoLastStroke()
    {
        Debug.Log("UNDO TRIGGERED");
        _oilpaintengine.UndoLastStroke();
    }
    
    private float currentWidthFirstImg, currentHeightFirstImg;
    private float currentWidthSecondImg, currentHeightSecondImg;
    private float currentWidthThirdImg, currentHeightThirdImg;
    private int formatAFirstImg, formatBFirstImg;
    private int formatASecondImg, formatBSecondImg;
    private int formatAThirdImg, formatBThirdImg;

    public void SaveImg(int imgNum)
    {
        StartCoroutine(SaveImgRoutine(imgNum));
    }

    
    private IEnumerator SaveImgRoutine(int imgNum)
    {
        string textObject = "Text" + imgNum;
        Debug.Log("Save - Button Pressed");
        _oilpaintengine.SaveImg(imgNum);
        string message = "Saved";
        GameObject.Find(textObject).GetComponent<TextMeshProUGUI>().SetText(message);
        
        //HACK: right now the first saved image has to be loaded instantly so the SaveAndLoad functions work correctly --> short pause but saves and loads work without a problem
        /*_saveCount++;

        if (_saveCount == 1)
        {
            _oilpaintengine.ClearCanvas();
            _oilpaintengine.LoadImg(imgNum);
        }*/

        //_ui.SetActive(false);
        //_uiCover.SetActive(false);
        
        yield return new WaitForEndOfFrame();
        string currentTime = System.DateTime.Now.ToString();
        currentTime = currentTime.Replace(" ", "");
        currentTime = currentTime.Replace(".", "");
        currentTime = currentTime.Replace(":", "");
        string path = $"Assets/SavedArtworks/{currentTime}Slot{imgNum}.png";
        //_canvasObj = GameObject.Find("Canvas");
        
        int rectWidth = (int)_oilpaintengine.Config.CanvasConfig.Width * _oilpaintengine.Config.TextureResolution;
        int rectHeight= (int)_oilpaintengine.Config.CanvasConfig.Height * _oilpaintengine.Config.TextureResolution;

        RenderTexture rt = new(rectWidth, rectHeight, GraphicsFormat.R8G8B8A8_SRGB, GraphicsFormat.None);
        rt.Create();
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;
        
        screenshotCamera.targetTexture = rt;
        screenshotCamera.backgroundColor = UnityEngine.Color.white;
        
        screenshotCamera.Render();

        Texture2D image = new Texture2D(rectWidth, rectHeight, TextureFormat.ARGB32, false,false);
        
        image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log("Screenshot saved");
        
        RenderTexture.active = currentRT;
        Destroy(image);
       
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate); 
        
        yield return new WaitForEndOfFrame();

        //_ui.SetActive(true);
        //_uiCover.SetActive(true);
    }
    
    public void ApplySize()
    {
        _sizeObj.SetActive(false);
        _settingsAfterSize.SetActive(true);
    }
  
    const int BaseFormatA = 3;
    const int BaseFormatB = 2;
    int width;
    int height;
    public void ChangeWidthOnController(int widthMult)
    {
        width = BaseFormatA * widthMult;
     
        int result = GetRatio(width, height);
        int newformatA = width / result;
        int newformatB = height / result;

        _oilpaintengine.UpdateWidth(width);
        _oilpaintengine.UpdateCanvasFormatA(newformatA);
        _oilpaintengine.UpdateCanvasFormatB(newformatB);
    }
 
    public void ChangeHeightOnController(int heightMult)
    {
        height = BaseFormatB * heightMult;
     
        int result = GetRatio(width, height);
        int newformatA = width / result;
        int newformatB = height / result;

        _oilpaintengine.UpdateHeight(height);
        _oilpaintengine.UpdateCanvasFormatA(newformatA);
        _oilpaintengine.UpdateCanvasFormatB(newformatB);
    }
    
    public void ChangeWidthOnWall(string direction)
    {
        int width = (int)_oilpaintengine.Config.CanvasConfig.Width;
        int height = (int)_oilpaintengine.Config.CanvasConfig.Height;

        if (direction == "plus")
        {
            width += BaseFormatA;
        }
        else
        {
            width -= BaseFormatA;
        }

        int result = GetRatio(width, height);
        int newformatA = width / result;
        int newformatB = height / result;

        _oilpaintengine.UpdateWidth(width);
        _oilpaintengine.UpdateCanvasFormatA(newformatA);
        _oilpaintengine.UpdateCanvasFormatB(newformatB);
    }

    public void ChangeHeightOnWall(string direction)
    {
        int width = (int)_oilpaintengine.Config.CanvasConfig.Width;
        int height = (int)_oilpaintengine.Config.CanvasConfig.Height;
        float orthographicSize = screenshotCamera.orthographicSize;
    
        if (direction == "plus")
        {
            height += BaseFormatB;
            orthographicSize += 1;
        }
        else
        {
            height -= BaseFormatB;
            orthographicSize -= 1;
        }

        int result = GetRatio(width, height);
        int newformatA = width / result;
        int newformatB = height / result;

        screenshotCamera.orthographicSize = orthographicSize;
        _oilpaintengine.UpdateHeight(height);
        _oilpaintengine.UpdateCanvasFormatA(newformatA);
        _oilpaintengine.UpdateCanvasFormatB(newformatB);
    }

    private static int GetRatio(int width, int height)
    {
        while (width != 0 && height != 0)
        {
            if (width > height)
                width %= height;
            else
                height %= width;
        }

        return width | height;
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

    public void ChangeRakelLengthOnWall(float length)
    {
        _oilpaintengine.UpdateRakelLength(length);
    }
    
    public void ChangeRakelVolumeOnWall(float volume)
    {
        _oilpaintengine.UpdateFillVolume((int)volume);
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
    
    public void DeleteBufferOnWall()
    {
        if (_delete == false)
        {
            _delete = true;
            _toggle.isOn = true;
        }
        else
        {
            _delete = false;
            _toggle.isOn = false;
        }
        
        _oilpaintengine.UpdateDeletePickedUpFromCSB(_delete);
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
        _canvasObj = GameObject.Find("Canvas");
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
            _canvasObj.transform.position = new Vector3(_canvasObj.transform.position.x, _canvasObj.transform.position.y, _canvasObj.transform.position.z-20);
            _uiCover.SetActive(false);
            _ui.SetActive(false);
        }
        else
        {
            _canvasObj.transform.position = new Vector3(_canvasObj.transform.position.x, _canvasObj.transform.position.y, 20); 
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
        current_pressure = pressure;
    }

    public float GetPressure()
    {
        return current_pressure;
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
            current_pressure = Mathf.Round(current_pressure * 10f) * 0.1f; //getting rid weird numbers from addition of float values
        }
        else
        {
            current_pressure = 1;
        }
        _oilpaintengine.UpdateRakelPressure(current_pressure);
        _pressureText.GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString());
    }

   

    public void DecreasePressure()
    {
        if (current_pressure > 0)
        {
            current_pressure -= 0.1f;
            current_pressure = Mathf.Round(current_pressure * 10f) * 0.1f; //getting rid weird numbers from addition of float values
        }
        else
        {
            current_pressure = 0;
        }
        _oilpaintengine.UpdateRakelPressure(current_pressure);
        _pressureText.GetComponent<TextMeshProUGUI>().SetText(current_pressure.ToString()); 
    }
}


