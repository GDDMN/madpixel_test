using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameWindow : MonoBehaviour
{
  public void Click()
  {
    Debug.Log("Click");
    SceneManager.LoadScene("game");
  }
}
