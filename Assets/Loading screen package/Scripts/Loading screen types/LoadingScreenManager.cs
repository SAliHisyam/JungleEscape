using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    private Animator _animatorComponent;

    private void Start()
    {
        // Pastikan bahwa Animator sudah ada di objek atau parent objek
        _animatorComponent = GetComponent<Animator>();

        if (_animatorComponent == null)
        {
            Debug.LogError("Animator component not found. Make sure it is attached to the object or its parent.");
            return;
        }

        // Hapus baris ini jika Anda tidak ingin menyembunyikan layar di fungsi Start dan panggil di tempat lain
        HideLoadingScreen();
    }

    public void RevealLoadingScreen()
    {
        if (_animatorComponent != null)
        {
            _animatorComponent.SetTrigger("Reveal");
        }
        else
        {
            Debug.LogError("Animator component is not initialized.");
        }
    }

    public void HideLoadingScreen()
    {
        // Panggil fungsi ini jika Anda ingin mulai menyembunyikan layar
        if (_animatorComponent != null)
        {
            _animatorComponent.SetTrigger("Hide");
        }
        else
        {
            Debug.LogError("Animator component is not initialized.");
        }
    }

    public void OnFinishedReveal()
    {
        // TODO: hapus ini dan muat scene Anda sendiri
        DemoSceneManager demoSceneManager = transform.parent.GetComponent<DemoSceneManager>();
        if (demoSceneManager != null)
        {
            demoSceneManager.OnLoadingScreenRevealed();
        }
        else
        {
            Debug.LogError("DemoSceneManager component is not initialized.");
        }
    }

    public void OnFinishedHide()
    {
        // TODO: hapus ini dan panggil fungsi Anda
        DemoSceneManager demoSceneManager = transform.parent.GetComponent<DemoSceneManager>();
        if (demoSceneManager != null)
        {
            demoSceneManager.OnLoadingScreenHided();
        }
        else
        {
            Debug.LogError("DemoSceneManager component is not initialized.");
        }
    }
}
