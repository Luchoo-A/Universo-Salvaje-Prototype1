using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    public float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private MaterialPropertyBlock propBlock;
    private Coroutine flashRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        propBlock = new MaterialPropertyBlock();
        ResetFlash();
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        sr.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_FlashAmount", 1f);
        sr.SetPropertyBlock(propBlock);

        yield return new WaitForSeconds(flashDuration);

        sr.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_FlashAmount", 0f);
        sr.SetPropertyBlock(propBlock);
    }

    public void SetTint(Color color)
    {
        sr.GetPropertyBlock(propBlock);
        propBlock.SetColor("_TintColor", color);
        sr.SetPropertyBlock(propBlock);
    }

    public void ResetTint()
    {
        SetTint(Color.white);
    }

    public void ResetFlash()
    {
        if (propBlock == null) propBlock = new MaterialPropertyBlock();
        sr.GetPropertyBlock(propBlock);

        propBlock.SetFloat("_FlashAmount", 0f);
        sr.SetPropertyBlock(propBlock);
    }

    public void ResetAll()
    {
        if (propBlock == null) propBlock = new MaterialPropertyBlock();
        sr.GetPropertyBlock(propBlock);

        propBlock.SetFloat("_FlashAmount", 0f);
        propBlock.SetColor("_TintColor", Color.white);
        sr.SetPropertyBlock(propBlock);
    }
}
