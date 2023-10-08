using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

[Serializable]
public struct CubeData
{
  public int Level;
  public Color Color;
}


public class Cube : MonoBehaviour
{
  public event Action<Cube, Cube> OnMerge;

  [SerializeField] private List<TextMeshPro> _allNumbers;
  [SerializeField] private List<Color> _allColors;

  [SerializeField] private AnimationCurve _jumpCurve;
  [SerializeField] private float _jumpSpeed;

  public CubeData Data;
  public bool AlreadyMerging = false;

  private Rigidbody _rigidbody;
  private Renderer _renderer;
  private bool flying = false;
  private int colorIndex = 0;
  private float yJumpPos = 0f;

  private void Start()
  {
    _renderer = gameObject.GetComponent<Renderer>();
    _rigidbody = gameObject.GetComponent<Rigidbody>();
    UpdateCubeNumbers(Data.Level);
  }

  private void UpdateCubeNumbers(int number)
  {
    foreach (var numText in _allNumbers)
      numText.text = number.ToString();
  }

  private void UpdateColor()
  {
    colorIndex++;
    if (colorIndex >= _allColors.Count)
      colorIndex = 0;

    _renderer.material.SetColor("_Color", _allColors[colorIndex]);
  }

  private void Merging(Collision collision)
  {
    if (AlreadyMerging)
      return;

    if (this.Data.Level != collision.gameObject.GetComponent<Cube>().Data.Level)
      return;

    Cube mergingCube = collision.gameObject.GetComponent<Cube>();
    mergingCube.AlreadyMerging = true;
    OnMerge?.Invoke(this, mergingCube);
  }

  public void Upgrade(List<Cube> allCubes)
  {
    Data.Level = Data.Level * 2;
    UpdateCubeNumbers(Data.Level);
    UpdateColor(); 
    
    Cube nextMergeCube = allCubes.Find(c => c.Data.Level == Data.Level && c!=this);

    if (nextMergeCube != null)
      JumpToOtherCube(nextMergeCube.transform.position);
  }

  private void JumpToOtherCube(Vector3 position)
  {
    flying = false;
    StartCoroutine(JumpAnimation(position));
  }

  private IEnumerator JumpAnimation(Vector3 position)
  {
    while(!flying && yJumpPos < 1f)
    {
      yJumpPos += _jumpSpeed * Time.deltaTime;

      transform.position = new Vector3(transform.position.x,
                                 _jumpCurve.Evaluate(yJumpPos),
                                 transform.position.z);

      transform.position += new Vector3((position.x - transform.position.x) * yJumpPos/2,
                                        0f,
                                        (position.z - transform.position.z) * yJumpPos/2);
      yield return null;
    }

    yJumpPos = 0f;
  }

  private void OnCollisionEnter(Collision collision)
  {
    _rigidbody.useGravity = true;

    if (collision.gameObject.layer == 10)
    {
      flying = true;
      Merging(collision);
      flying = false;
    }
  }
}
