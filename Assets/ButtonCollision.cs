using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCollision : MonoBehaviour
{
    private Button _button;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Button>())
        {
            _button = other.GetComponent<Button>();
            _button.onClick.Invoke();
        }
    }
}