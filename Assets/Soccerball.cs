using UnityEngine;

public class Soccerball : MonoBehaviour, IMoveableEntity
{
    private Vector2 _beginPosition;
    private Vector2 _endPosition;
    private float _elapsedTime;
    public void MoveTo(float x, float y)
    {
        _beginPosition = _endPosition;
        if (_beginPosition == Vector2.zero)
        {
            _beginPosition = transform.position;
        }
        _endPosition = new Vector2(x, y);
        _elapsedTime = 0.0f;
    }

    public void Teleport(float x, float y)
    {
        _beginPosition = _endPosition = new Vector2(x, y);
        transform.position = _endPosition;
    }

    private void Start()
    {
        _beginPosition = _endPosition = Vector2.zero;
    }
    
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > 0.1f)
        {
            transform.position = _endPosition;
        }
        var lerpedPosition = Vector2.Lerp(_beginPosition, _endPosition, _elapsedTime / 0.1f);
        transform.position = lerpedPosition;
    }
}
