using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_instance;
    public static EffectManager Instacne
    {
        get
        {
            if (m_instance == null) m_instance = FindObjectOfType<EffectManager>();
            return m_instance;
        }
    }
    public enum EffectType
    {
        COMMON,
        PLESH,
    }
    public ParticleSystem commonHitEffect;
    public ParticleSystem fleshHitEffect;

    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null, EffectType effectType = EffectType.COMMON)
    {
        var targetEffect = commonHitEffect;
        if (effectType == EffectType.PLESH)
            targetEffect = fleshHitEffect;

        var effect = Instantiate(targetEffect, pos, Quaternion.LookRotation(normal));
        if (parent != null) effect.transform.SetParent(parent);
        effect.Play();
    }
}
