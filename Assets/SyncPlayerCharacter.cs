using UnityEngine;

public class SyncPlayerCharacter : MonoBehaviour, ISyncControllable
{
    private SyncPlayerController _controller;
    public Rigidbody2D Rigidbody { get; private set; }

    public void MoveTo(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    public void AttachController(SyncPlayerController controller) => _controller = controller;
    public void DetachController() => _controller = null;

    private void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
}
