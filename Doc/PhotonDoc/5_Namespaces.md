---
ebook:
  theme: one-dark.css
  title: Photon
  authors: Escatrgot
  disable-font-rescaling: true
  margin: [0.1, 0.1, 0.1, 0.1]
---
<style>
        h2:not(.tit) { border-top: 12px solid #143666; border-left: 5px solid #143666; border-right: 5px solid #143666; background-color: #143666; color: #FFF !important; font-weight: bold;}

    h3:not(.tit) { border-top: 3px solid #004480; border: 2px solid #004480; background-color: #004480; color: #FFF !important;}


    h4:not(.tit) { font-weight: bold; color: #FFF !important; }

    summary { cursor:pointer; font-weight:bold; color : #0F0 !important;}

    .red{color: #d93d3d;}
    .darkred{color: #470909;}
    .orange{color: #cf6d1d;}
    .yellow{color: #DD3;}
    .green{color: #25ba00;}
    .blue{color: #169ae0;}
    .pink{color: #d10fd1;}
    .dim{color : #666666;}
    .lime{color : #addb40;}
    
    .container {
        display : flex; 
        flex-direction:row;
        align-items:center;
    }
    .item {
        margin-right:2%;
    }

    @media screen and (min-width:1001px){
        .container {
            width: 90%;
            flex-wrap : nowrap;
            justify-content:center;
        }
    }
    
    @media screen and (max-width:1000px){
        .container {
            width: 98%;
            flex-wrap : nowrap;
            justify-content:center;
        }
    }
    
    @media screen and (max-width:799px){
        .container {
            justify-content:left;
            flex-wrap : wrap;
        }
    }

</style>

## 🔄 게임 씬과 룸(레벨)


### 📄 1. Chat

#### 실시간 채팅 기능을 통합하도록 기능을 제공

* #### 1. 인증 및 설정
* #### 2. 채팅 클라이언트 & 채널
* #### 3. 사용자 상태 & 이벤트 수신

---

### 📄 2. Pun

#### 네트워크 오브젝트 관리, 동기화 등등 클라이언트 사이드에 툴을 제공

* ##### 1. 오브젝트 풀링
* ##### 2. 물리, 트랜스폼, 트랜스폼 조작, 애니메이션 에 대한 포톤 "뷰 동기화와 인터페이스" 제공
* ##### 3. SceneManagerHelper
* ##### 4. 네트워크 기능이 확장된 MonoBehaviour 제공 
  * 네트워크 이벤트에 반응하고, 상태를 동기화 하는 스크립트 제공
* ##### 5. 데이터 스트리밍과 직렬화 툴 제공
* ##### 6. 네트워크 / 포톤 전용 Update루프 핸들러와 메시지 처리
* ##### 7. RPC 기능 제공

---

### 📄 3. Realtime

#### 서버 관련 네임 스페이스로 네트워크 초기 연결과 인증, 방 관리, 이벤트 처리 기능 제공

* ##### 1. 설정 및 인증
* ##### 2. 이벤트와 패러미터 관련 툴 제공
* ##### 3. PhotonHandler에서 Update 루프에 처리 가능하도록 콜백 인터페이스 제공
    1. 입장/퇴장
    2. 로비 갱신
    3. 매치메이킹
    4. 서버 및 클라이언트 간 이벤트 처리
    5. WebRPC 
* ##### 4. Room, Player, Friend 관리
* ##### 5. 로드 밸런싱, 네트워크 성능
