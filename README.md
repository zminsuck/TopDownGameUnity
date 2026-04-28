# 🎮 Top Down Game Unity
> 소울라이크 스타일의 탑다운 액션 RPG 프로젝트 — Unity 6 기반

<br>

## 🛠 Tech Stack
<p align="left">
  <img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=unity&logoColor=black">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white">
  <img src="https://img.shields.io/badge/VS%20Code-007ACC?style=for-the-badge&logo=visual-studio-code&logoColor=white">
  <img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white">
</p>

<br>

## 📖 프로젝트 소개

**Shape of Dreams** 를 모티브로 한 탑다운 뷰 소울라이크 액션 RPG입니다.

- 🔺 **탑다운 시점** — 카메라가 플레이어를 위에서 내려다보는 쿼터뷰 방식
- ⚔️ **소울라이크 전투** — 스태미나 관리, 회피 타이밍, 콤보 시스템
- 🤖 **적 AI 상태머신** — 감지 → 추격 → 공격 → 경직 → 사망 흐름
- 🎯 **마우스 조준** — 이동 방향과 독립적인 마우스 기반 조준

<br>

## ✨ Key Features

### ✅ 구현 완료
- [x] **플레이어 시스템**
  - [x] WASD 이동 (카메라 기준 방향 변환)
  - [x] 스태미나 기반 회피 (Shift)
  - [x] 회피 중 무적 프레임
  - [x] 마우스 조준 (이동과 독립적)
  - [x] 카메라 팔로우 (부드러운 Lerp 보간)
- [x] **전투 시스템**
  - [x] 3단 콤보 공격 (좌클릭)
  - [x] 부채꼴 히트박스 판정
  - [x] 공격 중 이동속도 감소
  - [x] IDamageable 인터페이스
- [x] **적 AI**
  - [x] 5단계 상태머신 (Idle / Chase / Attack / Stagger / Dead)
  - [x] NavMesh 기반 길찾기
  - [x] 플레이어 감지 및 추격
- [x] **생존 시스템**
  - [x] 플레이어 피격 처리
  - [x] 사망 시 씬 리로드
- [x] **스킬 시스템**
  - [x] Q — 광역 슬래시 (주변 360도 범위 공격)
  - [x] E — 돌진 공격 (전방 대시 + 피격)
  - [x] R — 프로젝타일 발사 (원거리 탄환)
  - [x] 스킬별 쿨다운 관리
- [x] **HUD**
  - [x] HP 바 (좌측 하단)
  - [x] 스태미나 바 (좌측 하단)
  - [x] 스킬 슬롯 UI Q / E / R (하단 중앙)
  - [x] 스킬 쿨다운 오버레이

### 🔲 예정 기능
- [ ] 랜덤 방 생성 (로그라이크 던전)
- [ ] 보스 패턴 (페이즈, 특수 공격)
- [ ] 아이템 / 드롭 시스템
- [ ] 빌드 시스템 (에센스 조합)
- [ ] 오디오 / VFX

<br>

## 🗂 프로젝트 구조

```
Assets/
├── _Scenes/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs     # 이동, 회피, 조준, 피격
│   │   └── CameraFollow.cs         # 카메라 팔로우
│   ├── Combat/
│   │   ├── ComboAttack.cs          # 콤보 공격, 히트박스
│   │   ├── Projectile.cs           # 프로젝타일 이동, 충돌
│   │   └── IDamageable.cs          # 데미지 인터페이스
│   ├── Enemy/
│   │   ├── EnemyAI.cs              # 적 AI 상태머신
│   │   └── EnemyState.cs           # 상태 열거형
│   ├── Skills/
│   │   ├── SkillBase.cs            # 스킬 추상 클래스
│   │   ├── SkillSlotManager.cs     # Q/E/R 슬롯 관리
│   │   └── Warrior/
│   │       ├── SkillSlash.cs       # Q — 광역 슬래시
│   │       ├── SkillDash.cs        # E — 돌진 공격
│   │       └── SkillProjectile.cs  # R — 프로젝타일 발사
│   └── UI/
│       └── HUDController.cs        # HP, 스태미나, 스킬 슬롯 UI
├── Prefabs/
├── Art/
└── Audio/
```

<br>

## 🚀 Getting Started

### 요구 사항
- Unity `6000.0.43f1`
- Universal Render Pipeline (URP)
- Input System 패키지
- AI Navigation 패키지

### 실행 방법

```bash
# 저장소 복제
git clone git@github.com:zminsuck/TopDownGameUnity.git

# 폴더 이동
cd TopDownGameUnity
```

1. **Unity Hub** 에서 `Open` → 복제한 폴더 선택
2. Unity 버전 `6000.0.43f1` 로 열기
3. `_Scenes` 폴더에서 메인 씬 열기
4. **Play** 버튼으로 실행

<br>

## 🎮 조작법

| 키 | 동작 |
|---|---|
| `W A S D` | 이동 |
| `Shift` | 회피 (스태미나 소모) |
| `마우스 이동` | 조준 방향 |
| `마우스 좌클릭` | 공격 (최대 3단 콤보) |
| `Q` | 스킬 — 광역 슬래시 |
| `E` | 스킬 — 돌진 공격 |
| `R` | 스킬 — 프로젝타일 발사 |

<br>

## 📝 개발 환경

| 항목 | 내용 |
|---|---|
| Engine | Unity 6000.0.43f1 |
| Render Pipeline | URP (Universal Render Pipeline) |
| Language | C# |
| IDE | Visual Studio Code |
| 버전 관리 | Git / GitHub |

<br>

---

> 🤝 개발 진행 중인 프로젝트입니다.
