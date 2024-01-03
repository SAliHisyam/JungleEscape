using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PAUSEMENU : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isGameOver = false;

    // Panggil metode ini saat permainan berakhir
    public void GameOver()
    {
        isGameOver = true;
        // Sembunyikan tombol Pause saat permainan berakhir
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        // Tampilkan menu pause hanya jika permainan belum berakhir
        if (!isGameOver)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("mainmenu");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
