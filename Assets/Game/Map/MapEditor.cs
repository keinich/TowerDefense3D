using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {

  Map map;

  public override void OnInspectorGUI() {
    if (DrawDefaultInspector()) {
    }

    if (GUILayout.Button("Generate")) {
      Debug.Log("Generating Map");

      map.Generate();
    }
  }

  private void OnEnable() {
    map = (Map)target;
    Tools.hidden = true;
  }

  void OnDisable() { 
    Tools.hidden = false;
  }

}
