using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    [SerializeField] List<AbilityBase> abilities;

    #region InputEvents
    public void OnAbility1Press() {
        Debug.Log("test1");
    }

    public void OnAbility2Press() {
        Debug.Log("test2");
    }

    public void OnAbility3Press() {
        Debug.Log("test3");
    }
    #endregion

    #region Helper Methods
    AbilityBase ReturnAbilityByIndex(int pIndex) {
        return abilities[pIndex];
    }
    #endregion
}
