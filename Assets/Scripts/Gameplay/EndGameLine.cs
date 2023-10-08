using System;
using UnityEngine;

public class EndGameLine : MonoBehaviour
{
  public event Action OnEndGame;

  private void OnTriggerEnter(Collider other)
  {
    if(other.gameObject.layer == 10)
    {
      if (other.gameObject.GetComponent<CubeSwipeHandler>() != null)
        return;

      Debug.Log("End Game");
      OnEndGame?.Invoke();
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.layer == 10)
    {
      if (other.gameObject.GetComponent<CubeSwipeHandler>() != null)
        return;

      Debug.Log("End Game");
      OnEndGame?.Invoke();
    }
  }
}
