using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MenuController : MonoBehaviour
{
    public bool tilesAbove;
    public string Username;
    public InputField nameField;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Username = nameField.text;
        SceneManager.LoadScene(1);
    }
}
