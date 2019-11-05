using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flythrough : MonoBehaviour
{
    /*public Vector3 CameraStartPosition;
    public Vector3 CameraStartRotation;*/
    public float CameraMoveSpeed;
    public float CameraRotateSpeed;
    public bool FlythroughActive = false;
    PlayerObject PO;

    // Start is called before the first frame update
    void Start()
    {
        FlythroughActive = false;
        PO = FindObjectOfType<PlayerObject>();
    }
    Rect Q = Rect.zero;
    Rect W = Rect.zero;
    Rect E = Rect.zero;
    Rect A = Rect.zero;
    Rect S = Rect.zero;
    Rect D = Rect.zero;
    Rect Up = Rect.zero;
    Rect Down = Rect.zero;
    Rect Left = Rect.zero;
    Rect Right = Rect.zero;
    public Rect Toggle = Rect.zero;

   // public Text debugText; 
    public int Size = 67;    
    private void OnGUI()
    {
        if(FlythroughActive == true)
        {
            if (GUI.Button(Q, "Q")) Debug.Log("Q");
            if (GUI.Button(W, "W")) Debug.Log("W");
            if (GUI.Button(E, "E")) Debug.Log("E");
            if (GUI.Button(A, "A")) Debug.Log("A");
            if (GUI.Button(S, "S")) Debug.Log("S");
            if (GUI.Button(D, "D")) Debug.Log("D");
            if (GUI.Button(Up, "Up")) Debug.Log("Up");
            if (GUI.Button(Down, "Down")) Debug.Log("Down");
            if (GUI.Button(Left, "Left")) Debug.Log("Left");
            if (GUI.Button(Right, "Right")) Debug.Log("Right");
        }

        if (GUI.Button(Toggle, "Toggle"))
        {
            //Debug.Log("Toggle");
            FlythroughActive = !FlythroughActive;
            PO.SetCameraSetup();
        }
    }
    // Update is called once per frame
    void Update()
    {        
        Vector3 deltaRot = Vector3.zero;
        Vector2 mousePos = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        bool mouse = (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0));

        if(Input.touchCount > 0)
        {
            Debug.Log("touch pos: " + Input.touches[0].position);
        }


        Q = new Rect(0, Screen.height - (Size * 2), Size, Size);
        W = new Rect(Size, Screen.height - (Size * 2), Size, Size);
        E = new Rect(Size*2, Screen.height - (Size * 2), Size, Size);
        A = new Rect(0, Screen.height - Size, Size, Size);
        S = new Rect(Size, Screen.height - Size, Size, Size);
        D = new Rect(Size*2, Screen.height - Size, Size, Size);
        Up = new Rect(Screen.width - (Size * 2), Screen.height - (Size * 2), Size, Size);
        Down = new Rect(Screen.width - (Size * 2), Screen.height - Size, Size, Size);
        Left = new Rect(Screen.width - (Size * 3), Screen.height - Size, Size, Size);
        Right = new Rect(Screen.width - Size, Screen.height - Size, Size, Size);
        Toggle = new Rect(Screen.width / 2 - Size / 2, Screen.height - Size, Size * 2, Size);

        // Forward/Backward
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W) || (mouse && W.Contains(mousePos)))
        {            
            transform.Translate(Vector3.forward * Time.deltaTime * CameraMoveSpeed);
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S) || (mouse && S.Contains(mousePos)))
        {            
            transform.Translate(Vector3.back * Time.deltaTime * CameraMoveSpeed);
        }
        // Left/Right
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) || (mouse && A.Contains(mousePos)))
        {
            transform.Translate(Vector3.left * Time.deltaTime * CameraMoveSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) || (mouse && D.Contains(mousePos)))
        {
            transform.Translate(Vector3.right * Time.deltaTime * CameraMoveSpeed);
        }
        // Up/Down
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Q) || (mouse && Q.Contains(mousePos)))
        {
            transform.Translate(Vector3.up * Time.deltaTime * CameraMoveSpeed);
        }
        else if(Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E) || (mouse && E.Contains(mousePos)))
        {
            transform.Translate(Vector3.down * Time.deltaTime * CameraMoveSpeed);
        }                

        // Rotate Left/Right (Y)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow) || (mouse && Left.Contains(mousePos)))
        {
            deltaRot.y = -Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow) || (mouse && Right.Contains(mousePos)))
        {
            deltaRot.y = Time.deltaTime;
        }
        // Rotate Up/Down (X)
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow) || (mouse && Up.Contains(mousePos)))
        {
            deltaRot.x = -Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || (mouse && Down.Contains(mousePos)))
        {
            deltaRot.x = Time.deltaTime;
        }

        transform.eulerAngles += (deltaRot * CameraRotateSpeed);

       /* if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = CameraStartPosition;
            transform.eulerAngles = CameraStartRotation;
        }*/
    }
}
