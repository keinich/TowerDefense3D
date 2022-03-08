using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  private Vector3 velocity;
  private Vector3 playerMovementInput;
  private Vector2 playerMouseInput;
  private float xRot;
  private float zoom;

  [SerializeField] private Transform playerCamera;
  [SerializeField] private CharacterController controller;
  [Space]
  [SerializeField] private float speed;
  [SerializeField] private float sensitivity;
  [SerializeField] private float zoomSpeed;


  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

    MovePlayer();
    MovePlayerCamera();
  }

  private void MovePlayerCamera() {

    if (Input.GetKey(KeyCode.Mouse1)) {
      xRot -= playerMouseInput.y * sensitivity;
      transform.Rotate(0f, playerMouseInput.x * sensitivity, 0f);
      playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
    zoom += Input.GetAxis("Mouse ScrollWheel");
    Vector3 localForward = playerCamera.transform.worldToLocalMatrix.MultiplyVector(playerCamera.transform.forward);
    playerCamera.transform.Translate(
      localForward * zoom * Time.deltaTime * zoomSpeed
    );
    zoom = 0;
    //zoom -= Time.deltaTime * zoomSpeed;
    //zoom = Math.Min(0, zoom);
  }

  private void MovePlayer() {
    Vector3 moveVector = transform.TransformDirection(playerMovementInput);

    if (Input.GetKey(KeyCode.Space)) {
      velocity.y = 1f;
    } else if (Input.GetKey(KeyCode.LeftShift)) {
      velocity.y = -1;
    }

    controller.Move(moveVector * speed * Time.deltaTime);
    controller.Move(velocity * speed * Time.deltaTime);

    velocity.y = 0f;
  }
}
