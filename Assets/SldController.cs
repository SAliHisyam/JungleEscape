using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private float max = 100.0f;
    [SerializeField] private Slider slider;
    [SerializeField] private float transitionTime = 2.0f; // Adjust as needed

    private Coroutine sliderCoroutine;

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider reference is not set.");
        }
    }

    public void StartSliderAnimation()
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
        }

        sliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float elapsedTime = 0.0f;
        float startValue = slider.value;
        float endValue = 1.0f;

        while (elapsedTime < transitionTime)
        {
            slider.value = Mathf.Lerp(startValue, endValue, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slider.value = endValue;

        // Once the animation is complete, reveal the loading screen
        LoadingScreenManager loadingScreenManager = FindObjectOfType<LoadingScreenManager>();

        if (loadingScreenManager != null)
        {
            loadingScreenManager.RevealLoadingScreen();
        }
        else
        {
            Debug.LogError("LoadingScreenManager not found in the scene.");
        }
    }
}
