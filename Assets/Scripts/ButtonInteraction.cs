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
    public bool wallController;
    public Camera screenshotCamera;
    private OilPaintEngine _oilPaintEngine;
    private GameObject _colorButtons;
    private GameObject _saveAndLoadButtons;
    private GameObject _ui;
    private GameObject _scrollButtons;
    private GameObject _uiCover;
    private GameObject _canvasObj;
    private UnityEngine.UI.Toggle _toggle;
    private TextMeshProUGUI _savedImageText1, _savedImageText2, _savedImageText3;
    private LineRenderer _line;
    private Color _paintColor;
    private Color_ _currentColor;
    private float _currentPressure;
    private bool _tab, _uiState;
    private TextMeshProUGUI _pressureText;
    private GameObject _currentColorObj;
    private GameObject _clearTextObj;
    private TextMeshProUGUI _clearText, _clearTextOnWall;
    private float _currentVolume;
    private GameObject _settingsAfterSize;
    private GameObject _sizeObj;
    private GameObject _sizeText;
    private bool _deleteFromBuffer;
    public bool uiActive;

    private GameObject _wallButtons;
    //Fill Rakel
    private int _paintvolume, _oldPaintVolume;
    private float _rakellength, _oldRakelLength;
    private int _color = -1;
    private int _oldColor = -2;
    
    private int _canvasWidth = 30;
    private int _canvasHeight = 20;
    
    
    //ColorObject Position
    private float _posXForColorObject, _posYForColorObject;
    
    private void Start()
    {
        _sizeObj = GameObject.Find("Size");
        _sizeText = GameObject.Find("AdjustSizeText");
        _currentColorObj = GameObject.Find("Color");
        _pressureText = GameObject.Find("PressureTextOnWall").GetComponent<TextMeshProUGUI>();
        _oilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        _ui = GameObject.Find("UIForVR");
        _uiCover = GameObject.Find("UICover");
        _colorButtons = GameObject.Find("ColorButtons");
        _saveAndLoadButtons = GameObject.Find("SaveAndLoad");
        _scrollButtons = GameObject.Find("ScrollButtons");
        _toggle = GameObject.Find("Checkbox").GetComponent<UnityEngine.UI.Toggle>();
        _toggle.isOn = false;
        _settingsAfterSize = GameObject.Find("SettingsAfterSize");
        _wallButtons = GameObject.Find("WallButtons");
        
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        
        //UI texts for saved Images
        _savedImageText1 = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();
        _savedImageText2 = GameObject.Find("Text2").GetComponent<TextMeshProUGUI>();
        _savedImageText3 = GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();
        
        _clearTextObj = GameObject.Find("ClearText");
        _clearText = _clearTextObj.GetComponent<TextMeshProUGUI>();
        _clearTextOnWall = GameObject.Find("ClearTextOnWall").GetComponent<TextMeshProUGUI>();
        
        GetChildren();
        
        if (_savedImageText1.text == "New Text") { _savedImageText1.text = "Empty"; }
        if (_savedImageText2.text == "New Text") { _savedImageText2.text = "Empty"; }
        if (_savedImageText3.text == "New Text") { _savedImageText3.text = "Empty"; }
        Invoke(nameof(DisableAtStart), 0.1f);
        
        _posXForColorObject = _colorButtons.transform.localPosition.x;
        _posYForColorObject = _colorButtons.transform.localPosition.y;

    }
    
    
    void DisableAtStart()
    {
        _ui.SetActive(false);
        _uiCover.SetActive(false);
        _saveAndLoadButtons.SetActive(false);
        _settingsAfterSize.SetActive(false); 

        if (!wallController)
        {
            _sizeObj.SetActive(false);
            _wallButtons.SetActive(false);
        }
        else
        {
            _sizeObj.SetActive(true);
            _wallButtons.SetActive(true);
        }
    }

    public void ClearRakelOnWall()
    {
        _oilPaintEngine.ClearRakel();
        StartCoroutine(ClearingOnWall());
    }

    //Coroutine to show Rakel was cleared as an Indicator
    private IEnumerator ClearingOnWall()
    {
        _clearTextOnWall.SetText("Rakel Cleared");
        _line.startColor = UnityEngine.Color.white;
        _line.endColor = UnityEngine.Color.white;
        yield return new WaitForSeconds(1);
        _clearTextOnWall.SetText("");
    }
    
    
    public void ClearRakel()
    {
        _oilPaintEngine.ClearRakel();
        StartCoroutine(Clearing());
    }

    //Coroutine to show Rakel was cleared as an Indicator
    private IEnumerator Clearing()
    {
        _clearText.SetText("Rakel Cleared");
        _line.startColor = UnityEngine.Color.white;
        _line.endColor = UnityEngine.Color.white;
        yield return new WaitForSeconds(1);
        _clearText.SetText("");
    }
    
    private IEnumerator ClearingCanvasOnWall()
    {   
        _clearTextOnWall.SetText("Canvas Cleared");
        yield return new WaitForSeconds(1);
        _clearTextOnWall.SetText("");
    }
    
    public void ClearCanvas()
    {
        Debug.Log("Cleared Canvas");
        _oilPaintEngine.ClearCanvas();
    }
    
    public void ClearCanvasOnWall()
    {
        _oilPaintEngine.ClearCanvas();
        StartCoroutine(ClearingCanvasOnWall());
    }
    
    public void UndoLastStroke()
    {
        Debug.Log("UNDO TRIGGERED");
        _oilPaintEngine.UndoLastStroke();
    }

    public void SaveImg(int imgNum)
    {
        StartCoroutine(SaveImgRoutine(imgNum));
    }
    
    private IEnumerator SaveImgRoutine(int imgNum)
    {
        string textObject = "Text" + imgNum;
        Debug.Log("Save - Button Pressed");
        _oilPaintEngine.SaveImg(imgNum);
        string message = "Saved";
        GameObject.Find(textObject).GetComponent<TextMeshProUGUI>().SetText(message);
        
        yield return new WaitForEndOfFrame();
        string currentTime = System.DateTime.Now.ToString();
        currentTime = currentTime.Replace(" ", "");
        currentTime = currentTime.Replace(".", "");
        currentTime = currentTime.Replace(":", "");
        string path = $"Assets/SavedArtworks/{currentTime}Slot{imgNum}.png";
        _canvasObj = GameObject.Find("Canvas");

        Debug.Log("Width x Height: " + _oilPaintEngine.Config.CanvasConfig.Width + " x " + _oilPaintEngine.Config.CanvasConfig.Height);
        
        int rectWidth = (int)_oilPaintEngine.Config.CanvasConfig.Width * _oilPaintEngine.Config.TextureResolution;
        int rectHeight = (int)_oilPaintEngine.Config.CanvasConfig.Height * _oilPaintEngine.Config.TextureResolution;

        RenderTexture rt = new(rectWidth, rectHeight, GraphicsFormat.R8G8B8A8_SRGB, GraphicsFormat.None);
        rt.Create();
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;
        
        screenshotCamera.targetTexture = rt;
        screenshotCamera.backgroundColor = UnityEngine.Color.clear;
        
        //Changing the shader of the plane for the screenshot to avoid reflections of the directional light
        //_canvasObj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Color");
        screenshotCamera.Render();
        //_canvasObj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
        
        Texture2D image = new Texture2D(rectWidth, rectHeight, TextureFormat.ARGB32, false,false);
        
        image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log("Screenshot saved");
        
        RenderTexture.active = currentRT;
        Destroy(image);
        //Destroy(rt);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate); 
        
        yield return new WaitForEndOfFrame();
        
    }
    
    public void ApplySize()
    {
        _sizeObj.SetActive(false);
        _sizeText.SetActive(false);
        _settingsAfterSize.SetActive(true);
    }
  
    const int BaseFormatA = 3;
    const int BaseFormatB = 2;
    
    public void ChangeWidthOnController(int widthMult)
    {
        _canvasWidth = BaseFormatA * widthMult;
     
        int result = GetRatio(_canvasWidth, _canvasHeight);
        int newformatA = _canvasWidth / result;
        int newformatB = _canvasHeight / result;

        _oilPaintEngine.UpdateWidth(_canvasWidth);
        _oilPaintEngine.UpdateCanvasFormatA(newformatA);
        _oilPaintEngine.UpdateCanvasFormatB(newformatB);
    }
 
    public void ChangeHeightOnController(int heightMult)
    {
        _canvasHeight = BaseFormatB * heightMult;
        float orthographicSize = screenshotCamera.orthographicSize;
        orthographicSize = heightMult - 2;
        int result = GetRatio(_canvasWidth, _canvasHeight);
        int newformatA = _canvasWidth / result;
        int newformatB = _canvasHeight / result;

        screenshotCamera.orthographicSize = orthographicSize;
        _oilPaintEngine.UpdateHeight(_canvasHeight);
        _oilPaintEngine.UpdateCanvasFormatA(newformatA);
        _oilPaintEngine.UpdateCanvasFormatB(newformatB);
    }
    
    public void ChangeWidthOnWall(string direction)
    {
        int width = (int)_oilPaintEngine.Config.CanvasConfig.Width;
        int height = (int)_oilPaintEngine.Config.CanvasConfig.Height;

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

        _oilPaintEngine.UpdateWidth(width);
        _oilPaintEngine.UpdateCanvasFormatA(newformatA);
        _oilPaintEngine.UpdateCanvasFormatB(newformatB);
    }

    public void ChangeHeightOnWall(string direction)
    {
        int width = (int)_oilPaintEngine.Config.CanvasConfig.Width;
        int height = (int)_oilPaintEngine.Config.CanvasConfig.Height;
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
        _oilPaintEngine.UpdateHeight(height);
        _oilPaintEngine.UpdateCanvasFormatA(newformatA);
        _oilPaintEngine.UpdateCanvasFormatB(newformatB);
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
    
        _oilPaintEngine.ClearCanvas();
        _oilPaintEngine.LoadImg(imgNum);
    }

  
    public void Color(int color)
    {
        _color = color;
        _currentColor = _oilPaintEngine.GetCurrentColor();
        ApplyRakelSettings();
        
    }
    
    public void ColorOnWall(int color)
    {
        _color = color;
        _currentColor = (Color_)_color;
        
        Vector3 colorVector = Colors.GetColor(_currentColor);
        if (_color != _oldColor)
        {
            Debug.Log("Current Color: " + _color);
            _oilPaintEngine.UpdateFillColor(_currentColor);
            _currentColorObj.GetComponent<MeshRenderer>().material.color = new Color(colorVector.x, colorVector.y, colorVector.z);
            _line.startColor = new Color(colorVector.x, colorVector.y, colorVector.z);
            _line.endColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        
            _oilPaintEngine.FillApply();
        
            _oldColor = _color;
        }
        
    }

    public void ChangeRakelLengthOnWall(float length)
    {
        _oilPaintEngine.UpdateRakelLength(length);
    }
    
    public void ChangeRakelVolumeOnWall(float volume)
    {
        _oilPaintEngine.UpdateFillVolume((int)volume);
    }

    public void RakelLength(float length)
    {
        _rakellength = length;
        ApplyRakelSettings();
    }
    
    public void DeleteBuffer(bool state)
    {
        _oilPaintEngine.UpdateDeletePickedUpFromCSB(state);
    }
    
    public void DeleteBufferOnWall()
    {
        if (_deleteFromBuffer == false)
        {
            _deleteFromBuffer = true;
            _toggle.isOn = true;
        }
        else
        {
            _deleteFromBuffer = false;
            _toggle.isOn = false;
        }
        
        _oilPaintEngine.UpdateDeletePickedUpFromCSB(_deleteFromBuffer);
    }
    

    public void PaintVolume(int volume)
    {
        _paintvolume = volume;
        ApplyRakelSettings();
    }

    public void ApplyRakelSettings()
    {
        if (_paintvolume == 0) { _paintvolume = 60; }
        if (_rakellength == 0) { _rakellength = 4; }

        if (_color == -1) { _color = 0; }
        
        _currentColor = (Color_)_color;
        Vector3 colorVector = Colors.GetColor(_currentColor);
        if (_paintvolume != _oldPaintVolume)
        {
            _oilPaintEngine.UpdateFillVolume(_paintvolume);
            _oldPaintVolume = _paintvolume;
        }

        if (_rakellength != _oldRakelLength)
        {
            _oilPaintEngine.UpdateRakelLength(_rakellength);
            _oldRakelLength = _rakellength;
        }

        if (_color != _oldColor)
        {
            //_oilPaintEngine.UpdateFillColor(_currentColor);
            //_oldColor = _color;
            //_currentColorObj.GetComponent<Renderer>().material.color = new Color(colorVector.x, colorVector.y, colorVector.z);
            _oilPaintEngine.UpdateFillColor(_currentColor);
            _currentColorObj.GetComponent<MeshRenderer>().material.color = new Color(colorVector.x, colorVector.y, colorVector.z);
            _line.startColor = new Color(colorVector.x, colorVector.y, colorVector.z);
            _line.endColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        
            _oldColor = _color;
        }
        _line.startColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        _line.endColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        
        _oilPaintEngine.FillApply();
    }


    private const float Step = 0.84f; // Step per scroll
    private const float PosZ = -0.15f;
    private float _posY;
    
    public void Scroll(string direction)
    {

        if (direction == "Up")
        {
            _posY  = _posYForColorObject - Step;
        
            _colorButtons.transform.localPosition = new Vector3(_posXForColorObject, _posY, PosZ);
            _posYForColorObject -= Step;
            GetChildren();
        }

        if (direction == "Down")
        {
            _posY = _posYForColorObject + Step;
        
            _colorButtons.transform.localPosition = new Vector3(_posXForColorObject, _posY, PosZ);
            _posYForColorObject += Step;
            GetChildren();
        }

    }
    
    public void TabSelection()
    {
        if (_tab == false)
        {
            _tab = true;
        }
        else
        {
            _tab = false;
        }

        if (_tab == false)
        {
            _colorButtons.SetActive(true);
            _saveAndLoadButtons.SetActive(false);
            _scrollButtons.SetActive(true);
            
        }
        
        if (_tab == true)
        {
            _colorButtons.SetActive(false);
            _saveAndLoadButtons.SetActive(true);
            _scrollButtons.SetActive(false);
        }
    }
    
    public void ShowUI()
    {
        _canvasObj = GameObject.Find("Canvas");
        if (_uiState == false)
        {
            _uiState = true;
        }
        else
        {
            _uiState = false;
        }

        if (_uiState == false)
        {
            uiActive = false;
            //_canvasObj.transform.position = new Vector3(_canvasObj.transform.position.x, _canvasObj.transform.position.y, _canvasObj.transform.position.z-20);
            _uiCover.SetActive(false);
            _ui.SetActive(false);
        }
        else
        {
            uiActive = true;
            //_canvasObj.transform.position = new Vector3(_canvasObj.transform.position.x, _canvasObj.transform.position.y, 20); 
            _uiCover.SetActive(true);
            _ui.SetActive(true);
            
            
        }
    }

    public void GetChildren()
    {
        foreach (Transform child in _colorButtons.transform)
        {
            GameObject g = child.gameObject;
            
            // Check if Color Button are between scroll buttons
            if (0f > g.transform.position.y &&  g.transform.position.y > -6f) 
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
        pressure = Mathf.Clamp01(pressure/500);
        _currentPressure = pressure;
    }

    public float GetPressure()
    {
        return _currentPressure;
    }
    
    public float GetLength()
    {
        return _rakellength;
    }
    
    public float GetVolume()
    {
        return _paintvolume;
    }
    
    public int GetCurrentColor()
    {
        return _color;
    }

    public void IncreasePressure()
    {
        if (_currentPressure < 1)
        {
            _currentPressure += 0.1f;
            _currentPressure = Mathf.Round(_currentPressure * 10f) * 0.1f; //getting rid weird numbers from addition of float values
        }
        else
        {
            _currentPressure = 1;
        }
        _oilPaintEngine.UpdateRakelPressure(_currentPressure);
        _pressureText.GetComponent<TextMeshProUGUI>().SetText(_currentPressure.ToString());
    }

    public void DecreasePressure()
    {
        if (_currentPressure > 0)
        {
            _currentPressure -= 0.1f;
            _currentPressure = Mathf.Round(_currentPressure * 10f) * 0.1f; //getting rid weird numbers from addition of float values
        }
        else
        {
            _currentPressure = 0;
        }
        _oilPaintEngine.UpdateRakelPressure(_currentPressure);
        _pressureText.GetComponent<TextMeshProUGUI>().SetText(_currentPressure.ToString()); 
    }
}


