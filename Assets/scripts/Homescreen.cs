using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Homescreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Gamescreen_1()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Quittohome()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
