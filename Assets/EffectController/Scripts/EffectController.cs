using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;

    [SerializeField] Transform particleParent;
    [SerializeField] Material blinkMaterial;
    [SerializeField] AudioSource playerAudioSource;

    void Awake()
    {
        instance = this;
    }

    public void InstantiateParticle(GameObject particlePrefab, Vector3 position)
    {
        GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity, particleParent);
        //ref da particula caso seja necessario fazer algo específico com ela
    }

    public void StopLoopingParticle(GameObject particle)
    {
        particle.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void BlinkMaterial(MeshRenderer meshRenderer)
    {
        StartCoroutine(BlinkMaterial(meshRenderer, .2f));
    }

    IEnumerator BlinkMaterial(MeshRenderer meshRenderer, float blinkTime)
    {
        if (meshRenderer.material != blinkMaterial)
        {
            Material startMaterial = meshRenderer.material;
            meshRenderer.material = blinkMaterial;
            yield return new WaitForSeconds(blinkTime);
            meshRenderer.material = startMaterial;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        playerAudioSource.clip = clip;
        playerAudioSource.Play();
    }
}
