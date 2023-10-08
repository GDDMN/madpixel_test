using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : Singleton<G>
{
  public UI UI;

  public Currency Currency = new Currency();
}
