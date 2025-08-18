using System;
using System.Collections;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using TMPro;
public class ArduinoReader : MonoBehaviour
{

    private ButtonInteraction _interaction;
    private int numberOfColors;
    private int oldColor,oldPressure;
    private Thread serialThread;
    private SerialPort _serialPort;
    private float _pressureValue;
    private TextMeshProUGUI _pressureText;
    private OilPaintEngine _oilpaintengine;
    private string line;
    private CanvasReservoir _canvasReservoir;
    private int _lastSave, _lastLoad;
    private TextMeshProUGUI _clearTextOnWall;
    private int _sizeSet;
    
    //Confirm CanvasClear
    private bool _clearCanvasPressed;
    private GameObject _sizeText;
    void Start()
    {
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        
        _clearTextOnWall =  GameObject.Find("ClearText").GetComponent<TextMeshProUGUI>();
        _sizeText  = GameObject.Find("AdjustSizeText");
        
        const string portName = "COM4";
        numberOfColors = 23;
        
        _serialPort = new SerialPort(portName, 115200);
        _serialPort.ReadTimeout = 100;
        _serialPort.WriteTimeout = 100;
        try
        {
            _serialPort.Open();
            serialThread = new Thread(ReadSerial);
            serialThread.Start();
        
            //Sending a signal to reset the sizeDone Variable, so the user is able to adjust the canvas size on every programm start
            _serialPort.WriteLine("Reset");
        }
        catch (Exception )
        {
            Debug.Log("Serial Error");
        }
        
    }

    void ReadSerial()
    {
        while (_serialPort != null && _serialPort.IsOpen)
        {
            try
            {
                line = _serialPort.ReadLine();
                //Debug.Log("CURRENT LINE: " + line);

            }
            catch (Exception)
            {
                //Debug.Log("Couldn't read line");
            }
        }
    }
    
   
    
    void Update()
    {
        if (string.IsNullOrEmpty(line) == false)
        {
            //ColorPicker
            if (line.Contains("Color"))
            {   
                AbortClearCanvas();
                string colorNum = line.Replace("Color", "");
                int colorInt;
                if (int.TryParse(colorNum, out colorInt))
                {
                    Colorpicker(colorInt);
                    //_interaction.ApplyRakelSettings();
                }
                else
                {
                    //Debug.Log("Couldn't convert String to Int");
                } 
            }
            //PressureController
            else if (line.Contains("Pressure"))
            {
                AbortClearCanvas();
                string pressureNum = line.Replace("Pressure", "");
                int pressureInt;
                if (int.TryParse(pressureNum, out pressureInt))
                {
                    if (pressureInt != oldPressure)
                    {
                        _interaction.Pressure(pressureInt);
                        oldPressure = pressureInt;
                    }
                }
            }
            //CanvasSnapshotBuffer
            if (line.Contains("CSB"))
            {
                AbortClearCanvas();
                string csb = line.Replace("CSB", "");
                int csbInt;
                if (int.TryParse(csb, out csbInt))
                {
                    if (csbInt == 0)
                    {
                        _interaction.DeleteBuffer(false);
                    }
                    else
                    {
                        _interaction.DeleteBuffer(true);
                    }
                }
            }
            else if (line.Contains("Canvas"))
            {
                if (!_clearCanvasPressed)
                {
                    _sizeText.SetActive(true);
                    _clearCanvasPressed = true;
                    _sizeText.GetComponent<TextMeshProUGUI>().SetText("Are you sure you want to clear the canvas?");
                }
                else
                {
                    Debug.Log("Cleared Canvas");
                    _interaction.ClearCanvas();
                    AbortClearCanvas();
                }
            }
            else if (line.Contains("Undo"))
            {
                AbortClearCanvas();
                Debug.Log("Undo");
                _interaction.UndoLastStroke();
            }
            //Save
            else if (line.Contains("Save"))
            {
                AbortClearCanvas();
                string saveStr = line.Replace("Save", "");
                int imgNum;
                if (int.TryParse(saveStr, out imgNum))
                {
                    Debug.Log(line);
                    _interaction.SaveImg(imgNum);
                }
                else
                {
                    Debug.Log("Save couldn't parse correctly");
                }
            }
            //Load
            else if (line.Contains("Load"))
            {
                AbortClearCanvas();
                string loadStr = line.Replace("Load", "");
                int imgNum;
                if (int.TryParse(loadStr, out imgNum))
                {
                    _interaction.LoadImg(imgNum);
                }
                else
                {
                    Debug.Log("Load couldn't parse correctly");
                }
            }
            //ClearRakel
            else if (line.Contains("Clear"))
            {
                AbortClearCanvas();
                Debug.Log("Cleared Rakel");
                 _interaction.ClearRakel();
            }
            else if (line.Contains("Length"))
            {
                AbortClearCanvas();
                string lengthStr = line.Replace("Length", "");
                float length;
                if (float.TryParse(lengthStr, out length))
                {
                    _interaction.RakelLength(length);
                }
                else
                {
                    Debug.Log("Length couldn't parse correctly");
                }
            }
            else if (line.Contains("Volume"))
            {
                AbortClearCanvas();
                string volumeStr = line.Replace("Volume", "");
                int volume;
                if (int.TryParse(volumeStr, out volume))
                {
                    _interaction.PaintVolume(volume);
                }
                else
                {
                    Debug.Log("Volume couldn't parse correctly");
                }
            }
            else if (line.Contains("Width"))
            {
                string widthStr = line.Replace("Width", "");
                int width;
                if (int.TryParse(widthStr, out width))
                {
                    _interaction.ChangeWidthOnController(width);
                }
                else
                {
                    Debug.Log("Width couldn't parse correctly");
                }
            }
            else if (line.Contains("Height"))
            {
                string heightStr = line.Replace("Height", "");
                int height;
                if (int.TryParse(heightStr, out height))
                {
                    _interaction.ChangeHeightOnController(height);
                }
                else
                {
                    Debug.Log("Height couldn't parse correctly");
                }
            }
            else if (line.Contains("Reapply"))
            {
                AbortClearCanvas();
                StartCoroutine(ShowRefill());
                _sizeSet++;
                
                //we only need to call ApplySize once so we pass on it if its pressed more than once
                if (_sizeSet == 1)
                {
                    _interaction.ApplySize();
                }
                Debug.Log("Reapply Color");
                _interaction.ApplyRakelSettings();
            }
        }
        else
        {
            //Debug.Log("String Empty");
        }
        line = "";
    }

    IEnumerator ShowRefill()
    {
        _clearTextOnWall.SetText("Squeegee refilled");
        yield return new WaitForSeconds(1);
        _clearTextOnWall.SetText("");
    }

    //Gets called if for every other string other than "Canvas" or after "Canvas" 2 times in a row
    void AbortClearCanvas()
    {
        // Clear Canvas Text is also used at the beginning for adjusting the Canvas size
        // After adjusting the Size we can turn off the Text
        if (_sizeSet == 1)
        {
            _clearCanvasPressed = false;
            _sizeText.SetActive(false);
        }
    }

    void Colorpicker(int colorNum)
    {
        int color = Mathf.Clamp(colorNum, 0, numberOfColors - 1);
        if (color != oldColor)
        {
            Debug.Log("Color" + colorNum);
            _interaction.Color(color);
            oldColor = color;
        }
    }
    
    void OnApplicationQuit()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }

        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Abort();
        }
    }
}


