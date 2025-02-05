using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Com.MyCompany.MyGame.Common;
using Cysharp.Threading.Tasks;

namespace Com.MyCompany.MyGame
{
    public class PlayerDataModel : SingletonDontDestroyMonobeheviour<PlayerDataModel>
    {
        public PlayerInfo PlayerInfo;

        public async UniTask LoadPlayerData(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string jsonString = await reader.ReadToEndAsync();
                PlayerInfo playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(jsonString);
                PlayerInfo = playerInfo;
            }
        }

        public async UniTask SavePlayerData(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string jsonString = JsonConvert.SerializeObject(PlayerInfo, Formatting.Indented);
                await writer.WriteAsync(jsonString);
            }
        }

        public async void OnDisable()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "playerinfo.json");
            await SavePlayerData(filePath);
        }

        public async void OnDestroy()
        {
            OnDisable();
        }
    }
}