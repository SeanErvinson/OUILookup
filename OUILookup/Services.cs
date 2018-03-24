using System;
using System.IO;
using OUILookup.Data.Models;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace OUILookup
{
    public class Services
    {
        public static string Location { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "vendorlist.json");
        private static List<OUI> OUIs = new List<OUI>();
        public const string SourceURL = @"https://code.wireshark.org/review/gitweb?p=wireshark.git;a=blob_plain;f=manuf;hb=HEAD";

        public static async void Update()
        {
            using (var client = new WebClient())
            {
                var stream = client.OpenRead(SourceURL);
                using (var reader = new StreamReader(stream))
                {
                    string line = string.Empty;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if(!ValidateString(line) && !string.IsNullOrEmpty(line))
                        {
                            OUI oui;
                            var ouiInfo = line.Split('\t');
                            if (ouiInfo.Length == 2) { oui = new OUI(ouiInfo[0], ouiInfo[1]); }
                            else { oui = new OUI(ouiInfo[0], ouiInfo[1], ouiInfo[2]); }

                            OUIs.Add(oui);
                        }
                    }
                }
            }
            SerializeJson();
        }

        private static bool ValidateString(string query)
        {
            string pattern = "^[\"#\\s\\r\\n]+";
            return Regex.IsMatch(query ?? "", pattern);
        }

        private static void SerializeJson()
        {
            var OUIJsonData = JsonConvert.SerializeObject(OUIs);
            using (StreamWriter file = File.CreateText(Location))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, OUIs);
            }
        }

        public static async Task<List<OUI>> RetrieveOUIs()
        {
            string jsonData = string.Empty;
            using (var reader = new StreamReader(Location))
            {
                jsonData = await reader.ReadToEndAsync();
            }
            OUIs = JsonConvert.DeserializeObject<List<OUI>>(jsonData);
            return OUIs;
        }
    }
}
