# RunningGame

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [팀소개](#팀소개)
3. [프로젝트 계기](#프로젝트-계기)
4. [주요기능](#주요기능)
5. [개발기간](#개발기간)
6. [기술스택](#기술스택)
7. [서비스 구조](#서비스-구조)
8. [와이어프레임](#와이어프레임)
9. [API 명세서](#API-명세서)
10. [ERD](#ERD)
11. [프로젝트 파일 구조](#프로젝트-파일-구조)
12. [Trouble Shooting](#trouble-shooting)
13. [WorkFlow](#워크플로우)
    
## 👨‍🏫 프로젝트 소개
TeamSparta의 Unity과정 중 Unity 2D 실습 팀 프로젝트 입니다.

## 팀소개
팀장 : 박종현<br>
팀원 : 김우민, 권진석, 박진우, 신명철

## 프로젝트 계기
프로젝트 레퍼런스 중에 선택할 수 있던 것이 궁수의전설, 쿠키런, Fire&Water 세가지였습니다.<br>
그 중 쿠키런이 모두가 알고 있었고, 로직을 구현하는데 큰 문제가 없을것이라 판단되어 프로젝트를 시작하게 되었습니다.<br>
## 💜 주요기능
기능 1 : 데이터 기능 - 박종현
<details><summary>접기/펼치기</summary>
<img width="299" height="167" alt="image" src="https://github.com/user-attachments/assets/74624209-b413-4a16-a5fe-0df0c014aafa" />

<img width="333" height="93" alt="image" src="https://github.com/user-attachments/assets/0ade290c-b032-4179-a337-08388598262c" />

<img width="194" height="130" alt="image" src="https://github.com/user-attachments/assets/bb6177f7-c059-454b-b387-ce128f74200f" />


- 사용자가 계정을 생성하면 Firebase Authentication에 신규 계정이 등록되고, 동시에 해당 계정의 초기 데이터를 Firebase Realtime Database에 저장합니다.

- 로그인 시(이메일과 비밀번호를 입력하면), 해당 이메일에 대응하는 UID(고유 식별자)를 받아옵니다.

- 받아온 UID를 기반으로 Realtime Database에서 해당 계정의 데이터를 조회한 뒤, 해당 데이터를 유니티에서 json 파일 형태로 받아와 활용합니다.


    
</details>

<hr>

기능 2 : UI 관리 - 박진우
<details><summary>접기/펼치기</summary>
<img width="875" height="734" alt="image" src="https://github.com/user-attachments/assets/1e49e4cc-bf6e-4342-82f7-f63af96f8a02" />

- IOnButton, IUiShow, IUiUpdate 인터페이스를 사용하여 기능관련 메소드를 관리했습니다. <br>
- UIManager에서 필드값을 관리하였고 인터페이스를 상속받아서 기능관련 메소드를 구현했습니다.. <br>


    
</details>
<hr>
기능 3 : 맵 관리
<details><summary>접기/펼치기</summary>

<img width="1159" height="238" alt="image" src="https://github.com/user-attachments/assets/3b110920-8779-4a8d-96a0-aa2be7482a8b" />

- 메인 메뉴에서 스테이지 선택시 스테이지에 맞는 프래팹 및 스테이지 맵 로드
- 다음맵 지나갈시 이전맵 확인후 맵 재활용
- 장애물 배치 및 애니메이션 생성
- 현재 맵 진행도에 따른 HP 회복스테이지 및 프로그레스바 적용
    
</details>

<hr>
기능 4 : 캐릭터, 카메라
<details><summary>접기/펼치기</summary>

캐릭터 기능
- 플레이어 더블점프 및 슬라이딩
- 특정 아이템을 흭득하면 특정 능력이 몇초간 발현 및 유지
- 플레이어가 시간이 지날수록 hp감소

카메라 기능
- 처음시작시 카메라는 가만히 있다가 플레이어가 확인되면 플레이어를 추적

</details>
<hr>
기능 5 : 아이템 - 권진석
<details><summary>접기/펼치기</summary>
아이템 스크립트
- 기본적인 아이템 타입값과 Unity 에서 사용할 수 있는 온트리거 명령어를 넣어서 발동하고 사라질 수 있게 만들었습니다.  그다음은 불값 케이스 설정으로 아이템마다 정해진 양을 설정하고 상황에 맞게 아이템을 발동시킬 수 있게 설정했습니다.

</details>
<hr>

## ⏲️ 개발기간
- 2025.07.29(화) ~ 2024.08.01(월)

## 📚️ 기술스택

### ✔️ Language
C#, Unity, git

### ✔️ Version Control
Unity : 2022.3.17f1, git DeskTop


### ✔️  DBMS
Firebase (Cloud Firestore)


## 와이어프레임
<img width="446" height="268" alt="image" src="https://github.com/user-attachments/assets/d00e4bcd-bb1b-4101-82e3-ccf96c7bf3c0" />


## API 명세서
| 기능 | 메서드 | 설명 |
| 데이터 저장 | `public async Task SaveData(string uid)` | `users/uid` 경로에 `currentPlayerdata`를 JSON으로 저장 | <br>
| 데이터 불러오기 | `public async Task<PlayerData> LoadData(string uid)` | `GetValueAsync()`로 Firebase에서 데이터 로드 후 `FromJson` 파싱 |<br>
| 이름 설정 | `public void SetName(string name)` | `userName`과 `isSetName` 값을 설정 (최초 1회) |<br>
| 캐릭터 추가 | `public void SetCharacter(string characterName)` | 문자열을 `CharacterType`으로 변환 후 보유 캐릭터 목록에 추가 |<br>
| 캐릭터 장착 | `public void SetCurrentCharacter(CharacterType characterType)` | 현재 장착 캐릭터를 설정 |<br>
| 던전 진입 설정 | `public void GoingDungeon(int level)` | 현재 진입할 던전 번호를 설정 |<br>
| 로그인 상태 UID 확인 후 저장 | `public async void OnSaveData()` | `FirebaseAuthManager.Instance.GetUserUID()`로 UID 조회 후 저장 트리거 |<br>


## ERD
```

users (노드)
└ userId (key)
├ userName: string
├ gold: int
├ bastScores: List<int>
├ isSetName: bool
├ characters: List<string> (CharacterType enum)
└ currentCharacter: string (CharacterType)
```


## 프로젝트 파일 구조
```

Asset<br>
├ Animator
│ ├ Opstacle
│ └ Player
├ Image<br>
│ ├ Dark UI
│ ├ FishFight
│ ├ Free 2D Cartoon Parallax Background
│ ├ Pixel Cursors
│ ├ PlayerJump
│ ├ Stage
│ └ TileMaps
├ Model
├ Prefeb
│ ├ Item
│ ├ Obstacle
│ ├ Player
│ └ RankingUI
├ Scenes
│ ├ Work
│ ├ 1.MainScene
│ ├ 2.InGameScene
│ └ AnimScene
└ Scripts
  ├ Auth<br>
  ├ Character
  ├ Interface
  ├ Item
  ├ Manager
  └ Obstacle
```


## Trouble Shooting

1. 박종현
<details><summary>접기/펼치기</summary>

1. 배경
- 플레이어가 특정 구역에 도달하면 속도가 빨라지도록 구현.

2. 발단
- 속도가 빨라지면 맵의 구조(발판 거리, 높이 등)가 기존 속도 기준이라 점프 거리가 길어져서 게임 밸런스가 깨짐.

3. 전개
- 목표: 속도가 바뀌어도 점프 거리, 높이, 체공 시간이 항상 일정해야 함.
- 해결 방법:
-> 속도 변화에 따라 점프 힘과 중력 값을 아래 공식으로 자동 조절.
-> 점프 힘 = 기준 점프 힘 × (현재 속도 ÷ 기준 속도)
-> 중력 값 = 기준 중력 값 × (현재 속도 ÷ 기준 속도)²

4. 위기
- 속도가 크게 증가하니 중력 값도 제곱에 따라 커짐.
- 중력이 너무 세지면 플레이어가 바닥 콜라이더를 뚫고 지나가는 터널링 현상(충돌 감지 실패) 발생.
<img width="192" height="160" alt="image" src="https://github.com/user-attachments/assets/dbd1125f-f0ca-4e6f-bb0a-6bb380cc41cb" />

5. 절정 (해결)
- Rigidbody2D의 Collision Detection을 Continuous로 변경
- Discrete: 기본값, 느린 물체에 적합(터널링 위험 있음)
- Continuous: 빠른 물체가 정적 콜라이더와 충돌할 때 터널링 방지

6. 결론 
- 속도가 빠른 게임에서 점프/중력 값을 공식으로 제어하는 것만으로는 충분하지 않음.
- 돌 감지 모드(Collision Detection)도 적절하게 설정해야 터널링 등 물리 버그를 예방할 수 있음
    
</details>
<hr>

2. 김우민
<details><summary>접기/펼치기</summary>
    
플레이어
- 플레이어 코드에 스킬발현이 같이 들어가있어 단일체계원칙에 어긋났었다
- 플레이어 코드에는 점프, 슬라이딩, 사망만 들어가있어야하고 Hp는 플레이어스탯에 스킬발현은 플레이어스킬 코드로 나눴으 유지보수가 용이했을것이다. 
- 플레이어가 슬라이딩중에 점프가 가능했고, 점프중에 슬라이딩이 작동됐다 
- 각각의 함수에서 조건문을 걸어 해당 이슈를 해결했습니다

카메라
- 카메라가 플레이어를 따라가면서 플레이어의 도트가 깨지는 문제가 발생
- Vector3.Lerp를 사용해 카메라가 자연스럽게 이동하게 만들었음


플레이어 스킬
- 플레이어와 젤리의 거리를 재서 자석이 활성화 되어있을때 자석범위안에 들어오면 플레이어한테 끌려오는 코드를 작성했는데 막상 디버그해보니 다 정상작동되는데 젤리가 플레이어에게 끌려오지않는 이상현상이 발생됐다
- 젤리에 리지드바디를 생성하고 바디타입을 다이나믹으로 변경했더니 정상작동됐다
  
</details>
<hr>

3. 권진석
<details><summary>접기/펼치기</summary>
    
</details>
<hr>

4. 박진우
<details><summary>접기/펼치기</summary>

사례1 : 해상도 변경시 UI 이미지, 위치 변경 문제
<img width="1132" height="669" alt="image" src="https://github.com/user-attachments/assets/a6f61953-3b3a-465e-a1f7-a7f599eb2428" />


배경 - 기준으로 삼았던 해상도에서 다른 해상도로 변경이 가능한지 테스트 하던 중 해상도 변경시 이미지의 크기와 위치가 유지되지 않은것을 발견하였습니다. 

발단 - 해결방법을 찾기위해 해상도 관련 자료를 찾아보았습니다. 

전개 - 이 문제를 해결하기 위해 아래의 방법을 조치했습니다.

-> Anchor의 위치값을 카메라를 기준으로 변경

위기 - 카메라를 기준으로 변경하여도 이미지의 크기는 그대로여서 관련 자료를 찾아보았습니다.

절정 - Canvas Scaler 컴포넌트 관련 자료 중 UI Scail Mode를 조정하여 해상도 변경시 Canvas에 보정을 줄 수 있다는 것을 확인하였습니다.

-> Scale With Screen Size를 설정하여 해상도가 변경되어도 UI의 Scale값에 보정을 줄 수 있도록 수정하였습니다.

결말 - 해상도 변경시 보정값을 주어 UI가 유지될 수 있도록 수정할 수 있었습니다.

https://zjaxjrhkd.tistory.com/210
    
</details>
<hr>

5. 신명철
<details><summary>접기/펼치기</summary>
<img width="269" height="112" alt="image" src="https://github.com/user-attachments/assets/c4b1d993-a3e1-4b70-9197-57ef427b3c81" />


맵 반복에 대한 문제
- 백그라운드는 고정적으로 움직이는 반면, 스테이지는 랜덤값을 주어 다음으로 이동해야됨으로 해당 문제를 해결하는데는 상관이 없었지만, 최대 스테이지에 따른 고정 맵 추가를 넣음으로 문제가 발생하였다

- 해당 문제를 해결하기위해 전체 맵 수를 확인하고 현재 부딪힌 BackGround가 HP를 회복시키는 맵이면 해당 맵을 제외하여 맵 카운트를 올렸고MapCheck()에서 마지막 맵의 bool값을 true로 변경하여스테이지를 불러오는 부분에서 bool값이 true일경우 회복맵을 추가했다

- 이 작업을 통해 맵을 재활용하여 루프로 할 경우 추가 맵을 고정적으로 생성 시킬 일이 있으면 여러 조건을 확인해서 넣어줘야 함으로 손이 많이 가게 된다는 걸 알게 되었다
</details>
<hr>

## WorkFlow
<details><summary>접기/펼치기</summary>
    
워크플로우
1. 이슈 작성하기
<img width="2848" height="1192" alt="image" src="https://github.com/user-attachments/assets/80763439-c5ad-4e94-900d-08f25433bfa7" />
<img width="798" height="570" alt="image" src="https://github.com/user-attachments/assets/f3ec2d77-55d6-4387-b901-f9e4021e829e" />



2. 프로젝트 입력하기
<img width="1626" height="726" alt="image" src="https://github.com/user-attachments/assets/f00d21df-d7b0-4a49-b3c6-db32f6036b7f" />
- Add Item
<img width="596" height="414" alt="image" src="https://github.com/user-attachments/assets/fbe45e8c-2404-4a1a-a03d-0156fba3988a" />

<img width="685" height="273" alt="image" src="https://github.com/user-attachments/assets/0a3c640c-9960-4ac3-b1bb-2d87ed1ea609" />
<img width="1346" height="453" alt="image" src="https://github.com/user-attachments/assets/20555ccd-e817-4fa3-8fb3-267f29f3579d" />

StartDate DeadLine입력하기

3. 이슈 번호 확인하기
<img width="541" height="86" alt="image" src="https://github.com/user-attachments/assets/cca97813-0f63-437c-a673-a79fd2b13be6" />

4. Branch 생성
<img width="402" height="385" alt="image" src="https://github.com/user-attachments/assets/d90d2a53-814e-4842-8c62-5ed6cdf2d482" />

- 종류가 feat 이슈번호가 40이었으면 feat#40으로 생성

5. 작업 종료 후 Commit
<img width="1221" height="687" alt="image" src="https://github.com/user-attachments/assets/e3537fcf-9096-45e8-883c-a44452c6ef19" />
<img width="697" height="136" alt="image" src="https://github.com/user-attachments/assets/4587f39d-db6a-49e1-ae4b-862b7b0d26bb" />

- 웹에서 내용 복사 후 수정사항이 있으면 수정하여 Commit내용 작성
   
6. 승인 단계를 건너뛰기 때문에 로컬에서 직접 Dev로 Merge 후 웹으로 Push

7. 포로젝트에서 state를 Done으로 수정, End Date 입력

8. 매일 7시 30분 코드리뷰
</details>
