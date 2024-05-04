using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyUI : MonoBehaviour {
    [SerializeField] PlayerData data;

    [Header("Hp Bar Data")]
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI hpRegenText;
    [SerializeField] RectTransform hpBarTransform;

    [Header("Mana Bar Data")]
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI manaRegenText;
    [SerializeField] RectTransform manaBarTransform;

    private void Awake() {
        data.OnHpChange += UpdateHpBar;
        data.OnHpRegenChange += UpdateHpRegen;
        data.OnManaChange += UpdateManaBar;
        data.OnManaRegenChange += UpdateManaRegen;
    }

    #region Hp Bar
    void UpdateHpBar(float pHp, float pMaxHp) {
        hpText.text = string.Format("{0} / {1}", (int)pHp, (int)pMaxHp);
        hpBarTransform.localScale = new Vector3(pHp / pMaxHp, 1, 1);
    }

    void UpdateHpRegen(float pValue) {
        hpRegenText.text = string.Format("+{0}", pValue.ToString("F1"));
    }
    #endregion

    #region Mana Bar
    void UpdateManaBar(float pMana, float pMaxMana) {
        manaText.text = string.Format("{0} / {1}", (int)pMana, (int)pMaxMana);
        manaBarTransform.localScale = new Vector3(pMana / pMaxMana, 1, 1);
    }

    void UpdateManaRegen(float pValue) {
        manaRegenText.text = string.Format("+{0}", pValue.ToString("F1"));
    }
    #endregion
}
