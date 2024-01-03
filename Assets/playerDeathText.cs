using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerkiller : MonoBehaviour
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

    // Tambahkan variabel untuk efek waktu yang berlangsung
    private bool isTimeEffected = false;
    private float timeEffectDuration = 7f;
    private float timeEffectStartTime = 0f;

    void Start()
    {
        playerMovement = GetComponent<playermovement>();
        playAgainButton.onClick.AddListener(PlayAgain);
        nextLevelButton.onClick.AddListener(completeLevel);
        playAgainFinishButton.onClick.AddListener(playAgainAfterFinish);
        playAgainButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        playAgainFinishButton.gameObject.SetActive(false);

        // Temukan AudioManager di scene (pastikan AudioManager ada di scene)
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // Periksa apakah efek waktu masih berlangsung
        if (isTimeEffected && Time.time - timeEffectStartTime >= timeEffectDuration)
        {
            // Kembalikan Time.timeScale ke nilai normal setelah efek waktu berakhir
            Time.timeScale = 1f;
            isTimeEffected = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("time"))
        {
            // Hancurkan objek dengan tag "time"
            Destroy(collision.gameObject);

            // Atur efek waktu
            Time.timeScale = 0.75f;
            isTimeEffected = true;
            timeEffectStartTime = Time.time;
        }
        else if (collision.CompareTag("spikes"))
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
        else if (collision.CompareTag("finish"))
        {
            gameOverText3.gameObject.SetActive(true);
            finishSoundEffect.Play();
            finish2SoundEffect.Play();

            if (CameraLeo != null)
            {
                CameraLeo.StopFollowingPlayer();
            }
            // Matikan semua suara termasuk footstep dan jump
            if (audioManager != null)
            {
                audioManager.GetComponent<AudioSource>().Stop();
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
        UnlockNewLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void playAgainAfterFinish()
    {
        UnlockNewLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
        }
    }






}
