
## 🔄 네트워크 오브젝트 상태 동기화

---
 
#### 1). `Pun.IPunObservable.OnPhotonSerializeView()`

* `OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)` 함수를 구현함으로 
직렬화가 가능한 클래스로 만들어 주도록 한다.
* 이 직렬화는 PUN에 의해 단위시간당 몇번정도로만 호출된다.

```cs
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool IsFiring;
    public float Health;
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion
```

---
 
#### 2). `Pun.PhotonStream`

* `IPunObservable.OnPhotonSerializeView()` 내부에서 사용하는 **컨테이너** 임.
* 들어오는 데이터를 Receive하고, 나가는 데이터를 Send하는 기능을 가진다.

#### - Constructor
* ##### `PhotonStream(bool write, object[] incomingData)`
  * 스트림 객체를 생성하고 초기화 한다.

#### - Method

* ##### a. `object ReceiveNext()` 
  * IsReading일때 `PhotonStream stream`에 데이터를 하나씩, 하나씩 인덱스를 증가 시켜 **읽는다.**
* ##### b. `void SendNext(object obj)`
  * IsWriting일 때, `PhotonStream stream`에 데이터를 하나씩 인덱스를 증가 시켜 **넣는다.**
* ##### c. `object[] ToArray`
  * stream 데이터를 `object[]` 배열로 캐스팅 한다.
* ##### d. `Serialize(ref <Type>)` 
  * `PhotonStream stream`에 데이터를 타입에 맞게 Read, Write를 플래그에 따라 수행한다
  * 모든 원시타입이 가능하고 `Vector2` `Vector3` `Quaternion`
  * 심지어 참조타입 `Player` `string`도 가능하다

#### - Properties
* ##### a.  `IsWriting` : 이게 참이면 Send
* ##### b.  `IsReading` : 이게 참이면 Receive
* ##### c.  `Count` : 스트림의 아이템 수로 `foreach`를 돌릴 수 있음


---
 
#### 3). `Pun.PhotonStreamQueue`

* PhotonStreamQueue는 
PhotonNetwork.SendRate에서 정한 빈도보다 더 높은 주기로 오브젝트 상태를 폴링하도록 도와주며,
* Serialize()가 호출되면 이 상태들을 한 번에 전송합니다. 
* 수신 측에서는 Deserialize()를 호출하면, 스트림이 기록된 순서와 시간 간격에 맞춰 
* 받은 오브젝트 상태들을 순차적으로 출력합니다.
