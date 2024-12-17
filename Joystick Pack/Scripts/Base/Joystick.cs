using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; } }
    public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    public InputAction moveAction;

    [SerializeField] protected float handleRange = 1;  // Changed to protected
    [SerializeField] protected float deadZone = 0;     // Changed to protected
    [SerializeField] protected RectTransform background = null; // Changed to protected
    [SerializeField] protected RectTransform handle = null;     // Changed to protected
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    protected Canvas canvas;                           // Changed to protected
    protected Vector2 input = Vector2.zero;            // Changed to protected
    private bool isDragging;

    protected virtual void Start()
    {
        HandleRange = handleRange;
        DeadZone = deadZone;
        canvas = GetComponentInParent<Canvas>();

        if (moveAction != null)
        {
            moveAction.Enable();
            moveAction.performed += OnMoveInput;
            moveAction.canceled += OnMoveInput;
        }
    }

    private void OnDestroy()
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnMoveInput;
            moveAction.canceled -= OnMoveInput;
            moveAction.Disable();
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!isDragging)
        {
            input = context.ReadValue<Vector2>();
            UpdateHandlePosition();
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        OnDrag(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        UpdateHandlePosition();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void UpdateHandlePosition()
    {
        Vector2 radius = background.sizeDelta / 2;
        handle.anchoredPosition = input * radius * handleRange;
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0) return value;
        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f) return 0;
                else return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f) return 0;
                else return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            return value > 0 ? 1 : (value < 0 ? -1 : 0);
        }
    }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    public AxisOptions AxisOptions { get { return axisOptions; } set { axisOptions = value; } }
    public bool SnapX { get { return snapX; } set { snapX = value; } }
    public bool SnapY { get { return snapY; } set { snapY = value; } }
}

public enum AxisOptions { Both, Horizontal, Vertical }
