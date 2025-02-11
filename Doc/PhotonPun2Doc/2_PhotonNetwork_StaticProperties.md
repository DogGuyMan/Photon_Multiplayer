
## 🔄 PhotonNetwork의 정적 변수들

---
> ### 📄   1. 서버 세팅

#### 1). `ServerSettings PUN.PhotonNetwork.PhotonServerSettings[get]`
#### 2). `PUN.PhotonNetwork.OfflineMode[get, set]`

* 역할 
    :   싱글 플레이어 게임 모드에서 사용할 수 있다.
    :   이걸 Setter함으로 모드를 바꿀 수 있고, 이걸 Getter함으로 모드정보를 얻을 수 있다.
    :   OfflineMode가 되면 어떠한 커넥션도 만들지 않고, 그에 따라 오버헤드도 없다

#### 3). `PUN.PhotonNetwork.AutomaticallySyncScene`

* 역할 
    :   마스터 클라의 Scene(Level)을 자동으로 따라가도록 동기화 하는 세팅 설정
    :   뿐만 아니라 이 값을 true로 세팅하면 MasterClient가 직접 
        PUN.PhotonNetwork.LoadLevel()을 호출 할 수 있다.

#### 4). `PUN.PhotonNetwork.SerializationRate`

* 역할 
    :   `PhotonView`에 의해 호출될 `OnPhotonSerialize()`을 
        단위시간동안 얼마나 많이 호출할지 틱타임을 정한다.
        `LateUpdate()` 로직 내에서 Serialize 된다.
        RPCs 랑은 관계 없는 업데이트다.


#### 5). `PUN.PhotonNetwork.GameVersion`

* 역할
    : 개임 버젼이 일치하는 유저끼리 매칭하도록 함.

---
> ### 📄   2. 네트워크 연결

#### 1). `PUN.PhotonNetwork.IsConnected`

* 역할 
    : 초기값 (포톤 서버에 연결되기전)은 False로,
    OfflineMode가 false인 상태에서 Connect-Call가 들어올때 True로 바뀐다.
    `IsConnectedReady` Static Properties도 한번 확인하는것을 추천한다.

---
> ### 📄   3. 클라이언트 유저 / 플레이어

#### 1). `bool PUN.PhotonNetwork.IsMasterClient`

* 현재 클라이언트가 마스터 클라이언트인지 확인할때 사용
  * 마스터 클라이언트라 함은 특수한 클라이언트이며 
  일부 서버의 기능을 제공하기도 하고, 다른말로 **호스트** 라고 불리기도 한다.
* 만약 자기 자신 컴퓨터의 플레이어 (자신의 클라이언트)라면 true 일 것이다.

#### 2). `string PUN.PhotonNetwork.NickName`

* 단순히 유저의 이름을 정하는 것 뿐만 아니라
* Room에 존재하는 모든 플레이어들에게도 이름을 동기화 시키기 위해서 사용
* 반드시 Unique할 필요는 없다.
* 그리고 PhotonCloud 연결 이전에도 세팅은 가능하다.

#### 3). `Player PUN.PhotonNetwork.LocalPlayer`

* 자기 자신의 컴퓨터의 플레이어 (자신의 클라이언트)에 대한 접근을 위해 사용한다.

#### 4). `Player[] PUN.PhotonNetwork.PlayerListOther`

* 현재 room에 접속한 정렬된 리스트
* join/leave 마다 리스트가 업데이트 된다.
* 그리고 자기 자신의 플레이어(자신의 클라이언트) 제외한 것
* `.Linq`를 사용하여 식별하도록 할 수 있고

#### 5). `int PUN.PhotonNetwork.CountOfPlayersOnMaster`

* "현재 사용가능한 room을 검색중인 플레이어" 들의 개수이다.

#### 6). `int PUN.PhotonNetwork.CountOfPlayers`

* "현재 사용가능한 room"을 의미한다.

---

> ### 📄   4. 로비

#### 1). `TypedLobby PUN.PhotonNetwork.CurrentLobby`

