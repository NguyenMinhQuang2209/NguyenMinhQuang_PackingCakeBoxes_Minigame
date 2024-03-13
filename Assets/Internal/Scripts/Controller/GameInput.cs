using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private Vector2 touchPosition = Vector2.zero;
    private Vector2 endTouchPosition = Vector2.zero;

    [SerializeField] private float checkDistanceStart = 10f;

    private void Update()
    {
        if (!GameController.instance.IsPlaying())
        {
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;
                endTouchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                CheckArrow();
            }
        }
    }
    private void CheckArrow()
    {
        if (!StartCheckDistance())
        {
            return;
        }
        Direction direction;
        if (VerticalMovement() > HorizontalMovement())
        {
            direction = touchPosition.y > endTouchPosition.y ? Direction.Down : Direction.Up;
        }
        else
        {
            direction = touchPosition.x > endTouchPosition.x ? Direction.Left : Direction.Right;
        }
        CheckDirection(direction);
    }
    public float VerticalMovement()
    {
        return Mathf.Abs(touchPosition.y - endTouchPosition.y);
    }
    public float HorizontalMovement()
    {
        return Mathf.Abs(touchPosition.x - endTouchPosition.x);
    }

    public bool StartCheckDistance()
    {
        return VerticalMovement() > checkDistanceStart || HorizontalMovement() > checkDistanceStart;
    }

    public void CheckDirection(Direction direction)
    {
        GameController.instance.CheckDirection(direction);
    }
}
public enum Direction
{
    Up,
    Left,
    Right,
    Down
}