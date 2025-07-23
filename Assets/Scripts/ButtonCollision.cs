using System;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCollision : MonoBehaviour
{
    [SerializeField] ButtonInteraction _interaction;
    [SerializeField] OilPaintEngine _oilPaintEngine;
    private bool _touchingCanvas = false;
    
    private Button _button;
    private Slider _slider;
    private bool _holding,_sliderHolding = false;
    private Coroutine _scrollCoroutine,_slideCoroutine,_pressureCoroutine;
    private float _cooldown = 0.5f;
    private float _sliderCooldown = 0.2f;
    private GameObject _line;
    private GameObject _rakelLengthStart, _rakelLengthEnd;
    private GameObject _paintVolumeStart, _paintVolumeEnd;
    
    
    private int _counter;
    
    private void Start()
    {
        if (_interaction == null)
        {
            Debug.Log("Still NULL");
        }
        _line = GameObject.Find("LineRenderer");
        
        //GameObjects for Sliders for Wallcontroller
        _rakelLengthStart = GameObject.Find("RakelLengthStart");
        _rakelLengthEnd = GameObject.Find("RakelLengthEnd");
        _paintVolumeStart = GameObject.Find("RakelVolumeStart");
        _paintVolumeEnd = GameObject.Find("RakelVolumeEnd");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Button>())
        {
            _button = other.GetComponent<Button>();
            _button.onClick.Invoke();
            if (other.CompareTag("ScrollUP"))
            {
                if (!_holding)
                {
                    Debug.Log("ScrollUp Tag");
                    _holding = true;
                    _scrollCoroutine = StartCoroutine(KeepScrolling("Up"));
                }
            }
            else if (other.CompareTag("ScrollDOWN"))
            {
                if (!_holding)
                {
                    Debug.Log("ScrollDown Tag");
                    _holding = true;
                    _scrollCoroutine = StartCoroutine(KeepScrolling("Down"));
                }
            }
            else if (other.CompareTag("PressureUP"))
            {
                if (!_holding)
                {
                    _holding = true;
                    _pressureCoroutine = StartCoroutine(KeepChangingPressure("Up"));
                }
            }
            else if (other.CompareTag("PressureDOWN"))
            {
                _holding = true;
                _pressureCoroutine = StartCoroutine(KeepChangingPressure("Down"));
            }

        }
        else if (other.GetComponent<Slider>())
        {
            _slider = other.GetComponent<Slider>();
            if (!_sliderHolding)
            {
                _sliderHolding = true;
                _slideCoroutine = StartCoroutine(KeepSliding(_slider.GetComponent<BoxCollider>()));
            }
        }
        else if (other.CompareTag("Canvas"))
        {
            _touchingCanvas = true;
        }
    }

    public bool TouchingCanvas()
    {
        Debug.Log("TouchingCanvas: "  + _touchingCanvas);
        return _touchingCanvas;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ScrollUP") || other.CompareTag("ScrollDOWN"))
        {
            _holding = false;
            if (_scrollCoroutine != null)
            {
                StopCoroutine(_scrollCoroutine);
                _scrollCoroutine = null;    
            }
        }
        else if (other.CompareTag("PressureUP") || other.CompareTag("PressureDOWN"))
        {
            _holding = false;
            if (_pressureCoroutine != null)
            {
                StopCoroutine(_pressureCoroutine);
                _pressureCoroutine = null;
            }
        }
        else if (other.CompareTag("Length") || other.CompareTag("Volume"))
        {
            _sliderHolding = false;
            StopCoroutine(_slideCoroutine);
            _slideCoroutine = null;
        }
    }
    
    //If Rakel is longer on Scroll Button
    private IEnumerator KeepScrolling(string direction)
    {
        while (_holding)
        {
            yield return new WaitForSeconds(_cooldown);
            _interaction.Scroll(direction);
        }
        
    }
    
    private IEnumerator KeepSliding(Collider other)
    {
        
        while (_sliderHolding)
        {
            if (other.CompareTag("Length"))
            { 
                float minX = _rakelLengthStart.transform.position.x;
                float maxX = _rakelLengthEnd.transform.position.x;
                const float minSlider = 2;
                const float maxSlider = 15;
                if (maxX + 0.1f > _line.transform.position.x && _line.transform.position.x > minX - 0.1f)
                {
                    _slider.handleRect.transform.position = new Vector3(_line.transform.position.x, _slider.handleRect.transform.position.y, _slider.handleRect.transform.position.z);
                    //_slider.fillRect.right = new Vector3(_hand.transform.position.x, _slider.fillRect.transform.position.y, _slider.fillRect.transform.position.z);
                    float currentX = _slider.handleRect.transform.position.x;
                    float normalizedValue = Mathf.InverseLerp(minX, maxX, currentX);
                    float sliderValue = Mathf.Lerp(minSlider, maxSlider, normalizedValue);
                    _interaction.ChangeRakelLengthOnWall(sliderValue);
                }
            }
            else if (other.CompareTag("Volume"))
            {
                float minX = _paintVolumeStart.transform.position.x;
                float maxX = _paintVolumeEnd.transform.position.x;
             
                const float minSlider = 60;
                const float maxSlider = 256;
                
                if (maxX + 0.1f > _line.transform.position.x && _line.transform.position.x > minX - 0.1f)
                {
                    _slider.handleRect.transform.position = new Vector3(_line.transform.position.x, _slider.handleRect.transform.position.y, _slider.handleRect.transform.position.z);
                    float currentX = _slider.handleRect.transform.position.x;
                    float normalizedValue = Mathf.InverseLerp(minX, maxX, currentX);
                    float paintvolume = Mathf.Lerp(minSlider, maxSlider, normalizedValue);
                    _interaction.ChangeRakelVolumeOnWall((int)paintvolume);
                }
                
            }
            
            yield return new WaitForSeconds(_sliderCooldown);
        }
        
    }
    
    private IEnumerator KeepChangingPressure(string direction)
    {
        while (_holding)
        {
            if (direction == "Up")
            {
                yield return new WaitForSeconds(_cooldown);
                _interaction.IncreasePressure();
            }
            else if (direction == "Down")
            {
                yield return new WaitForSeconds(_cooldown);
                _interaction.DecreasePressure();
            }
        }
        
    }
}