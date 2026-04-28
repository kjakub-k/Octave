using GameJolt.API;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
namespace KJakub.Octave.Managers.GamejoltManager
{
    public class GamejoltLoader : MonoBehaviour
    {
        public int gameID = 1066564;
        async void Start()
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            try
            {
                var result = await CloudCodeService.Instance.CallEndpointAsync<SecretResponse>("GetGamejoltKey", null);

                GameJoltAPI.Instance.Settings.GameId = gameID;
                GameJoltAPI.Instance.Settings.PrivateKey = result.key;

                if (result != null && !string.IsNullOrEmpty(result.key))
                {
                    Debug.Log("Game Jolt credentials set via Direct Access.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Setup Failed: " + e.Message);
            }
        }

        [System.Serializable]
        private class SecretResponse { public string key; }
    }
}