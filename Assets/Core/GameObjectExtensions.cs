using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions {

  public static void DestroyAllChildren(this GameObject self) { 
    if (self == null) { return; }

    for (int i = self.transform.childCount; i > 0; --i)
      Object.DestroyImmediate(self.transform.GetChild(0).gameObject);
  }

}
