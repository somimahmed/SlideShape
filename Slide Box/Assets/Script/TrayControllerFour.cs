using UnityEngine;
using UnityEngine.EventSystems;

public class TrayControllerFour : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera cam;
    public Grid grid;
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

            if (touch.phase == TouchPhase.Began)
            {
                TryClickObject();
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

    void TryClickObject()
    {
    if (IsPointerOverUI()) return;

      Vector3 worldTouchPos = GetTouchWorldPosition();
      RaycastHit2D hit = Physics2D.Raycast(worldTouchPos, Vector2.zero);

      if (hit.collider != null && hit.collider.gameObject == gameObject)
      {
        isDragging = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        offset = transform.position - worldTouchPos;
      }
    }

    void DragObject()
    {
        if (!isDragging) return;

        Vector3 target = GetTouchWorldPosition() + offset;
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

    Vector3 GetTouchWorldPosition()
    {
        Vector3 touchPos = Input.GetTouch(0).position;
         touchPos.z=10f;
        return cam.ScreenToWorldPoint(touchPos);
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
