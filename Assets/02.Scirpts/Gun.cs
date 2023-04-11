using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; private set; }

    private AudioSource audioSource;

    public AudioClip shootClip;
    public AudioClip reloadClip;

    public Transform firePosition; //�Ѿ˳����� ��ġ�� ����
    public ParticleSystem muzzleFlashEffect;
    public float bulletLineEffectTime = 0.03f;

    private LineRenderer bulletLineRenderer;
    public float damage = 25;

    public float fireDistance = 100f; //�߻簡�� �Ÿ�

    public int magCapacity = 30; //źâ �뷮
    public int magAmmo; //���� źâ�� �ִ� ź���
    public float timeBetFire = 0.12f; //ź�� �߻� ����
    public float reloadTime = 1.8f; //������ �ҿ�ð�
    private float lastFireTime; //���� ���������� �߻��� �ð�

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }
    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }
    private IEnumerator ShootEffect(Vector3 hitPosition)
    {
        audioSource.clip = shootClip;
        audioSource.Play();

        muzzleFlashEffect.Play();
        bulletLineRenderer.SetPosition(0, firePosition.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(bulletLineEffectTime);
        bulletLineRenderer.enabled = false;

    }

    private IEnumerator ReloadTime()
    {

        audioSource.clip = reloadClip;
        audioSource.Play();
        state = State.Reloading;
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }

    public bool Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            Debug.Log("�߻��");
            lastFireTime = Time.time;
            Shoot();
        }
        return false;
    }

    private void Shoot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if (Physics.Raycast(firePosition.position, firePosition.right * -1, out hit, fireDistance))
        {
            var target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            else
            {
                EffectManager.Instacne.PlayHitEffect(hit.point, hit.normal, hit.transform);
            }

            hitPosition = hit.point;
        }
        else
        {
            //�Ÿ��� ��ġ
            hitPosition = firePosition.position + firePosition.right * -1 * fireDistance;
        }
        StartCoroutine(ShootEffect(hitPosition));
        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    public bool Reload()
    {
        if (state == State.Reloading || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadTime());
        return true;
    }
}
