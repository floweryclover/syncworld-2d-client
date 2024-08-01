using System;
using System.Collections;
using UnityEngine;

public class SyncPlayerController : MonoBehaviour
{
    private SyncPlayerCharacter _controllingCharacter;

    public void AttachCharacter(SyncPlayerCharacter controllingCharacter) => _controllingCharacter = controllingCharacter;
    public void DetachCharacter() => _controllingCharacter = null;

    public bool TryGetCharacter(out SyncPlayerCharacter character)
    {
        if (_controllingCharacter)
        {
            character = _controllingCharacter;
            return true;
        }
        else
        {
            character = null;
            return false;
        }
    }

    private void Start()
    {
        StartCoroutine(SendCurrentPosition());
    }
    private void Update()
    {
        if (!_controllingCharacter)
        {
            return;
        }

        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        var newVelocity = new Vector2(horizontalInput, 0.0f);
        newVelocity *= 10.0f;
        if (Mathf.Abs(_controllingCharacter.Rigidbody.velocity.y) < 0.001f && verticalInput > 0.001f)
        {
            newVelocity.y = 5.0f;
        }
        else
        {
            newVelocity.y = _controllingCharacter.Rigidbody.velocity.y;
        }
        _controllingCharacter.Rigidbody.velocity = newVelocity;
    }

    private void LateUpdate()
    {
        if (!_controllingCharacter)
        {
            return;
        }
        
        var newPosition = _controllingCharacter.transform.position;
        newPosition.z = -10.0f;
        transform.position = newPosition;
    }

    private IEnumerator SendCurrentPosition()
    {
        while (true)
        {
            if (_controllingCharacter)
            {
                NetworkManager.SendCurrentPosition(
                    _controllingCharacter.transform.position.x,
                    _controllingCharacter.transform.position.y,
                    _controllingCharacter.Rigidbody.velocity.x,
                    _controllingCharacter.Rigidbody.velocity.y,
                    _controllingCharacter.Acceleration.x,
                    _controllingCharacter.Acceleration.y);     
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
