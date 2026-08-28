# 게임 그래픽 프로그래밍 포트폴리오

## 1. 그래픽 콘셉트

본 프로젝트는 Unity 6와 URP를 기반으로 제작한 **SF 스타일의 그래픽 쇼케이스 씬**이다.

간단한 3D 오브젝트와 에너지 이펙트를 배치하고, 발광 머티리얼과 파티클 효과를 통해 게임에서 사용되는 실시간 그래픽 표현을 확인할 수 있도록 구성하였다.

차가운 금속 재질의 오브젝트와 청록색 계열의 발광 효과를 대비시키고, Shader Graph의 Fresnel과 Emission, Particle System, Visual Effect Graph 및 Bloom을 활용하여 미래적인 에너지 효과를 표현하였다.

---

## 2. 3D 요소와 이펙트 공간 배치 분석

### 2-1. 씬 구성

| 요소 | 용도 |
|---|---|
| Plane | 이펙트와 3D 오브젝트를 배치하는 바닥 |
| Sphere | PBR 머티리얼 비교 |
| Cube | Shader Graph 발광 머티리얼 적용 |
| HitEffect | 입력에 반응하는 순간적인 Particle System |
| GroundEffect | 바닥에 발생하는 Particle System |
| VFX_Glow | Visual Effect Graph 기반 발광 효과 |
| Global Volume | Bloom 후처리 |
| Main Camera | 최종 화면 출력 |

### 2-2. HitEffect 배치 분석

과제의 대표 이펙트로 `HitEffect`를 선정하였다.

| 항목 | 설정 |
|---|---|
| 이펙트 | HitEffect |
| 발생 조건 | Space 키 입력 |
| 발생 위치 | EffectSpawner 위치 |
| Position | `(-0.22459, 0, 0.94083)` |
| 방향 | 기본 회전 기준 |
| Rotation | `(-90, 0, 0)` |
| Scale | `(1, 1, 1)` |
| Particle Start Size | `0.3` |
| Duration | `1초` |
| Start Lifetime | `0.5초` |
| 카메라 거리 | Main Camera에서 확인 가능한 중앙 영역 |
| 플레이 규칙 | Space 입력 시 순간적인 이펙트 발생 |

`HitEffect`는 짧은 시간 동안 발생하고 사라지는 효과로 구성하여 플레이어의 입력이나 공격·피격과 같은 순간적인 게임 사건을 시각적으로 전달하는 용도로 설계하였다.

Space 키를 누르면 `EffectSpawner`가 이펙트를 생성하며, 키를 계속 누르고 있어도 같은 프레임에서 반복 생성되지 않도록 구성하였다.

### 2-3. GroundEffect 배치 분석

| 항목 | 설정 |
|---|---|
| 이펙트 | GroundEffect |
| 발생 위치 | 씬의 바닥 영역 |
| Position | `(-1.57158, 0, 0.87769)` |
| Rotation | `(-90, 0, 0)` |
| Scale | `(1, 1, 1)` |
| Duration | `1초` |
| Start Lifetime | `0.8초` |
| Start Size | `0.2` |

GroundEffect는 바닥 영역에서 발생하는 효과를 표현하여 지면 기반의 그래픽 이펙트를 확인할 수 있도록 구성하였다.

---

## 3. Shader Graph 머티리얼

### Shader Graph

사용 Shader Graph:

`SG_Glow`

### 핵심 노드 연결

