using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] PlayerData playerData;

    [Header("Hp Bar Data")]
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI hpRegenText;
    [SerializeField] RectTransform hpBarTransform;

    [Header("Mana Bar Data")]
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI manaRegenText;
    [SerializeField] RectTransform manaBarTransform;

    [Header("Abilities")]
    [SerializeField] AbilityManager abilityManager;
    [SerializeField] List<Image> cooldownIndicators;
    [SerializeField] List<Button> abilityButtons;

    private void Awake() {
        playerData.OnHpChange += UpdateHpBar;
        playerData.OnHpRegenChange += UpdateHpRegen;
        playerData.OnManaChange += UpdateManaBar;
        playerData.OnManaRegenChange += UpdateManaRegen;

        abilityButtons[0].onClick.AddListener(abilityManager.OnAbility1Press);
        abilityButtons[1].onClick.AddListener(abilityManager.OnAbility2Press);
        abilityButtons[2].onClick.AddListener(abilityManager.OnAbility3Press);
    }

    #region Hp Bar
    void UpdateHpBar(float pHp,float pMaxHp) {
        hpText.text = string.Format("{0} / {1}",(int)pHp,(int)pMaxHp);
        hpBarTransform.localScale = new Vector3(pHp/pMaxHp, 1, 1);
    }

    void UpdateHpRegen(float pValue) {
        hpRegenText.text = string.Format("+{0}",pValue.ToString("F1"));
    }
    #endregion

    #region Mana Bar
    void UpdateManaBar(float pMana,float pMaxMana) {
        manaText.text = string.Format("{0} / {1}", (int)pMana, (int)pMaxMana);
        manaBarTransform.localScale = new Vector3(pMana / pMaxMana, 1, 1);
    }

    void UpdateManaRegen(float pValue) {
        manaRegenText.text = string.Format("+{0}", pValue.ToString("F1"));
    }
    #endregion

}
