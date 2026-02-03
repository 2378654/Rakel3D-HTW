using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthIndicator : MonoBehaviour
{
    private GameObject _indicator;
    private ButtonInteraction _interaction;
    float currentLength, oldLength;
    private LineRenderer _line, _start, _end;
    private Coroutine _coroutine;
    public float currentX= 0f;
    void Start()
    {
        _indicator = GameObject.Find("LengthIndicator");
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
        _line = _indicator.GetComponent<LineRenderer>();
        _start = GameObject.Find("Start").GetComponent<LineRenderer>();
        _end = GameObject.Find("End").GetComponent<LineRenderer>();

        _line.startWidth = 0.1f;
        _line.endWidth = 0.1f;
        _line.startColor = Color.black;
        _line.endColor = Color.black;
        
        _start.startWidth = 0.1f;
        _start.endWidth = 0.1f;
        _start.startColor = Color.black;
        _start.endColor = Color.black;
        
        _end.startWidth = 0.1f;
        _end.endWidth = 0.1f;
        _end.startColor = Color.black;
        _end.endColor = Color.black;
        
        _line.SetPosition(0, new Vector3(currentX, 0, -0.15f));
        _line.SetPosition(1, new Vector3(currentX, 0, -0.15f));
        
    }

    // Update is called once per frame
    void Update()
    {
        currentLength = _interaction.GetLength();
        if (currentLength != oldLength)
        {
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(ShowLengthIndicator());

            oldLength = currentLength;
        }
    }

    private IEnumerator ShowLengthIndicator()
    {
        //line between Start and End
        _line.SetPosition(0, new Vector3(currentX, 0-currentLength/2, -0.15f));
        _line.SetPosition(1, new Vector3(currentX, 0+currentLength/2, -0.15f));
        
        //Start
        _start.SetPosition(0, new Vector3(currentX-1, 0+currentLength/2, -0.15f));
        _start.SetPosition(1, new Vector3(currentX+1, 0+currentLength/2, -0.15f));
        
        //End
        _end.SetPosition(0, new Vector3(currentX-1, 0-currentLength/2, -0.15f));
        _end.SetPosition(1, new Vector3(currentX+1, 0-currentLength/2, -0.15f));
        
        yield return new WaitForSeconds(1f);
        
        //line between Start and End
        _line.SetPosition(0, new Vector3(currentX, 0, -0.15f));
        _line.SetPosition(1, new Vector3(currentX, 0, -0.15f));
        
             
        //Start
        _start.SetPosition(0, new Vector3(currentX, 0+currentLength/2, -0.15f));
        _start.SetPosition(1, new Vector3(currentX, 0+currentLength/2, -0.15f));
        
        //End
        _end.SetPosition(0, new Vector3(currentX, 0-currentLength/2, -0.15f));
        _end.SetPosition(1, new Vector3(currentX, 0-currentLength/2, -0.15f));
    }
}