```text
Fresnel Effect
      ↓
   Multiply ← Color
      ↓
   Emission

   Fresnel Effect를 이용하여 카메라와 표면의 각도에 따라 외곽 부분이 강조되도록 구성하였다.

Fresnel 결과에 Color 값을 Multiply하여 발광 색상을 조절하고, 최종 결과를 Emission에 연결하였다.

사용 기능
기능	사용
Emission	O
Fresnel	O
Noise	X
UV Animation	X
Alpha	X

과제에서 요구하는 Emission, Fresnel, Noise, UV Animation, Alpha 중 Emission과 Fresnel을 사용하였다.

4. PBR 머티리얼 비교
사용 머티리얼

PBR_Metal

Sphere 오브젝트에 PBR 머티리얼을 적용하여 금속성 및 표면의 매끄러움에 따른 표현을 확인하였다.

주요 설정
항목	값
Workflow Mode	Metallic
Metallic	1.0
Smoothness	0.8
Surface Type	Opaque
Receive Shadows	On

Metallic 값을 1.0으로 설정하여 금속성 표면을 표현하고 Smoothness를 0.8로 설정하여 매끄럽고 강한 반사 특성을 표현하였다.

과제에서 요구하는 Metallic, Smoothness, Normal, Emission 중 Metallic과 Smoothness 2개 항목을 조절하였다.

5. Particle System 이펙트

본 프로젝트에는 Built-in Particle System 기반 이펙트 2개를 제작하였다.

5-1. HitEffect

Prefab:

HitEffect

항목	값
Duration	1
Looping	Off
Start Lifetime	0.5
Start Speed	2
Start Size	0.3
Start Rotation	0
Simulation Space	Local
Max Particles	1000
Play On Awake	On
Emission	On
Shape	On
5-2. GroundEffect

Prefab:

GroundEffect

항목	값
Duration	1
Looping	Off
Start Lifetime	0.8
Start Speed	1
Start Size	0.2
Start Rotation	0
Simulation Space	Local
Max Particles	1000
Play On Awake	On
Emission	On
Shape	On

두 Particle System을 각각 Prefab으로 구성하였다.

6. Visual Effect Graph 이펙트
VFX Graph

사용 에셋:

VFX_Glow

기본 흐름
Spawn
  ↓
Initialize
  ↓
Update
  ↓
Output

VFX Graph의 기본적인 파티클 처리 흐름을 구성하여 파티클 생성부터 초기화, 갱신, 화면 출력까지의 과정을 확인하였다.

Spawn

SpawnRate를 Exposed Property로 설정하였다.

SpawnRate = 30

Inspector에서 SpawnRate 값을 직접 변경할 수 있도록 구성하였다.

Initialize

주요 설정:

항목	값
Capacity	128
Bounds Center Y	-2
Bounds Size	7 / 8 / 7
Lifetime	0.5
Position Shape	Sphere
Spawn Mode	Random
Speed Mode	Random
Min Speed	0.5
Max Speed	4.5
Update
항목	값
Gravity	사용
Gravity Force Y	-9.81
Linear Drag	사용
Drag Coefficient	0.5
Output
항목	설정
Blend Mode	Alpha
Orient	Face Camera Plane
Multiply Size Over Life	사용
Multiply Color Over Life	사용
7. 코드 연동
EffectSpawner.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class EffectSpawner : MonoBehaviour
{
    public GameObject effectPrefab;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }
}
동작 과정
Space 입력
    ↓
wasPressedThisFrame 확인
    ↓
EffectSpawner 위치 확인
    ↓
HitEffect Prefab 생성

wasPressedThisFrame을 사용하여 Space 키를 누른 순간에만 이펙트가 생성되도록 구성하였다.

Space 키를 한 번 누르면 HitEffect가 한 번 생성되며, Space 키를 계속 누르고 있어도 매 프레임마다 이펙트가 반복 생성되지 않는 것을 확인하였다.

따라서 하나의 입력 사건에서 불필요하게 이펙트가 중복 생성되는 현상을 방지하였다.

8. 구현 설정 기록
8-1. 프로젝트 및 URP 환경
항목	설정
Unity	6000.5.9f1
Render Pipeline	URP
Project Template	Universal 3D
Color Space	Linear
Active Input Handling	Input System
Main Camera Post Processing	On
Global Volume	사용
Bloom	사용
8-2. Shader Graph 핵심 연결
Fresnel Effect
      ↓
   Multiply ← Color
      ↓
   Emission

Shader Graph에서 Fresnel과 Emission을 사용하여 발광 효과를 구현하였다.

8-3. PBR Material
Material	Metallic	Smoothness
PBR_Metal	1.0	0.8
8-4. Particle System
이펙트	Duration	Lifetime	Speed	Size	Loop
HitEffect	1	0.5	2	0.3	Off
GroundEffect	1	0.8	1	0.2	Off
8-5. VFX Graph
Spawn
  ↓
Initialize
  ↓
Update
  ↓
Output

SpawnRate를 Exposed Property로 설정하여 Inspector에서 생성량을 조절할 수 있도록 구성하였다.

현재 값:

SpawnRate = 30
8-6. Bloom
항목	값
Threshold	0.8
Intensity	1
Scatter	0.7

Bloom을 적용하여 Emission으로 생성된 밝은 영역이 주변으로 퍼지는 발광 효과를 강화하였다.

8-7. 성능 조절 가능 값

이펙트의 성능과 화면 표현을 조절할 수 있도록 다음과 같은 값을 관리하였다.

항목	조절 목적
SpawnRate	VFX Graph 파티클 생성량 조절
Max Particles	Particle System 최대 파티클 수 제한
Start Lifetime	파티클 유지 시간 조절
Start Size	파티클 크기 조절
Start Speed	파티클 이동 속도 조절
Bloom Intensity	화면 발광 강도 조절
9. 실행 검증 기록
확인 항목	결과
Unity 6 URP 프로젝트 실행	PASS
Shader Graph 머티리얼 적용	PASS
Fresnel + Emission 확인	PASS
PBR Material 적용	PASS
Metallic / Smoothness 확인	PASS
HitEffect 출력	PASS
GroundEffect 출력	PASS
Particle Prefab 확인	PASS
VFX Graph 출력	PASS
SpawnRate Inspector 노출	PASS
Space 입력 연동	PASS
Space 입력 시 HitEffect 생성	PASS
Space 키를 계속 눌렀을 때 중복 생성 방지	PASS
Bloom 발광 효과 확인	PASS
10. 최종 체크리스트
확인 항목	완료
Unity 6 URP 프로젝트에서 실행된다.	O
이펙트의 위치, 방향, 크기, 카메라 거리, 지속 시간을 분석했다.	O
이펙트가 전달할 플레이 규칙과 발생 조건을 설명했다.	O
Shader Graph 머티리얼이 씬 오브젝트에 적용되어 있다.	O
Shader Graph 핵심 흐름을 설명할 수 있다.	O
Particle System 이펙트가 2개 이상 있다.	O
이펙트 Prefab이 코드에서 재생된다.	O
Visual Effect Graph 이펙트가 씬에서 재생된다.	O
VFX Graph의 주요 프로퍼티를 조절할 수 있다.	O
URP·Material·Particle System·VFX Graph의 핵심 설정과 실행 결과를 기록했다.	O
성능을 위해 조절 가능한 값을 정리했다.	O
같은 사건에서 이펙트가 중복 재생되지 않는 것을 확인했다.	O
11. 주요 산출물
Unity Project
├─ 구현 씬
├─ Shader Graph
├─ Particle System Prefab
│  ├─ HitEffect
│  └─ GroundEffect
├─ Visual Effect Graph
│  └─ VFX_Glow
└─ C# Script
   └─ EffectSpawner.cs
12. 최종 결과

본 프로젝트에서는 Unity 6와 URP 환경에서 Shader Graph, PBR Material, Particle System, Visual Effect Graph를 활용하여 하나의 그래픽 쇼케이스 씬을 제작하였다.

Shader Graph에서는 Fresnel과 Emission을 활용하여 발광 머티리얼을 제작하고, PBR Material에서는 Metallic과 Smoothness를 조절하여 금속성 표면을 표현하였다.

Particle System은 HitEffect와 GroundEffect 두 종류를 제작하여 Prefab으로 구성하였으며, C# 스크립트를 통해 Space 입력 시 HitEffect가 생성되도록 연결하였다.

VFX Graph에서는 Spawn → Initialize → Update → Output의 기본 흐름을 구성하고 SpawnRate를 Exposed Property로 설정하여 Inspector에서 생성량을 조절할 수 있도록 구현하였다.

최종적으로 Play Mode에서 Shader, Particle System, VFX Graph 및 Bloom 효과가 정상적으로 출력되는 것을 확인하였으며, Space 입력에 따른 이펙트 생성과 중복 생성 방지까지 검증하였다.
