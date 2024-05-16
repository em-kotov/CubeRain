using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Color _defaultColor;
    private bool _hasEnteredPlatform = false;

    public event UnityAction<Cube> EnteredPlatform;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
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
        _renderer.material.color = _defaultColor;
        _renderer = null;
        _hasEnteredPlatform = false;
    }

    private void SetColor()
    {
        _renderer.material.color = Random.ColorHSV(0f, 1f, 0.3f, 0.3f, 0.95f, 0.95f);
    }
}
