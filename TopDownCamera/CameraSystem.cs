using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace CodeMonkey.CameraSystem {

    public class CameraSystem : MonoBehaviour {

        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private bool useEdgeScrolling = false;
        [SerializeField] private bool useDragPan = false;
        [SerializeField] private float fieldOfViewMax = 50;
        [SerializeField] private float fieldOfViewMin = 10;
        [SerializeField] private float followOffsetMin = 5f;
        [SerializeField] private float followOffsetMax = 50f;
        [SerializeField] private float followOffsetMinY = 10f;
        [SerializeField] private float followOffsetMaxY = 50f;

        private bool dragPanMoveActive;
        private Vector2 lastMousePosition;
        private float targetFieldOfView = 50;
        private Vector3 followOffset;


        private void Awake() {
            followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }

        private void Update() {
            HandleCameraMovement();

            if (useEdgeScrolling) {
                HandleCameraMovementEdgeScrolling();
            }

            if (useDragPan) {
                HandleCameraMovementDragPan();
            }

            HandleCameraRotation();

            //HandleCameraZoom_FieldOfView();
            //HandleCameraZoom_MoveForward();
            HandleCameraZoom_LowerY();
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

        private void HandleCameraZoom_FieldOfView() {
            if (Mouse.current.scroll.value.y > 0) {
                targetFieldOfView -= 5;
            }
            if (Mouse.current.scroll.value.y < 0) {
                targetFieldOfView += 5;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraZoom_MoveForward() {
            Vector3 zoomDir = followOffset.normalized;

            float zoomAmount = 3f;
            if (Mouse.current.scroll.value.y > 0) {
                followOffset -= zoomDir * zoomAmount;
            }
            if (Mouse.current.scroll.value.y < 0) {
                followOffset += zoomDir * zoomAmount;
            }

            if (followOffset.magnitude < followOffsetMin) {
                followOffset = zoomDir * followOffsetMin;
            }

            if (followOffset.magnitude > followOffsetMax) {
                followOffset = zoomDir * followOffsetMax;
            }

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraZoom_LowerY() {
            float zoomAmount = 3f;
            if (Mouse.current.scroll.value.y > 0) {
                followOffset.y -= zoomAmount;
            }
            if (Mouse.current.scroll.value.y < 0) {
                followOffset.y += zoomAmount;
            }

            followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

        }

    }

}