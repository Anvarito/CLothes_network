using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputController))]
public class Player : MonoBehaviour
{
    private Animator _animator;
    private int _animIDwalk;

    [Header("Game elements")]
    public GameObject topNaked;
    public GameObject middleNaked;
    public GameObject bottomNaked;

    public GameObject topDressed;
    public GameObject middleDressed;
    public GameObject bottomDressed;

    private bool _isTopNaked = true;
    private bool _isMiddleNaked = true;
    private bool _isBottomNaked = true;

    [Space(1)]
    [Header("Input controller")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float PlayerSpeed;
    private InputController _input;
    private CharacterController _controller;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    [Space(1)]
    [Header("Camera")]
    public float CameraAngleOverride = 0.0f;
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    public GameObject CinemachineCameraTarget;
    private GameObject _mainCamera;

    void Start()
    {
        _input = GetComponent<InputController>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _animIDwalk = Animator.StringToHash("walk");
        _mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMoving();
        CharacterAnimation();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    public void ChangeTop()
    {
        topNaked.SetActive(_isTopNaked);
        topDressed.SetActive(!_isTopNaked);
        _isTopNaked = !_isTopNaked;
    }

    public void ChangeMiddle()
    {
        middleNaked.SetActive(_isMiddleNaked);
        middleDressed.SetActive(!_isMiddleNaked);
        _isMiddleNaked = !_isMiddleNaked;
    }

    public void ChangeBottom()
    {
        bottomNaked.SetActive(_isBottomNaked);
        bottomDressed.SetActive(!_isBottomNaked);
        _isBottomNaked = !_isBottomNaked;
    }


    private void CharacterMoving()
    {
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * PlayerSpeed * Time.deltaTime);
        }
    }

    private void CharacterAnimation()
    {
        bool isWalk = _input.move != Vector2.zero;
        _animator.SetBool(_animIDwalk, isWalk);
    }
    private void CameraRotation()
    {
        if (_input.look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _input.look.x;
        }
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
