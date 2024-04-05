using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] GameObject explosionPrefab;

    Collider ownerCollider;
    float speed;
    float explosionSize;
    float range;
    float damage;

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

    private void FixedUpdate() {
        rb.velocity = Vector3.zero;
        rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
    }
    #endregion

    #region Setter Methods
    public void SetSpeed(float pSpeed) {
        speed = pSpeed;
    }

    public void SetExplosionSize(float pSize) {
        explosionSize = pSize;
    }

    public void SetRange(float pRange) {
        range = pRange;
    }

    public void SetOwnerCollider(Collider pCol) {
        ownerCollider = pCol;
        Physics.IgnoreCollision(ownerCollider, GetComponent<Collider>());
        col.enabled = false;
    }

    public void SetDamage(float pDamage) {
        damage = pDamage;
    }
    #endregion

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("destroyed");
        //PlayerData data = collision.gameObject.GetComponent<PlayerData>();

        //if (data != null)
        //    data.TakeDamage(damage);

        //GameObject g = Instantiate(explosionPrefab, transform.position, Quaternion.identity);


        Destroy(gameObject);
    }

}