* 룸 생성 또는 참가할때 로비가 정의 된다.
* 클라이언트가 로비에 있는지 없는 지 확인하기 위해서는 `PhotonNework.InLobby`를 사용해야 한다.

#### 2). `bool PUN.PhotonNetwork.InLobby`
* 자기 자신의 플레이어(자신의 클라이언트)가 로비에 있으면 True를 반환한다.
* `IPunCallbacks.OnRoomListUpdate()`에서 룸이 활성화 되거나 업데이트 되었는지를 쓸때 사용할 수 있다.

---
> ### 📄   5. 룸

#### 1). `Room? PUN.PhotonNetwork.CurrentRoom`

* 현재 들어가 있는 방을 리턴한다. 만약, 방에 들어있지 않으면 null을 반환한다.
* 그리고 `PUN.PhotonNetwork.NetworkingClient.CurrentRoom`을 절대 쓰면 안되는것은 
오프라인 모드에서는 항상 null을 반환할 것이다.

#### 2). `PUN.PhotonNetwork.InRoom`

* (NetworkClientState == ClientState.Joined) 함수와 동일하다.
* `IMatchmakingCallbacks`의 구현된 함수에서 `InRoom`이 호출된다는 듯

#### 3). `int PUN.PhotonNetwork.CountOfPlayersOnRooms`

* 현재 모든 방에 플레아어 숫자를 리턴한다.

#### 4). `Room PUN.PhotonNetwork.CountOfRooms`

* 현재 사용중인 방을 리턴한다.

#### 5). `int PUN.PhotonNetwork.LevelLoadingProgress`

* 씬의 로딩 진행 상황을 나타낸다. `LoadLevel()`
* 0 : 하나도 로딩 안됨 / 1 : 로딩이 끝남.

---
> ### 📄   6. Time

#### 1). `double PUN.PhotonNetwork.Time`

* 서버와 동기화 되는 시간이다.
* 이 값은 서버의 Environment.TickCount에 의존한다. 따라서 서버의 퍼포먼스에 따라서 달라진다.
* 그리고 이건 `DateTime`이 아니다.

#### 2). `int PUN.PhotonNetwork.ServerTimestamp`

* 현재 서버의 millisecond 타임 스탬프를 나타낸다.
* 클라이언트의 행동이나, 한 룸에 들어있는 모든 클라이언트들과 동기화 하기 유용한다.
* 마찬가지로 서버의 Environment.TickCount에 의존한다. 
* 그런데 이게 양수 혹은 음수도 될 수 있기 때문에 값의 차이를 구분할때 쓰면 좀 애매하다.

---
> ### 📄   7. Utility

#### 1). `PUN.PhotonNetwork.PrefabPool`

* 게임 오브젝트 풀을 구현한 것이다.
* 이걸 사용하려면 `IPunPrefabPool`이 구현되야 한다.

#### 2). `PUN.PhotonNetwork.IsMessageQueueRunning`

* RPC, Instantiate 등과 같은 이벤트 발생시,
네트워크 오퍼레이션 디스패칭을 잠깐 멈추고 싶을때 사용한다.
* 디스패칭도 안되고, 네트워크도 연결 안되면 `OnPhotonSerializeView`는 어떠한 작업에도 동기화 되지 않을 것이다.
* 따라서 LevelLoading할때나, RPC 등등 네트워크 동기화 이벤트가 렉 걸리면 Queue를 통해 쌓인다.

---
> ### 📄   8. PhotonView

#### 1). [`PUN.PhotonNetwork.PhotonView`](https://doc-api.photonengine.com/en/pun/current/class_photon_1_1_pun_1_1_photon_view.html)

* ##### 설명
  ```
  * 네트워크에 연결해주기 위해서는 PhotonView 컴포넌트가 무조건~ 
    액터 게임 오브젝트에 붙어있어야 한다. 
    그래야 액터에 대한 데이터 공유가 가능하기 때문이다.
  * `PUN.PhotonNetwork.PhotonView`는 네트워크 오브젝트를 식별하며(`viewID`), 
    타 클라이언트에 의해 조종되는 오브젝트의 생명 주기, Update를 설정을한다. `MonoBehaviour`을 상속하기에
  * Discrete Synchronization 세팅을 통해 동기화 촘촘도를 설정한다
  ```
