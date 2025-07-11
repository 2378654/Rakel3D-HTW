using System;
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
    void Start()
    {
        
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        //_canvasReservoir = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>().GetComponent<CanvasReservoir>();
        
        const string portName = "COM4";
        numberOfColors = 23;
        
        _serialPort = new SerialPort(portName, 115200);
        _serialPort.ReadTimeout = 100;
        _serialPort.Open();

        serialThread = new Thread(ReadSerial);
        serialThread.Start();
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
                string csb = line.Replace("CSB", "");
                int csbInt;
                if (int.TryParse(csb, out csbInt))
                {
                    if (csbInt == 0)
                    {
                        _oilpaintengine.UpdateDeletePickedUpFromCSB(false);
                    }
                    else
                    {
                        _oilpaintengine.UpdateDeletePickedUpFromCSB(true);
                    }
                }
            }
            else if (line.Contains("Canvas"))
            {
                Debug.Log("Cleared Canvas");
                _interaction.ApplySize();
                _interaction.ClearCanvas();
            }
            else if (line.Contains("Undo"))
            {
                Debug.Log("Undo");
                _interaction.UndoLastStroke();
            }
            else if (line.Contains("Size"))
            {
                string sizeStr = line.Replace("Size", "");
                int size;
                if (int.TryParse(sizeStr, out size))
                {
                    //_interaction.ChangeSize(size);
                }
                else
                {
                    Debug.Log("Size couldn't parse correctly");
                }
            }
            //Save
            else if (line.Contains("Save"))
            {
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
                Debug.Log("Cleared Rakel");
                 _interaction.ClearRakel();
            }
            else if (line.Contains("Length"))
            {
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


