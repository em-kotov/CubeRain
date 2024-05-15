using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Platform _lastPlatform = null;
    private Color _defaultColor;

    public event UnityAction<Cube> EnteredPlatform;

    public Platform LastPlatform => _lastPlatform;

    private void Start()
    {
        _defaultColor = this.GetComponent<Renderer>().material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.GetComponent<Platform>() == false)
            return;

        Platform platform = obj.GetComponent<Platform>();

        if (platform == _lastPlatform)
            return;

        SetPlatform(platform);
        SetColor();
        EnteredPlatform?.Invoke(this);
    }

    public void Reset()
    {
        GetComponent<Renderer>().material.color = _defaultColor;
        _lastPlatform = null;
    }

    private void SetPlatform(Platform platform)
    {
        _lastPlatform = platform;
    }

    private void SetColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 0.3f, 0.3f, 0.95f, 0.95f);
    }
}
