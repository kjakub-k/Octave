using Discord;
using UnityEngine;
using UnityEngine.SocialPlatforms;
namespace KJakub.Octave.Managers.DiscordManager
{
    public class DiscordController : MonoBehaviour
    {
        private Discord.Discord discord;
        private long clientId = 1503420000562839784;
        private void Start()
        {
            discord = new Discord.Discord(clientId, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
            UpdateActivity();
        }

        private void Update()
        {
            discord.RunCallbacks();
        }

        public void UpdateActivity()
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Assets = {
                    LargeImage = "icon",
                    LargeText = "Octave"
                },
                Timestamps = {
                    Start = System.DateTimeOffset.Now.ToUnixTimeSeconds()
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                    Debug.Log("Discord rich presence updated!");
            });
        }

        private void OnApplicationQuit()
        {
            discord.Dispose();
        }
    }
}
