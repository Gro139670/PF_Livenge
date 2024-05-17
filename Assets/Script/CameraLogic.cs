using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [SerializeField] GameObject _UpObject;
    [SerializeField] GameObject _DownObject;

    [SerializeField] float _CameraSpeed = 1;

    private MouseHover _UP;
    private MouseHover _DOWN;
    private Vector3 _Posistion;

    public MouseHover UP
    { set {  _UP = value; } }
    public MouseHover Down
    { set { _DOWN = value; } }
    private void Awake()
    {
        _Posistion = transform.position;
        _UP = _UpObject?.GetComponent<MouseHover>();
        _DOWN = _DownObject?.GetComponent<MouseHover>();
        if(_CameraSpeed <= 0)
        {
            _CameraSpeed = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_UP?.IsMouseHover == true)
        {
            _Posistion.y += 1 * _CameraSpeed;
            if (_Posistion.y > 18)
            {
                _Posistion.y = 18;
            }
            transform.position = _Posistion;
        }

        if(_DOWN?.IsMouseHover == true)
        { 
            _Posistion.y -= 1 * _CameraSpeed;
            if (_Posistion.y < -10)
            {
                _Posistion.y = -10;
            }
            transform.position = _Posistion;
        }

    }
}
