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

### 📄 1. PUN 콜백

#### 1). `IConnectionCallbacks`

* ##### a. `IConnectionCallbacks.OnConnected()`
  * 설명 
    :   서버에 접속하기 아직 전, 로컬에서 수행할 함수
        이 함수가 호출된 이후에는 Auth 체크를 한다.
        그리고 `OnConnectedToMaster()` 함수가 실행되야지 호출되는 순서다.
  * 용례
    :   서버에 접근이 가능한지 아닌지를 체크할떄 사용한다.
        하지만 일반적으로 `OnDisconnected()`에서 체크하는게 대부분이다.
* ##### b. ⭐️ `IConnectionCallbacks.OnConnectedToMaster()` ⭐️ 
  * 설명 *(이거 궁금했다!)*
    : 클라이언트가 Master Server에 접속 되었을때, +
    그리고 매치메이킹 준비가 되었을떄 호출된다.
    그리고 이 마스터 서버에 접속함에 따라 JoinLobby가 발동되지 않는다면 
    룸 자체도 다른 클라이언트도 못쓴다 
    : 여기서 MasterServer는 Photon.Server의 (Master/Game Server)의 그 서버를 말한다.
* ##### c. `IConnectionCallbacks.OnDisconnected()`
  * 설명 
    : 포톤 서버에 연결이 끊어지면 발생하는 콜백
    혹은 아예 서버에 접속을 못하면 발생하기도 한다.
    그 원인은 `Enum.DisconnectCause`에 들어있다.

---

#### 2). `IMatchmakingCallbacks`

* ##### a. `IMatchmakingCallbacks.OnCreatedRoom()`
  * 설명 
    : 클라이언트가 룸을 생성하고 + 룸에 참가할때 호출된다.
    단, 룸을 생성한 클라이언트에게만 발생하는 콜백이다 "OpCreateRoom"
* ##### b. `IMatchmakingCallbacks.OnJoinedRoom()`
  * 설명 
    : `LoadBalancingClient`가 방에 참가했을때 발생하는 콜백,
    이 콜백 호출 중, 이후로 부터 Room.Players 에 접근할 수 있고, "플레이어 GameObject"(프리펩 같은) 생성할 수 있다.

* ##### c. `IMatchmakingCallbacks.OnLeftRoom()`
  * 설명 
    : `PhotonNetwork.LeaveRoom()`를 사용하고 
    로컬의 user/client가 방을 떠났을때, 애플리케이션이 종료될때, 발생할 것이다.
    LoadBalancingClient는 GameServer와 연결이 끊어질 것이고, 대신 Master Server에 연결될 것이다.
    순서는 `OnConnectedToMaster`이 호출되야 그다음에 수행될 것이다.

* ##### d. `IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)`
  * 설명 
    : 서버가 방을 만들어지지 못했을때, 발생하는 에러
* ##### e. `IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)`
  * 설명 
    : OnJoinRoom호출 이후로 GameServer에 참가하지 못했을때, 발생
    Master Server에게 실패 콜백을 전달한다.

---

#### 3). [`IInRoomCallbacks`](https://doc-api.photonengine.com/en/pun/current/interface_photon_1_1_realtime_1_1_i_in_room_callbacks.html)

* ##### a. `IInRoomCallbacks.OnPlayerEnteredRoom(Player player)`
* ##### b. `IInRoomCallbacks.OnPlayerLeftRoom(Player player)`
* ##### b. `IInRoomCallbacks.OnMasterClientSwitched(Player player)`
  * 설명
    : 마스터 클라이언트가 현재 방에 나갔을때, 호출되는 콜백이다.