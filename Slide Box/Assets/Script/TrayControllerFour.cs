using UnityEngine;
using UnityEngine.EventSystems;

public class TrayControllerFour : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera cam;
    public Grid grid;
    private bool isColliding = false;
    Vector3 PreviusPosition;

    void Start()
    {
        cam = Camera.main;
        PreviusPosition = transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = cam.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                TryClickObject(touchPosition);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                DragObject();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                DropObject();
            }
        }
    }

    void TryClickObject(Vector3 touchPosition)
    {
        if (IsPointerOverUI()) return;

        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            // Only THIS object is touched
            isDragging = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            offset = transform.position - GetMouseWorldPos();
        }
    }

    void DragObject()
    {
        if (!isDragging) return;

        Vector3 target = GetMouseWorldPos() + offset;
        GetComponent<Rigidbody2D>().MovePosition(target);
    }

    void DropObject()
    {
        if (!isDragging) return;

        isDragging = false;
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        Vector3 worldSnap = grid.CellToWorld(cellPos);
        GetComponent<Rigidbody2D>().MovePosition(worldSnap);
        Invoke("MakeRigidBodyStatic", 0.1f);
        PreviusPosition = worldSnap;
    }

    private void MakeRigidBodyStatic()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    Vector3 GetMouseWorldPos()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 TouchPos = touch.position;
        TouchPos.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(TouchPos);
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
