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

## 🔄 PhotonNetwork의 정적 변수들


---

### 📄 1. 서버 세팅

#### 1). `ServerSettings PhotonNetwork.PhotonServerSettings[get]`
#### 2). `PhotonNetwork.OfflineMode[get, set]`

* 역할 
    :   싱글 플레이어 게임 모드에서 사용할 수 있다.
    :   이걸 Setter함으로 모드를 바꿀 수 있고, 이걸 Getter함으로 모드정보를 얻을 수 있다.
    :   이게 활성화 되면 어떠한 커넥션도 만들지 않고, 그에따라 오버헤드도 없다

#### 3). `PhotonNetwork.AutomaticallySyncScene`

* 역할 
    :   마스터 클라의 Scene(Level)을 자동으로 따라가도록 동기화 하는 세팅 설정
    :   뿐만 아니라 이 값을 true로 세팅하면 MasterClient가 직접 
        PhotonNetwork.LoadLevel()을 호출 할 수 있다.

#### 4). `PhotonNetwork.SerializationRate`

* 역할 
    :   `PhotonView`에 의해 호출될 `OnPhotonSerialize()`을 단위시간동안 얼마나 많이 호출할지 틱타임을 정한다.
        `LateUpdate()` 로직 내에서 Serialize 된다.
        RPCs 랑은 관계 없는 업데이트다.


#### 5). `PhotonNetwork.GameVersion`

* 역할
    : 개임 버젼이 일치하는 유저끼리 매칭하도록 함.

---

### 📄 2. 네트워크 연결

#### 1). `PhotonNetwork.IsConnected`

* 역할 
    : 초기값 (포톤 서버에 연결되기전)은 False로,
    OfflineMode가 false인 상태에서 Connect-Call가 들어올떄 True로 바뀐다.
    `IsConnectedReady` Static Properties도 한번 확인하는것을 추천한다.

---

### 📄 3. 클라이언트 유저 / 플레이어

#### 1). `bool PhotonNetwork.IsMasterClient`

* 현재 클라이언트가 마스터 클라이언트인지 확인할때 사용
* 만약 자기 자신 컴퓨터의 플레이어 (자신의 클라이언트)라면 true 일 것이다.

#### 2). `string PhotonNetwork.NickName`

* 단순히 유저의 이름을 정하는 것 뿐만 아니라
* Room에 존재하는 모든 플레이어들에게도 이름을 동기화 시키기 위해서 사용
* 반드시 Unique할 필요는 없다.
* 그리고 PhothonClound 연결 이전에도 세팅은 가능하다.

#### 3). `Player PhotonNetwork.LocalPlayer`

* 자기 자신의 컴퓨터의 플레이어 (자신의 클라이언트)에 대한 접근을 위해 사용한다.

#### 4). `Player[] PhotonNetwork.PlayerListOther`

* 현재 room에 접속한 정렬된 리스트
* join/leave 마다 리스트가 업데이트 된다.
* 그리고 자기 자신의 플레이어(자신의 클라이언트) 제외한 것
* `.Linq`를 사용하여 식별하도록 할 수 있고

#### 5). `int PhotonNetwork.CountOfPlayersOnMaster`

* "현재 사용가능한 room을 검색중인 플레이어" 들의 개수이다.

#### 6). `int PhotonNetwork.CountOfPlayers`

* "현재 사용가능한 room"을 의미한다.

---

### 📄 4. 룸

#### 1). `Room? PhotonNetwork.CurrentRoom`

* 현재 들어가 있는 방을 리턴한다. 만약, 방에 들어있지 않으면 null을 반환한다.
* 그리고 `PhotonNetwork.NetworkingClient.CurrentRoom`을 절대 쓰면 안되는것은 
오프라인 모드에서는 항상 null을 반환할 것이다.

#### 2). `PhotonNetwork.InRoom`

* (NetworkClientState == ClientState.Joined) 함수와 동일하다.
* `IMatchmakingCallbacks`의 구현된 함수에서 `InRoom`이 호출된다는 듯

#### 3). `int PhotonNetwork.CountOfPlayersOnRooms`

* 현재 모든 방에 플레아어 숫자를 리턴한다.

#### 4). `Room PhotonNetwork.CountOfRooms`

* 현재 사용중인 방을 리턴한다.

#### 5). `int PhotonNetwork.LevelLoadingProgress`

* 씬의 로딩 진행 상황을 나타낸다. `LoadLevel()`
* 0 : 하나도 로딩 안됨 / 1 : 로딩이 끝남.

---

### 📄 5. 로비

#### 1). `TypedLobby PhotonNetwork.CurrentLobby`

* 룸 생성 또는 참가할때 로비가 정의 된다.
* 클라이언트가 로비에 있는지 없는 지 확인하기 위해서는 `PhotonNework.InLobby`를 사용해야 한다.

#### 2). `bool PhotonNetwork.InLobby`
* 자기 자신의 플레이어(자신의 클라이언트)가 로비에 있으면 True를 반환한다.
* `IPunCallbacks.OnRoomListUpdate()`에서 룸이 활성화 되거나 업데이트 되었는지를 쓸떄 사용할 수 있다.

---

### 📄 6. Time

#### 1). `double PhotonNetwork.Time`

* 서버와 동기화 되는 시간이다.
* 이 값은 서버의 Environment.TickCount에 의존한다. 따라서 서버의 퍼포먼스에 따라서 달라진다.
* 그리고 이건 `DateTime`이 아니다.

#### 2). `int PhotonNetwork.ServerTimestamp`

* 현재 서버의 millisecond 타임 스탬프를 나타낸다.
* 클라이언트의 행동이나, 한 룸에 들어있는 모든 클라이언트들과 동기화 하기 유용한다.
* 마찬가지로 서버의 Environment.TickCount에 의존한다. 
* 그런데 이게 양수 혹은 음수도 될 수 있기 떄문에 값의 차이를 구분할때 쓰면 좀 애매하다.

--- 

### 📄 7. Utility

#### 1). `PhotonNetwork.PrefabPool`

* 게임 오브젝트 풀을 구현한 것이다.
* 이걸 사용하려면 `IPunPrefabPool`이 구현되야 한다.

#### 2). `PhotonNetwork.IsMessageQueueRunning`

* RPC, Instantiate 등과 같은 이벤트 발생시,
네트워크 오퍼레이션 디스패칭을 잠깐 멈추고 싶을때 사용한다.
* 디스패칭도 안되고, 네트워크도 연결 안되면 `OnPhotonSerializeView`는 어떠한 작업에도 동기화 되지 않을 것이다.
* 따라서 LevelLoading할때나, RPC 등등 네트워크 동기화 이벤트가 렉 걸리면 Queue를 통해 쌓인다.

---

## 의문

* CountOfPlayersOnMaster : 현재 사용가능한 룸을 찾으러 검색중인 플레이어
마스터는 로비를 의미하는 것 인가?

* `PhotonNetwork.NetworkingClient`이 뭐지
  * `Photon.Realtime.LoadBalancingClient` 이다.
* `Photon.Realtime.LoadBalancingClient` 이 뭐지
* `Photon.Chat.ChatClient`