using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ResultPanelManeger : MonoBehaviour
{
    public void OnRestartButton() {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackToTitleButton() {
        SceneManager.LoadScene("TitleScene");
    }
}
