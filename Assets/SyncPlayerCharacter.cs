using UnityEngine;

public class SyncPlayerCharacter : MonoBehaviour
{
    private SyncPlayerController _controller;
    public Rigidbody2D Rigidbody { get; private set; }

    public void AttachController(SyncPlayerController controller) => _controller = controller;
    public void DetachController() => _controller = null;

    private void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
}
