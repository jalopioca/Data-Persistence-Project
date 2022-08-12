using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float MaxX = 4.0f;
    public float MaxZ = 3.8f;
    public Vector3 StartPosition = new Vector3(10f, 11.8f, 0);
    public float mouseSensitivity = 5f;

    private MainManager _mainManager;

    // Start is called before the first frame update
    void Start()
    {
        _mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        if (_mainManager.tilesAbove)
        {
            StartPosition = new Vector3(StartPosition.x, StartPosition.y - 1, StartPosition.z);
        }
        transform.position = StartPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        pos.z += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        if (pos.x > StartPosition.x + MaxX)
            pos.x = StartPosition.x + MaxX;
        else if (pos.x < StartPosition.x - MaxX)
            pos.x = StartPosition.x - MaxX;

        if (pos.z > StartPosition.z + MaxZ)
            pos.z = StartPosition.z + MaxZ;
        else if (pos.z < StartPosition.z - MaxZ)
            pos.z = StartPosition.z - MaxZ;

        transform.position = pos;
    }
}
