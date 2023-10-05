using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSwipeHandler : MonoBehaviour
{
  private float _minPos;
  private float _maxPos;

  private InputController _inputController;
  private Vector2 _pressPosition;

  private void Awake()
  {
    _inputController = new InputController();
  }

  private void OnEnable()
  {
    _inputController.Enable();
  }

  private void OnDisable()
  {
    _inputController.Disable();
  }

  public void Init(float minPos, float maxPos)
  {
    _minPos = minPos;
    _maxPos = maxPos;
  }

  private void Update()
  {
    _pressPosition = _inputController.Cube.PointPress.ReadValue<Vector2>();
    SetPosition();
  }

  private void SetPosition()
  {
    Vector2 fingerPos = Camera.main.ScreenToWorldPoint(_pressPosition);
    float xPosition = Mathf.Clamp(fingerPos.x, _minPos, _maxPos);
    Debug.Log(fingerPos);
    transform.position = new Vector3(xPosition,
                                     transform.position.y,
                                     transform.position.z);

  }

}