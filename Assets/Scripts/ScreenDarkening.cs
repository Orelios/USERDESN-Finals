using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class ScreenDarkening : MonoBehaviour
{
    private ActionsManager actionsManager;
    private PostProcessVolume postProcessVolume;

    private void Awake()
    {
        actionsManager = FindObjectOfType<ActionsManager>();
        postProcessVolume = GetComponent<PostProcessVolume>();
    }

    private void SetVignetteIntensity()
    {
        int totalNumActions = actionsManager.ActionCounterSO.InitialNumberOfActions;
        int currentNumActions = actionsManager.NumActions;
        
        float intensity = 1f - (float)currentNumActions / (float)totalNumActions;

        postProcessVolume.profile.GetSetting<Vignette>().intensity.value = intensity;
    }

    private void OnEnable()
    {
        ActionsManager.onActionCounterUpdated += SetVignetteIntensity;
    }

    private void OnDisable()
    {
        ActionsManager.onActionCounterUpdated -= SetVignetteIntensity;
    }
}
