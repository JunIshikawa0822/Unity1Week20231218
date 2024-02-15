using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   

public class SceneTranslator : MonoBehaviour
{
    public Image StartButton;
    Animator animator;

    private void Start()
    {
        Time.timeScale = 1.0f;
        animator = StartButton.GetComponent<Animator>();
    }
    public void EnterButton()
    {
        animator.SetBool("Enter", true);
    }

    public void ExitButton()
    {
        animator.SetBool("Enter", false);
    }

    public void SceneToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void SceneToStart()
    {
        SceneManager.LoadScene("Start");
    }

}
