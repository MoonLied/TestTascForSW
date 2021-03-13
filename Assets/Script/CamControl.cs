using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public static CamControl Instance { get; private set; }

    private const float _camFlyTime = 1;
    private const float _toRadial = 0.0174532925199433f;

    private float _x;
    private float _y;
    private float _z;

    private float _deltaX;
    private float _deltaY;
    private float _deltaZ;
    private float _timeFly;

    private Vector3 _shapePosition;
    private Vector3 _shapeFocus;
    private Vector3 _freePosition;
    private Quaternion _freeRotaton;

    private bool _toShape;
    private bool _isFree = true;
    private bool _needUpdate;


    void Start()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (_isFree)
        {
            _x = 0;
            _z = 0;
            if (Input.GetKey(KeyCode.A)) _x -= 1;
            if (Input.GetKey(KeyCode.W)) _z += 1;
            if (Input.GetKey(KeyCode.D)) _x += 1;
            if (Input.GetKey(KeyCode.S)) _z -= 1;
            _y = Input.GetAxis("Mouse ScrollWheel");
            GoCamera(_x, _z, _y);
        }
        else
        {
            if (!_needUpdate) return;
            _timeFly -= Time.deltaTime;
            if (_toShape)
            {
                CamUpdateToShape();
            }
            else
            {
                CamUpdateToFree();
            }
        }
    }

    private void CamUpdateToShape()
    {
        if (_timeFly > 0)
        {
            Vector3 V = transform.position;
            transform.position = new Vector3(V.x + _deltaX * Time.deltaTime, V.y + _deltaY * Time.deltaTime, V.z + +_deltaZ * Time.deltaTime);
            Vector3 deltaVector = _shapeFocus - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(deltaVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, _camFlyTime - _timeFly);
        }
        else
        {
            _needUpdate = false;
            transform.position = _shapePosition;
            Vector3 deltaVector = _shapeFocus - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(deltaVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 1);
        }
    }

    private void CamUpdateToFree()
    {
        if (_timeFly > 0)
        {
            Vector3 V = transform.position;
            transform.position = new Vector3(V.x + _deltaX * Time.deltaTime, V.y + _deltaY * Time.deltaTime, V.z + +_deltaZ * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _freeRotaton, _camFlyTime - _timeFly);
        }
        else
        {
            _needUpdate = false;
            _isFree = true;
            transform.position = _freePosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, _freeRotaton, Time.deltaTime * 100);
        }
    }

    private void GoCamera(float valueX, float valueZ, float valueY)
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        float x = transform.position.x + valueX * Time.deltaTime * 10;
        float z = transform.position.z + valueZ * Time.deltaTime * 10;
        float y = transform.position.y + valueY * Time.deltaTime * 100;
        Vector3 pos = new Vector3(x, y, z);
        transform.position = pos;
    }

    public void GoShowShape(Vector3 position, Vector3 focusPoint)
    {
        _isFree = false;
        _toShape = true;
        _needUpdate = true;
        _shapePosition = position;
        _shapeFocus = focusPoint;
        _freePosition = transform.position;
        _freeRotaton = transform.rotation;
        //вачисляем нужную позицию камеры
        _deltaX = position.x - transform.position.x;
        _deltaY = position.y - transform.position.y;
        _deltaZ = position.z - transform.position.z;
        _timeFly = _camFlyTime;
        _deltaX /= _timeFly;
        _deltaY /= _timeFly;
        _deltaZ /= _timeFly;
    }

    public void ExitShowShape()
    {
        _toShape = false;
        _needUpdate = true;
        _deltaX = _freePosition.x - transform.position.x;
        _deltaY = _freePosition.y - transform.position.y;
        _deltaZ = _freePosition.z - transform.position.z;
        _timeFly = 1;
    }
}
