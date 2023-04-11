using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour, IDamageable
{

    public Texture[] textures;
    private MeshRenderer render;

    public GameObject expEffect;
    private Rigidbody rigid;
    private int hitCount = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        render = GetComponentInChildren<MeshRenderer>();
    }
    private void Start()
    {
        int idx = Random.Range(0, textures.Length);
        render.material.mainTexture = textures[idx];
    }
    public void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (++hitCount == 3)
        {
            ExpBarrel();
        }
        else
            AttackBarrel(-hitNormal, hitPosition);
    }

    private void AttackBarrel(Vector3 hitNormal, Vector3 hitPos)
    {
        EffectManager.Instacne.PlayHitEffect(hitPos, hitNormal/*, effectType: EffectManager.EffectType.PLESH*/);
        rigid.AddForce(hitNormal * 50, ForceMode.Impulse);
    }

    private void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, transform.position, transform.rotation);
        Destroy(exp, 2);

        rigid.mass = 1;
        rigid.AddForce(Vector3.up * 1500);
        Destroy(gameObject, 2);
    }
}
