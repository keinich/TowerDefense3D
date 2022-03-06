using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour {

  [SerializeField] private Camera playerCamera;

  private GameObject currentPlacingTower;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (currentPlacingTower != null) {

      Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(camRay, out RaycastHit hitInfo, 500f)) {
        Debug.Log($"Placing Tower at {hitInfo.point}");
        currentPlacingTower.transform.position = hitInfo.point;
      }

      if (Input.GetMouseButtonDown(0)) {
        currentPlacingTower = null;
      }
    }
  }

  public void SetTowerToPlace(GameObject tower) {
    currentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
  }

}
