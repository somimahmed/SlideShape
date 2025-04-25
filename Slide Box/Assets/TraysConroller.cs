using UnityEngine;
using UnityEngine.EventSystems;

public class TraysConroller : MonoBehaviour
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
        PreviusPosition=transform.position;
    }

    void OnMouseDown()
    {
        if (IsPointerOverUI()) return;

        isDragging = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        offset = transform.position - GetMouseWorldPos();
        
    }

    void OnMouseDrag()
    {
        if (!isDragging ) return; 

        Vector3 target = GetMouseWorldPos() + offset;
        gameObject.GetComponent<Rigidbody2D>().MovePosition(target);
    }

    void OnMouseUp()
    {
        if (!isDragging) return;

        isDragging = false;
         
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        Vector3 worldSnap = grid.CellToWorld(cellPos);
        gameObject.GetComponent<Rigidbody2D>().MovePosition(worldSnap); 
       
        Invoke("MakeRigidBodyStatic", 0.1f);
        PreviusPosition=worldSnap;
        
    }
    private void MakeRigidBodyStatic(){
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
     

  

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(mouse);
        
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
    

    
}
