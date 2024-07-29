using UnityEngine;

public class SyncPlayerCharacter : MonoBehaviour
{
    private SyncPlayerController _controller;
    public Rigidbody2D Rigidbody { get; private set; }

    private Vector2 _beginPosition;
    private Vector2 _targetPosition;
    private float _elapsedTime;

    private Vector2 _lastVelocity;
    public Vector2 Acceleration { get; private set; }
    
    public void MoveTo(float x, float y)
    {
        _beginPosition = (_targetPosition == Vector2.zero ? transform.position : _targetPosition);
        _targetPosition = new Vector2(x, y);
        _elapsedTime = 0.0f;
    }
    
    public void AttachController(SyncPlayerController controller) => _controller = controller;
    public void DetachController() => _controller = null;

    private void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _targetPosition = Vector2.zero;
    }

    private void Update()
    {
        Acceleration = (Rigidbody.velocity - _lastVelocity) / Time.fixedDeltaTime;
        _lastVelocity = Rigidbody.velocity;
        
        if (_targetPosition == Vector2.zero)
        {
            return;
        }
        
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > 0.5f)
        {
            transform.position = _targetPosition;
            return;
        }
        var lerpedPosition = Vector2.Lerp(_beginPosition, _targetPosition, _elapsedTime / 0.5f);
        transform.position = lerpedPosition;
    }
}
