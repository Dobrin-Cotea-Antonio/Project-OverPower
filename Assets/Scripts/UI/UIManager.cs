using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class AbilityUIData {
    public Image cooldownIndicator;
    public Image abilityImage;
    public Button abilityButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI chargeText;
}

public class UIManager : MonoBehaviour {

    [SerializeField] private PlayerData playerData;

    [Header("Bottom Bar Background")]
    [SerializeField] private Image background;

    [Header("Hp Bar Data")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI hpRegenText;
    [SerializeField] private RectTransform hpBarTransform;

    [Header("Mana Bar Data")]
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI manaRegenText;
    [SerializeField] private RectTransform manaBarTransform;

    [Header("Abilities")]
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private List<AbilityUIData> abilities;
    [SerializeField] private Gradient abilityOnCooldownGradient;
    [SerializeField] private Gradient abilityInUseGradient;
    [SerializeField] private Color abilityChargeCooldownColor;

    [Header("Capture UI")]
    [SerializeField] private GameObject captureUIHolder;
    [SerializeField] private List<MapArea> mapAreas;
    [SerializeField] private SerializedDictionary<Team, Image> teamCaptureBar;

    [Header("Gold")]
    [SerializeField] private TextMeshProUGUI goldText;

    #region Unity Events
    private void Awake() {
        for (int i = 0; i < abilities.Count; i++)
            abilities[i].abilityImage.sprite = abilityManager.ReturnAbilityIconByIndex(i);
    }

    private void Start() {
        background.color = ZoneReusableData.instance.teamZoneColor[playerData.team];

        UpdateImageSize(teamCaptureBar[Team.Red], new Vector3(0, 1, 1));
        UpdateImageSize(teamCaptureBar[Team.Green], new Vector3(0, 1, 1));
        UpdateImageSize(teamCaptureBar[Team.Blue], new Vector3(0, 1, 1));

        EnableCaptureBars(false);
    }

    private void OnEnable() {
        playerData.OnHpChange += UpdateHpBar;
        playerData.OnHpRegenChange += UpdateHpRegen;
        playerData.OnManaChange += UpdateManaBar;
        playerData.OnManaRegenChange += UpdateManaRegen;

        playerData.OnGoldChange += UpdateGold;

        abilities[0].abilityButton.onClick.AddListener(abilityManager.OnAbility1Press);
        abilities[1].abilityButton.onClick.AddListener(abilityManager.OnAbility2Press);
        abilities[2].abilityButton.onClick.AddListener(abilityManager.OnAbility3Press);

        abilityManager.OnAbilityCooldownDecrease += UpdateAbilityCooldownIcon;
        abilityManager.OnAbilityUseTimeChange += UpdateAbilityUseTime;
        abilityManager.OnAbilityLevelUp += UpdateAbilityLevel;
        abilityManager.OnChargeChange += UpdateAbilityCharges;

        foreach (MapArea area in mapAreas)
            area.OnZoneValueChange += UpdateCaptureBar;
    }

    private void OnDisable() {
        playerData.OnHpChange -= UpdateHpBar;
        playerData.OnHpRegenChange -= UpdateHpRegen;
        playerData.OnManaChange -= UpdateManaBar;
        playerData.OnManaRegenChange -= UpdateManaRegen;

        playerData.OnGoldChange -= UpdateGold;

        abilities[0].abilityButton.onClick.RemoveListener(abilityManager.OnAbility1Press);
        abilities[1].abilityButton.onClick.RemoveListener(abilityManager.OnAbility2Press);
        abilities[2].abilityButton.onClick.RemoveListener(abilityManager.OnAbility3Press);

        abilityManager.OnAbilityCooldownDecrease -= UpdateAbilityCooldownIcon;
        abilityManager.OnAbilityUseTimeChange -= UpdateAbilityUseTime;
        abilityManager.OnAbilityLevelUp -= UpdateAbilityLevel;
        abilityManager.OnChargeChange -= UpdateAbilityCharges;

        foreach (MapArea area in mapAreas)
            area.OnZoneValueChange -= UpdateCaptureBar;
    }
    #endregion

    #region Hp Bar
    private void UpdateHpBar(float pHp, float pMaxHp) {
        hpText.text = string.Format("{0} / {1}", (int)pHp, (int)pMaxHp);
        hpBarTransform.localScale = new Vector3(pHp / pMaxHp, 1, 1);
    }

    private void UpdateHpRegen(float pValue) {
        hpRegenText.text = string.Format("+{0}", pValue.ToString("F1"));
    }
    #endregion

    #region Mana Bar
    private void UpdateManaBar(float pMana, float pMaxMana) {
        manaText.text = string.Format("{0} / {1}", (int)pMana, (int)pMaxMana);
        manaBarTransform.localScale = new Vector3(pMana / pMaxMana, 1, 1);
    }

    private void UpdateManaRegen(float pValue) {
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

        if (pCharges == 0) {
            abilities[pAbilityIndex].cooldownIndicator.color = abilityOnCooldownGradient.Evaluate(1f - pCooldownLeft / pAbilityCooldown);
            abilities[pAbilityIndex].cooldownIndicator.fillAmount = pCooldownLeft / pAbilityCooldown;
            return;
        }

        if (pAbilityChargeCooldownLeft > 0) {
            abilities[pAbilityIndex].cooldownIndicator.color = abilityOnCooldownGradient.Evaluate(1f - pAbilityChargeCooldownLeft / pAbiliyChargeCooldownMax);
            abilities[pAbilityIndex].cooldownIndicator.fillAmount = pAbilityChargeCooldownLeft / pAbiliyChargeCooldownMax;
        } else {
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

    private void UpdateAbilityCharges(int pCharges, int pMaxCharges, int pAbilityIndex) {
        if (pMaxCharges == 1)
            abilities[pAbilityIndex].chargeText.text = "";
        else
            abilities[pAbilityIndex].chargeText.text = (pCharges.ToString());
    }
    #endregion

    #region Capture Bars
    public void EnableCaptureBars(bool pState) {
        captureUIHolder.SetActive(pState);
    }

    void UpdateCaptureBar(List<PlayerData> pData, Team pTeam, float pCaptureValue, float pMaxValue) {
        //Debug.Log("test");

        Vector3 size = new Vector3(Mathf.Clamp(pCaptureValue / pMaxValue, 0, 1), 1, 1);
        UpdateImageSize(teamCaptureBar[pTeam], size);
    }

    private void UpdateImageSize(Image pImage, Vector3 pSize) {
        pImage.rectTransform.localScale = pSize;
    }
    #endregion

    #region Gold
    private void UpdateGold(float pGold) {
        goldText.text = ((int)pGold).ToString();
    }
    #endregion
}
