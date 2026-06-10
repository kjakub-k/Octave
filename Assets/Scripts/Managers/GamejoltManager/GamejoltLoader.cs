using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;
namespace KJakub.Octave.Managers.GamejoltManager
{
    public class GamejoltLoader : MonoBehaviour
    {
        private static GamejoltLoader instance;
        public static GamejoltLoader Instance { get { return instance; } }
        async void Awake()
        {
            instance = this;
        }
        async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        public async Task<string> GetUserData(string key)
        {
            var result = await CallCloudBridge(new Dictionary<string, object> {
            { "action", "getData" },
            { "username", PlayerPrefs.GetString("username") },
            { "user_token", PlayerPrefs.GetString("token") },
            { "key", key }
            });

            try
            {
                var json = JObject.Parse(result);

                if (json["response"]?["success"]?.ToString() == "true")
                {
                    return json["response"]["data"].ToString();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return null;
        }
        public async Task<bool> Login(string user, string token)
        {
            var result = await CallCloudBridge(new Dictionary<string, object> {
                { "action", "auth" },
                { "username", user },
                { "user_token", token }
            });

            try
            {
                var json = JObject.Parse(result);

                if (json["response"]?["success"]?.ToString() == "true")
                {
                    PlayerPrefs.SetString("username", user);
                    PlayerPrefs.SetString("token", token);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return false;
        }
        public async void AwardTrophy(string trophyId)
        {
            await CallCloudBridge(new Dictionary<string, object> {
            { "action", "awardTrophy" },
            { "username", PlayerPrefs.GetString("username") },
            { "user_token", PlayerPrefs.GetString("token") },
            { "trophy_id", trophyId }
        });
        }
        public async void SavePerformance(float score, float accuracy)
        {
            await CallCloudBridge(new Dictionary<string, object> {
            { "action", "setData" },
            { "username", PlayerPrefs.GetString("username") },
            { "user_token", PlayerPrefs.GetString("token") },
            { "key", "score" },
            { "value", score }
        });
            await CallCloudBridge(new Dictionary<string, object> {
            { "action", "setData" },
            { "username", PlayerPrefs.GetString("username") },
            { "user_token", PlayerPrefs.GetString("token") },
            { "key", "accuracy" },
            { "value", accuracy }
        });
        }
        private async Task<string> CallCloudBridge(Dictionary<string, object> args)
        {
            try
            {
                return await CloudCodeService.Instance.CallEndpointAsync("Gamejolt", args);
            }
            catch (CloudCodeException e)
            {
                Debug.LogError(e.Message);
                return "false";
            }
        }
    }
}