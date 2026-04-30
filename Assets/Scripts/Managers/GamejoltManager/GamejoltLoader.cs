using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json.Linq;
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
            await UnityServices.InitializeAsync();
        }
        async void Start()
        {
            instance = this;

            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    Debug.Log("Signed into Unity Services Anonymously");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Unity Services Init Failed: {e.Message}");
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
                Debug.LogError("Data Store Error: " + e.Message);
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
                    Debug.Log("GameJolt Login Successful!");
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to parse JSON: " + e.Message);
            }

            return false;
        }

        public async void SaveAchievementDisplay(string achievementId, int slot)
        {
            await CallCloudBridge(new Dictionary<string, object> {
            { "action", "setData" },
            { "username", PlayerPrefs.GetString("username") },
            { "user_token", PlayerPrefs.GetString("token") },
            { "key", $"achievement_slot_{slot}" },
            { "value", achievementId }
            });
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
                Debug.LogError($"Cloud Code Failed: {e.Message}");
                return "false";
            }
        }
    }
}