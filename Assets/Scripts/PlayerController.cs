using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{

    public bool isMoving = false;
    public float speed = 5f;

    public InputActionAsset inputActionAsset;

    private InputActionMap actionMap;
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        actionMap = inputActionAsset.FindActionMap("Player");
        actionMap.Enable();

        if (!IsOwner)
        {
            Camera camera = GetComponentInChildren<Camera>();
            camera.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        float horizontalAxis = 0;
        float verticalAxis = 0;
        if (actionMap.FindAction("move").IsPressed())
        {
            Vector2 movement = actionMap.FindAction("Move").ReadValue<Vector2>();
            horizontalAxis = movement.x;
            verticalAxis = movement.y;
        }
        MovePlayer(horizontalAxis, verticalAxis);
    }

    private void MovePlayer(float horizontalAxis, float verticalAxis)
    {
        if (!isMoving)
        {
            if (horizontalAxis != 0 || verticalAxis != 0)
            {
                isMoving = true;
                StartCoroutine(moveTo(horizontalAxis, verticalAxis, speed, () => isMoving = false));
            }
        }
       
    }

    public IEnumerator moveTo(float horizontalAxis, float verticalAxis, float speedModifier, Action moveCompleteCallback)
    {
        Vector3 location = getNewPosition(horizontalAxis, verticalAxis);
       
        while (transform.position != location)
        {
            transform.position = Vector3.MoveTowards(transform.position, location, speedModifier * Time.deltaTime);
            yield return null;
        }
     
        moveCompleteCallback();
    }

    private Vector3 getNewPosition(float horizontalAxis, float verticalAxis)
    {
        float verticalMovement = 0f;
        float horizontalMovement = 0f;
        if (horizontalAxis > 0f)
        {
            horizontalMovement = Mathf.CeilToInt(horizontalAxis);
        }
        if (horizontalAxis < 0f)
        {
            horizontalMovement = Mathf.FloorToInt(horizontalAxis);
        }
        if (verticalAxis > 0f)
        {
            verticalMovement = Mathf.CeilToInt(verticalAxis);
        }
        if (verticalAxis < 0f)
        {
            verticalMovement = Mathf.FloorToInt(verticalAxis);
        }
        if (verticalMovement != 0 && horizontalMovement != 0)
        {
            verticalMovement = verticalMovement / 2;
            horizontalMovement = horizontalMovement / 2;
        }

        Vector3 inputPosition = new Vector3(horizontalMovement, verticalMovement, 0f);

        if (grid != null)
        {
            Vector3Int cellPosition = grid.WorldToCell(transform.position + inputPosition);
            // Put inside center of cell
            float halfCell = grid.cellSize.y / 2;
            return grid.CellToWorld(cellPosition) + new Vector3(0, halfCell, 0);
        }

        return transform.position + inputPosition;

    }
}
