using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeMonkey.CameraSystem {

    public class CameraSystem2D : MonoBehaviour {

        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private bool useEdgeScrolling = false;
        [SerializeField] private bool useDragPan = false;
        [SerializeField] private float orthographicSizeMin = 10;
        [SerializeField] private float orthographicSizeMax = 50;

        private bool dragPanMoveActive;
        private Vector2 lastMousePosition;
        private float targetOrthographicSize = 50;



        private void Update() {
            HandleCameraMovement();

            if (useEdgeScrolling) {
                HandleCameraMovementEdgeScrolling();
            }

            if (useDragPan) {
                HandleCameraMovementDragPan();
            }

            HandleCameraRotation();

            HandleCameraZoom_OrthographicSize();
        }

        private void HandleCameraMovement() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Keyboard.current.wKey.isPressed) inputDir.z = +1f;
            if (Keyboard.current.sKey.isPressed) inputDir.z = -1f;
            if (Keyboard.current.aKey.isPressed) inputDir.x = -1f;
            if (Keyboard.current.dKey.isPressed) inputDir.x = +1f;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementEdgeScrolling() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            int edgeScrollSize = 20;

            if (Mouse.current.position.value.x < edgeScrollSize) {
                inputDir.x = -1f;
            }
            if (Mouse.current.position.value.y < edgeScrollSize) {
                inputDir.z = -1f;
            }
            if (Mouse.current.position.value.x > Screen.width - edgeScrollSize) {
                inputDir.x = +1f;
            }
            if (Mouse.current.position.value.y > Screen.height - edgeScrollSize) {
                inputDir.z = +1f;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementDragPan() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Mouse.current.rightButton.wasPressedThisFrame) {
                dragPanMoveActive = true;
                lastMousePosition = Mouse.current.position.value;
            }
            if (Mouse.current.rightButton.wasReleasedThisFrame) {
                dragPanMoveActive = false;
            }

            if (dragPanMoveActive) {
                Vector2 mouseMovementDelta = (Vector2)Mouse.current.position.value - lastMousePosition;

                float dragPanSpeed = 1f;
                inputDir.x = mouseMovementDelta.x * dragPanSpeed;
                inputDir.z = mouseMovementDelta.y * dragPanSpeed;

                lastMousePosition = Mouse.current.position.value;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraRotation() {
            float rotateDir = 0f;
            if (Keyboard.current.qKey.isPressed) rotateDir = +1f;
            if (Keyboard.current.eKey.isPressed) rotateDir = -1f;

            float rotateSpeed = 100f;
            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
        }

        private void HandleCameraZoom_OrthographicSize() {
            if (Mouse.current.scroll.value.y > 0) {
                targetOrthographicSize -= 5;
            }
            if (Mouse.current.scroll.value.y < 0) {
                targetOrthographicSize += 5;
            }

            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, orthographicSizeMin, orthographicSizeMax);

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.m_Lens.OrthographicSize =
                Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
        }

    }

}