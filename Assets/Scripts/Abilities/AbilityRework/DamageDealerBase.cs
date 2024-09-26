using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageDealerBase : MonoBehaviour {

    public System.Action<PlayerData, float> OnDamageDealt;

    protected Team team;

    protected Rigidbody rb;
    protected Collider col;

    #region Unity Events
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        col.enabled = false;
    }

    protected virtual void Start() {

    }
    #endregion

    #region Helper Methods
    public virtual void SetTeam(Team pTeam) {
        team = pTeam;

        if (team != Team.Neutral)
            gameObject.layer = LayerMask.NameToLayer(ZoneReusableData.instance.teamLayerName[team]);
    }
    #endregion
}
