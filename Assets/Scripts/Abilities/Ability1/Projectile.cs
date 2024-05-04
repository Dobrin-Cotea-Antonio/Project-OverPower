using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour {
    public Action<GameObject> OnImpact;

    [SerializeField] GameObject explosionPrefab;

    Collider ownerCollider;
    float speed;
    Vector2 explosionSize;
    float range;
    float damage;
    float extraDamageToBurningTargets;
    PlayerData owner;

    Rigidbody rb;
    Collider col;
    Vector3 initialPosition;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        col.enabled = false;
        initialPosition = transform.position;
    }

    #region Updates
    private void Update() {
        if ((initialPosition - transform.position).magnitude > range) {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Setter Methods
    public void SetSpeed(float pSpeed,Vector3 pForward) {
        speed = pSpeed;
        transform.forward = pForward;
        rb.velocity = Vector3.zero;
        rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
        
    }

    public void SetExplosionSize(Vector2 pSize) {
        explosionSize = pSize;
    }

    public void SetRange(float pRange) {
        range = pRange;
    }

    public void SetOwnerCollider(Collider pCol) {
        ownerCollider = pCol;
        Physics.IgnoreCollision(ownerCollider, GetComponent<Collider>());
        col.enabled = true;
        owner = ownerCollider.GetComponent<PlayerData>();
    }

    public void SetDamage(float pDamage) {
        damage = pDamage;
    }

    public void SetExtraDamageFromBurning(float pDamage) {
        extraDamageToBurningTargets = pDamage;
    }
    #endregion

    #region
    void OnProjectileImpact(GameObject pGameObject) {
        PlayerData data = pGameObject.GetComponent<PlayerData>();
        if (data == null)
            return;

        if (data.CheckStackCountOfEffectType<BurnEffect>()<=1)
            return;

        data.TakeDamage(extraDamageToBurningTargets);
    }
    #endregion

    private void OnCollisionEnter(Collision collision) {
        GameObject g = Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
        AoeTimed aoe = g.GetComponent<AoeTimed>();
        aoe.SetDamage(damage);
        aoe.SetDelay(0.05f);
        aoe.SetOwner(owner);
        aoe.SetSize(explosionSize);
        aoe.OnImpact += OnImpact;
        aoe.OnImpact += OnProjectileImpact;

        Destroy(gameObject);
    }

}
