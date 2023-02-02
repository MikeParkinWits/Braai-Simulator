using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Instantiables")]
    [SerializeField] Transform playerCamera = null;

    [Header("Customizable Variables")]
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float shiftMoveSpeed = 6f;
    [SerializeField] float normalMoveSpeed = 3.5f;
    [SerializeField][Range(0.0f, 0.5f)] float playerIntertia = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float cameraInertia = 0.03f;
    [SerializeField] float gravityScale = -9.8f;

    private float _cameraPitch = 0.0f;
    private bool _lockCursor = true;
    private CharacterController _charController;
    private Vector2 _playerCurrentDir = Vector2.zero;
    private Vector2 _playerCurrDirVelocity = Vector2.zero;

    private Vector2 _cameraCurrentDir = Vector2.zero;
    private Vector2 _cameraCurrDirVelocity = Vector2.zero;

    private float downwardVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        instantiateObjects();

        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void instantiateObjects()
    {
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GameIsPaused)
        {
            MouseLook();
            Movement();
            
            moveSpeed = normalMoveSpeed;
            
        }
    }

    void MouseLook()
    {
        Vector2 targetMouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        _cameraCurrentDir = Vector2.SmoothDamp(_cameraCurrentDir, targetMouse, ref _cameraCurrDirVelocity, cameraInertia);

        _cameraPitch -= _cameraCurrentDir.y * mouseSensitivity;

        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * _cameraPitch;

        transform.Rotate(Vector3.up * _cameraCurrentDir.x * mouseSensitivity);
    }

    void Movement()
    {
        Vector2 moveTarget = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveTarget.Normalize();
        _playerCurrentDir = Vector2.SmoothDamp(_playerCurrentDir, moveTarget, ref _playerCurrDirVelocity, playerIntertia);

        if (_charController.isGrounded)
        {
            downwardVelocity = 0f;
        }

        downwardVelocity += gravityScale * Time.deltaTime;

        Vector3 velocity = (transform.forward * _playerCurrentDir.y + transform.right * _playerCurrentDir.x) * moveSpeed + Vector3.up * downwardVelocity;

        _charController.Move(velocity * Time.deltaTime);
    }
}
