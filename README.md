# 🕹️ FightStars - Brawl 스타일 실시간 메치 게임

## 📌 게임 개요

**FightStars**는 Brawl Stars에서 영감을 받아 제작된 Unity 기반의 **메어시트 실시간 액션 게임**입니다.  
플레이어는 다양한 **브롤러 캐릭터와 스킨**을 수집하고, 여러가지 전장에서 다른 유저들과 실시간으로 경쟁하게 됩니다.

---

## 🔐 인증 처리 방식

### ✅ 로그인 방식

- 기본 **JWT 기반 로그인** 방식 사용
- 첫 번째 로그인 시:
  - 유저 계정 인증 → 서버에서 AccessToken / RefreshToken 발급
  - Unity 클라이언트에서는 `PlayerPrefs`에 JWT 저장
- 이후 API 요청 시 JWT를 `Authorization` 헤더에 포함해서 인증 처리

```http
Authorization: Bearer <accessToken>
```

---

## 🔗 API 처리 방식

### ✅ API 유틸 구조

- 클라이언트에서는 `ApiClient.cs`를 통해 REST API 호출
- API 요청은 공통적으로 아래 순서로 처리됩니다:

1. URL 구성 + JSON 질리트
2. `UnityWebRequest`로 요청 전송
3. 응답 수신 후 `JsonConvert`로 역질리트
4. JWT 토큰 자동 착분
5. 실패 시 `401`일 경우 Refresh 시도 → 실패 시 로그인 화면으로 전환

```csharp
ApiClient.Get<T>(
    path: "user/info",
    onSuccess: (res) => { /* 처리 */ },
    onError: (err) => { /* 에러 처리 */ }
);
```

### ✅ 응답 처리 방식

- `HandleResponse<T>()`를 통해 공통 처리
  - 파싱 실패 감지
  - 상태 코드별 분기 처리
  - Refresh 시도 실패 시 `LogoutAndRedirectToLogin()` 호철

---

## 🛙️ 상점 데이터 처리

- **기본 상점 정보** (브롤러, 스키르 등)은 클라이언트 내 JSON 파일로 포함
  - `Resources/Json/shop_items.json` 등에서 로드
  - 포함 정보:
    - 상품 ID
    - 이름 / 설명
    - 이미지 경로
    - 노출 카테고리 ( 스키르, 캐릭터 등 )
    - 기본 가격

- **기간 한정/특가 상품**은 서버에서 API로 발음
  - 클라이언트 진입 시점에 `/shop/limited` API 호출
  - 포함 정보:
    - 상품 ID
    - 구매 가능 여부
    - 적용된 가격 / 자료
    - 남은 시간

- 실제 **구매 처리는 모두 서버 검증 후 처리**

