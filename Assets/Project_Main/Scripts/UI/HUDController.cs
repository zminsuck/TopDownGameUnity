using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("바 연결")]
    public Slider hpBar;
    public Slider staminaBar;

    [Header("스킬 슬롯")]
    public Image skillQCooldown;
    public Image skillECooldown;
    public Image skillRCooldown;

    private PlayerController _player;
    private SkillBase _skillQ;
    private SkillBase _skillE;
    private SkillBase _skillR;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) return;

        _player = playerObj.GetComponent<PlayerController>();
        _skillQ = playerObj.GetComponent<SkillSlash>();
        _skillE = playerObj.GetComponent<SkillDash>();
        _skillR = playerObj.GetComponent<SkillProjectile>();
    }

    private void Update()
    {
        if (_player == null) return;

        // HP / 스태미나 바 업데이트
        hpBar.value     = _player.HpRatio;
        staminaBar.value = _player.StaminaRatio;

        // 스킬 쿨다운 업데이트
        UpdateCooldown(skillQCooldown, _skillQ);
        UpdateCooldown(skillECooldown, _skillE);
        UpdateCooldown(skillRCooldown, _skillR);
    }

    private void UpdateCooldown(Image overlay, SkillBase skill)
    {
        if (overlay == null || skill == null) return;
        overlay.fillAmount = skill.CooldownRatio;
    }
}