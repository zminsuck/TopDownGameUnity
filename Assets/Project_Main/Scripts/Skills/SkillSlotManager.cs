using UnityEngine;
using UnityEngine.InputSystem;

public class SkillSlotManager : MonoBehaviour
{
    [Header("스킬 슬롯")]
    private SkillBase skillQ;  // Inspector에서 드래그
    private SkillBase skillE;
    private SkillBase skillR;

    private InputAction _qAction;
    private InputAction _eAction;
    private InputAction _rAction;

    private void Awake()
    {
    // Inspector 드래그 대신 자동으로 컴포넌트 찾기
    skillQ = GetComponent<SkillSlash>();
    skillE = GetComponent<SkillDash>();
    skillR = GetComponent<SkillProjectile>();

    _qAction = new InputAction("SkillQ", InputActionType.Button);
    _qAction.AddBinding("<Keyboard>/q");

    _eAction = new InputAction("SkillE", InputActionType.Button);
    _eAction.AddBinding("<Keyboard>/e");

    _rAction = new InputAction("SkillR", InputActionType.Button);
    _rAction.AddBinding("<Keyboard>/r");

    _qAction.performed += _ => skillQ?.TryUse();
    _eAction.performed += _ => skillE?.TryUse();
    _rAction.performed += _ => skillR?.TryUse();
    }
    private void OnEnable()
    {
        _qAction.Enable();
        _eAction.Enable();
        _rAction.Enable();
    }

    private void OnDisable()
    {
        _qAction.Disable();
        _eAction.Disable();
        _rAction.Disable();
    }
}