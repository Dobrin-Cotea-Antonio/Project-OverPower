using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class AbilityUIData {
    public Image cooldownIndicator;
    public Image abilityImage;
    public Button abilityButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI chargeText;
}

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
    [SerializeField] List<AbilityUIData> abilities;
    [SerializeField] Gradient abilityOnCooldownGradient;
    [SerializeField] Gradient abilityInUseGradient;
    [SerializeField] Color abilityChargeCooldownColor;

    private void Awake() {
        for (int i = 0; i < abilities.Count; i++)
            abilities[i].abilityImage.sprite = abilityManager.ReturnAbilityIconByIndex(i);
    }

    #region Events
    void OnEnable() {
        playerData.OnHpChange += UpdateHpBar;
        playerData.OnHpRegenChange += UpdateHpRegen;
        playerData.OnManaChange += UpdateManaBar;
        playerData.OnManaRegenChange += UpdateManaRegen;

        abilities[0].abilityButton.onClick.AddListener(abilityManager.OnAbility1Press);
        abilities[1].abilityButton.onClick.AddListener(abilityManager.OnAbility2Press);
        abilities[2].abilityButton.onClick.AddListener(abilityManager.OnAbility3Press);

        abilityManager.OnAbilityCooldownDecrease += UpdateAbilityCooldownIcon;
        abilityManager.OnAbilityUseTimeChange += UpdateAbilityUseTime;
        abilityManager.OnAbilityLevelUp += UpdateAbilityLevel;
        abilityManager.OnChargeChange += UpdateAbilityCharges;
    }

    void OnDisable() {
        playerData.OnHpChange -= UpdateHpBar;
        playerData.OnHpRegenChange -= UpdateHpRegen;
        playerData.OnManaChange -= UpdateManaBar;
        playerData.OnManaRegenChange -= UpdateManaRegen;

        abilities[0].abilityButton.onClick.RemoveListener(abilityManager.OnAbility1Press);
        abilities[1].abilityButton.onClick.RemoveListener(abilityManager.OnAbility2Press);
        abilities[2].abilityButton.onClick.RemoveListener(abilityManager.OnAbility3Press);

        abilityManager.OnAbilityCooldownDecrease -= UpdateAbilityCooldownIcon;
        abilityManager.OnAbilityUseTimeChange -= UpdateAbilityUseTime;
        abilityManager.OnAbilityLevelUp -= UpdateAbilityLevel;
        abilityManager.OnChargeChange -= UpdateAbilityCharges;
    }
    #endregion

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

    #region Abilities
    private void UpdateAbilityCooldownIcon(float pCooldownLeft, float pAbilityCooldown, float pAbilityChargeCooldownLeft, float pAbiliyChargeCooldownMax, int pCharges, int pMaxCharges, int pAbilityIndex) {

        if (pMaxCharges == 1) {
            abilities[pAbilityIndex].cooldownIndicator.color = abilityOnCooldownGradient.Evaluate(1f - pCooldownLeft / pAbilityCooldown);
            abilities[pAbilityIndex].cooldownIndicator.fillAmount = pCooldownLeft / pAbilityCooldown;
            return;
        }

        if (pCharges == 0) {//|| pAbilityChargeCooldownLeft <= 0
            abilities[pAbilityIndex].cooldownIndicator.color = abilityOnCooldownGradient.Evaluate(1f - pCooldownLeft / pAbilityCooldown);
            abilities[pAbilityIndex].cooldownIndicator.fillAmount = pCooldownLeft / pAbilityCooldown;
            return;
        }

        if (pAbilityChargeCooldownLeft > 0) {
            Debug.Log("test1");
            abilities[pAbilityIndex].cooldownIndicator.color = abilityOnCooldownGradient.Evaluate(1f - pAbilityChargeCooldownLeft / pAbiliyChargeCooldownMax);
            abilities[pAbilityIndex].cooldownIndicator.fillAmount = pAbilityChargeCooldownLeft / pAbiliyChargeCooldownMax;
        } else {
            Debug.Log("test2");
            abilities[pAbilityIndex].cooldownIndicator.color = abilityChargeCooldownColor;
            abilities[pAbilityIndex].cooldownIndicator.fillAmount = pCooldownLeft / pAbilityCooldown;
        }

    }

    private void UpdateAbilityUseTime(float pUseTimeLeft, float pAbilityDuration, int pAbilityIndex) {
        abilities[pAbilityIndex].cooldownIndicator.color = abilityInUseGradient.Evaluate(1f - pUseTimeLeft / pAbilityDuration);
        abilities[pAbilityIndex].cooldownIndicator.fillAmount = pUseTimeLeft / pAbilityDuration;
    }

    private void UpdateAbilityLevel(int pLevel, int pMaxLevel, int pAbilityIndex) {
        abilities[pAbilityIndex].levelText.text = (pLevel + 1).ToString();
    }

    private void UpdateAbilityCharges(int pCharges,int pMaxCharges,int pAbilityIndex) {
        if (pMaxCharges==1)
            abilities[pAbilityIndex].chargeText.text = "";
        else
            abilities[pAbilityIndex].chargeText.text = (pCharges.ToString());
    }
    #endregion
}
