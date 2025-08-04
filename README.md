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
기능 1 : 데이터 관리
<details><summary>접기/펼치기</summary>
</details>

기능 2 : UI 관리
<details><summary>접기/펼치기</summary>
<img width="875" height="734" alt="image" src="https://github.com/user-attachments/assets/1e49e4cc-bf6e-4342-82f7-f63af96f8a02" />

IOnButton, IUiShow, IUiUpdate 인터페이스를 사용하여 기능관련 메소드를 관리했습니다. <br>
UIManager에서 필드값을 관리하였고 인터페이스를 상속받아서 기능관련 메소드를 구현했습니다.. <br>


    
</details>

기능 3 : 맵 관리
<details><summary>접기/펼치기</summary>
    
</details>

기능 4 : 캐릭터 관리
<details><summary>접기/펼치기</summary>
    
</details>

기능 5 : 아이템 관리
<details><summary>접기/펼치기</summary>
    
</details>


## ⏲️ 개발기간
- 2025.07.29(화) ~ 2024.08.01(월)

## 📚️ 기술스택

### ✔️ Language
C#, Unity, git

### ✔️ Version Control
Unity : 2022.3.17f1, git DeskTop

### ✔️ Deploy


### ✔️  DBMS
Firebase (Cloud Firestore)

## 서비스 구조



## 와이어프레임



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
    
</details>

2. 김우민
<details><summary>접기/펼치기</summary>
    
</details>

3. 권진석
<details><summary>접기/펼치기</summary>
    
</details>

4. 박진우
<details><summary>접기/펼치기</summary>

사례1 : 해상도 변경시 UI 이미지, 위치 변경 문제

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

5. 신명철
<details><summary>접기/펼치기</summary>
    
</details>

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
   
