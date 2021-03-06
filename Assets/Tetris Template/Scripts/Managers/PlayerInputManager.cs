﻿//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games                                                        *
//   * Facebook: https://goo.gl/5YSrKw                                                *
//   * Contact me: https://goo.gl/y5awt4                                              *
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;

public enum InputMethod
{
    KeyboardInput,
    MouseInput,
    TouchInput
}

public class PlayerInputManager : MonoBehaviour
{
    public bool isActive;
    public InputMethod inputType;
    float interval;
    float first_inteval = 0.2f;
    float second_inteval = 0.05f;
    int key_state = 0;

    void Awake()
    {
        interval = 0.0f;
    }

    void Update()
    {
        if (isActive)
        {
            if (inputType == InputMethod.KeyboardInput)
                KeyboardInput();
            else if (inputType == InputMethod.MouseInput)
                MouseInput();
            else if (inputType == InputMethod.TouchInput)
                TouchInput();
        }
    }

    #region KEYBOARD
    void KeyboardInput()
    {
        if(Managers.Game.currentShape == null){
            return;
        }
        if (Input.GetKeyDown(KeyCode.A) 
            || Input.GetKeyDown(KeyCode.X) 
            )
        {
            Managers.Game.currentShape.movementController.RotateClockWise(false);
        }
        else if (Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.Z)
            )
        {
            Managers.Game.currentShape.movementController.RotateClockWise(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
                Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                interval = first_inteval;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
                Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                interval = first_inteval;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(interval <= 0.0f)
            {
                Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                interval = second_inteval;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow)) 
        {
            if(interval <= 0.0f)
            {
                Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                interval = second_inteval;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Managers.Game.currentShape != null)
            {
                isActive = false;
                Managers.Game.currentShape.movementController.InstantFall();
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Managers.Game.currentShape.movementController.MoveDown();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift)
             || Input.GetKeyDown(KeyCode.Space)
             )
        {
            isActive = false;
            Managers.Game.currentShape.movementController.SetHold();
            Managers.Spawner.Hold();
        }

        if(interval >= 0.0f){
            interval -= Time.deltaTime;
        }
    }
    #endregion

    #region MOUSE
    Vector2 _startPressPosition;
    Vector2 _endPressPosition;
    Vector2 _currentSwipe;
    float _buttonDownPhaseStart;
    public float tapInterval;

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            _startPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _buttonDownPhaseStart = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - _buttonDownPhaseStart > tapInterval)
            {
                //save ended touch 2d point
                _endPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                //normalize the 2d vector
                _currentSwipe.Normalize();

                //swipe left
                if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                }
                //swipe right
                if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                }

                //swipe down
                if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    if (Managers.Game.currentShape != null)
                    {
                        isActive = false;
                        Managers.Game.currentShape.movementController.InstantFall();
                    }
                }
            }
            else
            {
                if (_startPressPosition.x < Screen.width / 2)
                    Managers.Game.currentShape.movementController.RotateClockWise(false);
                else
                    Managers.Game.currentShape.movementController.RotateClockWise(true);
            }
        }
    }
    #endregion

    #region TOUCH
    void TouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _startPressPosition = touch.position;
                _endPressPosition = touch.position;
                _buttonDownPhaseStart = Time.time;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                _endPressPosition = touch.position;

            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time - _buttonDownPhaseStart > tapInterval)
                {
                    //save ended touch 2d point
                    _endPressPosition = new Vector2(touch.position.x, touch.position.y);

                    //create vector from the two points
                    _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                    //normalize the 2d vector
                    _currentSwipe.Normalize();

                    //swipe left
                    if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    {
                        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                    }
                    //swipe right
                    if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    {
                        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                    }

                    //swipe down
                    if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    {
                        if (Managers.Game.currentShape != null)
                        {
                            isActive = false;
                            Managers.Game.currentShape.movementController.InstantFall();
                        }
                    }
                }
                else /*if (_currentSwipe.x + _currentSwipe.y< 0.5f */
                {
                    if (_startPressPosition.x < Screen.width / 2)
                        Managers.Game.currentShape.movementController.RotateClockWise(false);
                    else
                        Managers.Game.currentShape.movementController.RotateClockWise(true);
                }
            }
        }

    }
    #endregion

}
