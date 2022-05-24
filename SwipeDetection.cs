using UnityEngine;

public enum Swipes { None, Up, Down, Left, Right };

public class SwipeDetection : MonoBehaviour {
    public float minSwipeLength = 200f;
    Vector2 currentSwipe;

    private Vector2 fingerStart;
    private Vector2 fingerEnd;

    private static Swipes direction;

    public static Swipes Direction { get => direction; private set => direction = value; }

    void Update() {
        DetectSwipes();
    }

    public void DetectSwipes() {
        if (Input.GetMouseButtonDown(0)) {
            fingerStart = Input.mousePosition;
            fingerEnd = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            fingerEnd = Input.mousePosition;

            currentSwipe = new Vector2(fingerEnd.x - fingerStart.x, fingerEnd.y - fingerStart.y);

            if (currentSwipe.magnitude < minSwipeLength) {
                Direction = Swipes.None;
                return;
            }

            float angle = (Mathf.Atan2(currentSwipe.y, currentSwipe.x) / (Mathf.PI));
            if (angle > 0.375f && angle < 0.625f) {
                Direction = Swipes.Up;
            } else if (angle < -0.375f && angle > -0.625f) {
                Direction = Swipes.Down;
            } else if (angle < -0.875f || angle > 0.875f) {
                Direction = Swipes.Left;
            } else if (angle > -0.125f && angle < 0.125f) {
                Direction = Swipes.Right;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            Direction = Swipes.None;
        }
    }
}