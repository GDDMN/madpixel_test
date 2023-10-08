using System;
using UnityEngine;
using UnityEngine.Events;

public class CubeSwipeHandler : MonoBehaviour
{
  public UnityEvent OnCollision;

  private float _minPos;
  private float _maxPos;

  private InputController _inputController;
  private Vector3 _pressPosition;

  private bool _canMove = false;
  private Rigidbody _rigidbody;

  [SerializeField] private float _forwardSpeed;

  private void Awake()
  {
    _inputController = new InputController();
    _rigidbody = gameObject.GetComponent<Rigidbody>();
  }

  private void OnEnable()
  {
    _inputController.Enable();
    _inputController.Cube.PointerPress.performed += context => MovementValidate();
    _inputController.Cube.PointerPress.canceled += context => LounchCube();
  }

  private void OnDisable()
  {
    _inputController.Disable();
    _inputController.Cube.PointerPress.canceled -= context => LounchCube();
  }

  public void Init(float minPos, float maxPos)
  {
    _minPos = minPos;
    _maxPos = maxPos;
  }

  private void Update()
  {
    _pressPosition = _inputController.Cube.PointerPosition.ReadValue<Vector2>();
    _pressPosition = new Vector3(_pressPosition.x, _pressPosition.y, Camera.main.nearClipPlane);
    SetPosition();
  }

  private void SetPosition()
  {
    if (!_canMove)
      return;

    Vector3 fingerPos = Camera.main.ScreenToWorldPoint(_pressPosition);

    float xPosition = Mathf.Clamp(fingerPos.x, _minPos, _maxPos);
    Debug.Log(xPosition);

    transform.position = new Vector3(xPosition,
                                     transform.position.y,
                                     transform.position.z);
  }

  private void MovementValidate()
  {
    _canMove = true;
  }

  private void LounchCube()
  {
    _canMove = false;
    _inputController.Cube.PointerPress.performed -= context => MovementValidate();
    _rigidbody.velocity = Vector3.forward * _forwardSpeed;
    
  }

  private void OnCollisionEnter(Collision collision)
  {
    OnCollision?.Invoke();
    OnCollision.RemoveAllListeners();
    Destroy(this);
  }

}
