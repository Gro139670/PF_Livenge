using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    protected bool _IsMouseDown = false;
    protected bool _IsMouseHover = false;
    protected SpriteRenderer _Renderer;


    public bool IsMouseDown
    {
        get
        {
            bool result = _IsMouseDown;
            if (_IsMouseDown == true)
            {
                _IsMouseDown = false;
            }

            return result;
        }
    }

    public bool IsMouseHover
    {
        get
        {
            return _IsMouseHover;
        }
    }


    protected virtual void Awake()
    {
        _Renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (_IsMouseHover == true)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                if (BattleManager.GetInstance().IsBattleStart == false)
                {
                    _IsMouseDown = true;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Mouse"))
        {
            _IsMouseHover = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Mouse"))
        {
            _IsMouseHover = false;
        }
    }
}
