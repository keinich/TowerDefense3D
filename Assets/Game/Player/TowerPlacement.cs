using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour {

  [SerializeField] private LayerMask placementCheckMask;
  [SerializeField] private LayerMask placementCollideMask;
  [SerializeField] private Camera playerCamera;

  private GameObject currentPlacingTower;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (currentPlacingTower != null) {

      Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      if (!Physics.Raycast(camRay, out hitInfo, 500f, placementCollideMask)) {
        return;
      }


      if (hitInfo.collider.gameObject != null) {
        Vector3 gridPoint = new Vector3(
          hitInfo.collider.gameObject.transform.position.x, 
          hitInfo.point.y, 
          hitInfo.collider.gameObject.transform.position.z
        );
        currentPlacingTower.transform.position = gridPoint;
        if (Input.GetMouseButtonDown(0)) {
          //hitInfo.collider.gameObject.transform.position;

          Debug.Log($"Placing Tower at {hitInfo.point}");
          if (hitInfo.collider.gameObject.CompareTag("CanPlace")) {
            BoxCollider towerCollider = currentPlacingTower.gameObject.GetComponent<BoxCollider>();
            towerCollider.isTrigger = true;

            Vector3 boxCenter = currentPlacingTower.gameObject.transform.position + towerCollider.center;
            Vector3 halfExtends = towerCollider.size / 2;
            if (
              !Physics.CheckBox(
                boxCenter, halfExtends, Quaternion.identity, placementCheckMask, QueryTriggerInteraction.Ignore
              )
            ) {
              towerCollider.isTrigger = false;
              currentPlacingTower = null;
            }
          }

        }
      }
    }
  }

  public void SetTowerToPlace(GameObject tower) {
    currentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
  }

}
