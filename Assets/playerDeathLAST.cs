using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerDeathLAST : MonoBehaviour
{
    public Text gameOverText;
    public Text gameOverText3;
    public Transform respawnPoint;
    public playermovement playerMovement;
    public CameraLeo CameraLeo;
    public Button playAgainButton;
    public Button nextLevelButton;
    public Button playAgainFinishButton;
    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] public AudioSource finishSoundEffect;
    [SerializeField] private AudioSource finish2SoundEffect;

    // Tambahkan variabel untuk menyimpan referensi ke AudioManager
    public AudioManager audioManager;
    public AudioSource footStepSoundEffect;
    public AudioSource jumpSoundEffect;

    private bool isFootstepPlayingBeforeDeath;
    private bool isJumpSoundPlayingBeforeDeath;

    void Start()
    {
        playerMovement = GetComponent<playermovement>();
        playAgainButton.onClick.AddListener(PlayAgain);
        nextLevelButton.onClick.AddListener(completeLevel);
        playAgainFinishButton.onClick.AddListener(playAgainAfterFinish);
        playAgainButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        playAgainFinishButton.gameObject.SetActive(false) ;

        // Temukan AudioManager di scene (pastikan AudioManager ada di scene)
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("spikes"))
        {
            // Menampilkan teks game over
            gameOverText.gameObject.SetActive(true);

            // Menampilkan tombol Play Again
            playAgainButton.gameObject.SetActive(true);

            // Menghentikan Gerakan
            Time.timeScale = 0;
            dieSoundEffect.Play();

            // Catat status pemutaran suara footstep dan jump sebelum kematian
            isFootstepPlayingBeforeDeath = footStepSoundEffect.isPlaying;
            isJumpSoundPlayingBeforeDeath = jumpSoundEffect.isPlaying;

            // Matikan semua suara termasuk footstep dan jump
            if (audioManager != null)
            {
                audioManager.GetComponent<AudioSource>().Stop();
            }
            footStepSoundEffect.Stop();
            jumpSoundEffect.Stop();
        }

        if (collision.CompareTag("finish"))
        {
            gameOverText3.gameObject.SetActive(true);
            finishSoundEffect.Play();
            finish2SoundEffect.Play();

            if (CameraLeo != null)
            {
                CameraLeo.StopFollowingPlayer();
            }
            
            footStepSoundEffect.Stop();
            footStepSoundEffect.volume = 0;
            jumpSoundEffect.Stop();

            //Next level
            nextLevelButton.gameObject.SetActive(true);
            // Menampilkan tombol Play Again
            playAgainFinishButton.gameObject.SetActive(true);
        }
    }

    void PlayAgain()
    {
        // Set posisi karakter ke respawnPoint
        transform.position = respawnPoint.position;

        // Menyembunyikan teks game over
        gameOverText.gameObject.SetActive(false);

        // Mengembalikan waktu ke keadaan normal
        Time.timeScale = 1f;

        // Menonaktifkan tombol Play Again setelah respawn
        playAgainButton.gameObject.SetActive(false);

        // Hidupkan kembali semua suara termasuk footstep dan jump
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioSource>().Play();
        }

        // Menghidupkan kembali suara footstep dan jump sesuai status sebelum kematian
        if (isFootstepPlayingBeforeDeath)
        {
            footStepSoundEffect.Play();
        }
        if (isJumpSoundPlayingBeforeDeath)
        {
            jumpSoundEffect.Play();
        }
    }

    private void completeLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 10);
    }

    private void playAgainAfterFinish()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
