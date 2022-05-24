using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TestCameraMovement : MonoBehaviour {
    private Camera _currentCam;
    private float moveSpeed = 5.0f;
    private float pitch = 0f;
    private float yaw = 0f;

    void Start() {
        _currentCam = GetComponent<Camera>();
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
    }

    void Update() {
        if (Input.GetMouseButton(1)) {
            var moveDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) {
                moveDir += moveSpeed * Time.deltaTime * Vector3.forward;
            }

            if (Input.GetKey(KeyCode.A)) {
                moveDir += moveSpeed * Time.deltaTime * Vector3.left;
            }

            if (Input.GetKey(KeyCode.S)) {
                moveDir += moveSpeed * Time.deltaTime * Vector3.back;
            }

            if (Input.GetKey(KeyCode.D)) {
                moveDir += moveSpeed * Time.deltaTime * Vector3.right;
            }

            if (Input.GetKey(KeyCode.Q)) {
                moveDir += moveSpeed * Time.deltaTime * Vector3.down;
            }

            if (Input.GetKey(KeyCode.E)) {
                moveDir += moveSpeed * Time.deltaTime * Vector3.up;
            }

            transform.Translate(moveDir);

            yaw += moveSpeed * Input.GetAxis("Mouse X");
            pitch -= moveSpeed * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }
}
