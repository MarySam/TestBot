using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MasterReloBot
{
    public class LuisCommute
    {
        //The Luis URI for Commute.
        public static async Task<LuisInfo> ParseUserInput(string strInput)
        {
            string strRet = string.Empty;
            string strEscaped = Uri.EscapeDataString(strInput);

            using (var client = new HttpClient())
            {
                string uri = "https://api.projectoxford.ai/luis/v1/application?id=2fb62e84-da20-4205-8dad-d6206e533681&subscription-key=a0794768387a459da34bab6f49878c1e&q=" + strEscaped;
                HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    string jsonResponse = await msg.Content.ReadAsStringAsync();
                    LuisInfo _Data = JsonConvert.DeserializeObject<LuisInfo>(jsonResponse);
                    return _Data;
                }
            }
            return null;
        }
    }
}