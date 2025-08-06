using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToColor : MonoBehaviour
{
    private ButtonInteraction _buttonInteraction;
    private int _currentColor = -1;
    private int _lastColor = -1;
    private float _offset;
    
    private Quaternion _targetRotation;
    //public int color;
    public float speed = 90f; // degrees/second

    void Start()
    {
        _buttonInteraction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
    }

    void Update()
    { 
        _currentColor = _buttonInteraction.GetCurrentColor();
        //_currentColor = color;
        if (_currentColor != _lastColor && _currentColor >= 0)
        {
            if (_currentColor > 3) { _offset = -5;}
            if (_currentColor > 12) { _offset = 0;}
            if (_currentColor > 17) { _offset = -5;}
            if (_currentColor > 20) { _offset = -10;}
            
            float startRotation = -35f;
            float rotationPerColor = 270f / 23f;
            float zielRotation = startRotation + _currentColor * rotationPerColor + _offset;

            _targetRotation = Quaternion.Euler(zielRotation, 90f, 90f);
            _lastColor = _currentColor;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * speed);
    }
}