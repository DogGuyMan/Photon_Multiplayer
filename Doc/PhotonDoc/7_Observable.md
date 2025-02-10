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


---

## 🔄 네트워크 오브젝트 상태 동기화

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