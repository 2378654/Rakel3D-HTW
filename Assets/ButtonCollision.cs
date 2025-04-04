using System;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCollision : MonoBehaviour
{
    private Button _button;
    private ButtonInteraction _interaction;
    private bool _holding = false;
    private Coroutine _scrollCoroutine, _pressureCoroutine;
    private float _cooldown = 1f;
    private GameObject Interaction;

    private void Start()
    {
        Interaction = GameObject.Find("Interaction");
        _interaction = Interaction.GetComponent<ButtonInteraction>();
        if (_interaction == null)
        {
            Debug.Log("Still NULL");
        }
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
    }

    private IEnumerator KeepScrolling(string direction)
    {
        while (_holding)
        {
            yield return new WaitForSeconds(_cooldown);
            _interaction.Scroll(direction);
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