* ##### 용례 
  ```
  1. 동기화 대상 컴포넌트 통함 관리 & 네트워크 오브젝트 상태(위치, 회전)등을 직렬화 해서 동기화 한다.
        동기화 될 네트워크 오브젝트의 상태는 IObservable이 구현되어 있어야 한다.
        대표적으로 `IPunObservable`가 있다 
  2. RPC : 원격 프로시저 호출을 통해 클라이언트 간 이벤트와 함수를 동기화
  3. 객체 식별 및 소유권 관리 
     * 오브젝트를 고유하게 식별하고, 누가 오브젝트를 제어하는지 결정
  4. 네트워크 객체 생명 주기 관리 객체의 생성, 인스턴스화, 소멸을 네트워크 상에서 관리 네트워크 이벤트 콜백 처리
  ```

<details>
    <summary> "ID - FindInstance" : 기법 참고해 보기 </summary>

```cs
public class GlobalManager : Singleton<Manager> {
    private static NonAllocDictionary<int, IdentifiableMonoBehaviour> idObjectList;

    public static bool TryLocalClean(IdentifiableMonoBehaviour idObject) {
        idObject.IsRemovedFromLocalObjectList = true;
        return idObjectList.Remove(idObject.ViewID);
    }

    public static IdentifiableMonoBehaviour GetObject(int viewID) {
        IdentifiableMonoBehaviour result = null;
        idObjectList.TryGetValue(viewID, out resutl);
        return result;
    }

    public static void RegisterObject(IdentifiableMonoBehaviour idObject) {
        if(!Application.isPlaying) {
            idObjectList = new NonAllocDictionary<int, IdentifiableMonoBehaviour>();
            return;
        }

        if(idObject.ViewID != 0) {
            IdentifiableMonoBehaviour pushedObject = null;
            if(idObjectList.TryGetValue(idObject.ViewID, out pushedObject)) {
                return;
            }
            idObjectList.Add(idObject.ViewID, idObject);
            idObject.IsRemovedFromLocalObjectList = false;
        }
    }
}

public class IdentifiableMonoBehaviour : MonoBehaviour {
    
    [NoSerialized]
    private int viewItField = 0;
    
    // 네트워크 게임 오브젝트를 식별하는데, per room 방마다 식별한다.
    // 그리고 이 값은 : "1 : 플레이어 ID", "2 : 씬 ID " 다양한게 된다.
    public int ViewID {
        get {return this.viewIdField;}
        set {
            // 이미 ID가 할당 되었으므로 재 초기화 막자
            if(value != 0 && this.viewIdField != 0) {
                Debug.LogWarning(...); return;
            }
            // ViewID = 0을 선언한 것은 일명 이 GameObject를 삭제하는 것과 같다.
            // OnDestroy() 할때 실행되는 것 과 같다.
            if(value == 0 && this.viewIdField != 0) {
                GlobalManager.TryLocalClean(this); //
            }
            this.viewIdField = value;
            if(value != 0) {GlobalManager.RegisterObject(this);}
        }
    }

    protected internal void Awake() {
        if(this.ViewID != 0) {return;} // 이미 초기화 되었으면 재 초기화 막기
        if(this.sceneViewID != 0) {
            this.ViewID = this.sceneViewID;
        }
    }
}
```
</details>

---

## 의문

* CountOfPlayersOnMaster : 현재 사용가능한 룸을 찾으러 검색중인 플레이어
마스터는 로비를 의미하는 것 인가?

* `PUN.PhotonNetwork.NetworkingClient`이 뭐지
  * `Photon.Realtime.LoadBalancingClient` 이다.
* `Photon.Realtime.LoadBalancingClient` 이 뭐지
* `Photon.Chat.ChatClient`