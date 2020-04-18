﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

   private PlayerInputActions inputActions;
   private Vector2 movementInput;

   [SerializeField]
   private float moveSpeed = 10f;

   private Vector3 inputDirection;
   private Vector3 moveVector;
   private Quaternion currentRotation;

   void Awake() {
      inputActions = new PlayerInputActions();
      inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();

   }

   void FixedUpdate() {
      float h = movementInput.x;
      float v = movementInput.y;

      Vector3 targetInput = new Vector3(h, 0, v);

      inputDirection = Vector3.Lerp(inputDirection, targetInput, Time.deltaTime * 10f);

      Vector3 camForward = Camera.main.transform.forward;
      Vector3 camRight = Camera.main.transform.right;
      camForward.y = 0f;
      camRight.y = 0f;

      Vector3 desiredDirection = camForward * inputDirection.z + camRight * inputDirection.x;

      Move(desiredDirection);

      Turn(desiredDirection);
   }

   void Move(Vector3 desiredDirection) {
      moveVector.Set(desiredDirection.x, 0f, desiredDirection.z);
      moveVector = moveVector * moveSpeed * Time.deltaTime;
      transform.position += moveVector;
   }

   void Turn(Vector3 desiredDirection) {
      if ((desiredDirection.x > 0.1f || desiredDirection.x < -0.1f) || (desiredDirection.z > 0.1f || desiredDirection.z < -0.1f)) {
         currentRotation = Quaternion.LookRotation(desiredDirection);
         transform.rotation = currentRotation;
      } else {
         transform.rotation = currentRotation;
      }
   }

   private void OnEnable() {
      inputActions.Enable();
   }

   private void OnDisable() {
      inputActions.Disable();
   }
}