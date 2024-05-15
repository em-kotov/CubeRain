using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Color _defaultColor;
    private bool _hasEnteredPlatform = false;

    public event UnityAction<Cube> EnteredPlatform;

    private void Start()
    {
        _defaultColor = GetComponent<Renderer>().material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform) &&
            _hasEnteredPlatform == false)
        {
            SetColor();
            _hasEnteredPlatform = true;
            EnteredPlatform?.Invoke(this);
        }
    }

    public void Reset()
    {
        GetComponent<Renderer>().material.color = _defaultColor;
        _hasEnteredPlatform = false;
    }

    private void SetColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 0.3f, 0.3f, 0.95f, 0.95f);
    }
}
