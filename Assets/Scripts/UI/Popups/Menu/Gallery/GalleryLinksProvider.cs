using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace UI.Popups.Menu.Gallery
{
    public class GalleryLinksProvider
    {
        private const string ImagesGalleryLink = "https://data.ikppbb.com/test-task-unity-data/pics/";
        private const string ImageCellLink = "https://data.ikppbb.com/";

        public IEnumerator GetLinks(System.Action<List<string>> callback)
        {
            using var req = UnityWebRequest.Get(ImagesGalleryLink);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
                yield break;
            }

            var html = req.downloadHandler.text;

            var links = ParseLinks(html);
            var sortedLinks = SortLinks(links);

            callback?.Invoke(sortedLinks);
        }

        private List<string> ParseLinks(string html)
        {
            var list = new List<string>();

            var matches = Regex.Matches(html,
                @"href\s*=\s*[""']([^""']+\.jpg)[""']",
                RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                var file = match.Groups[1].Value;

                if (!file.StartsWith("http"))
                    file = ImageCellLink + file;

                list.Add(file);
            }

            return list;
        }

        private List<string> SortLinks(List<string> links)
        {
            return links.OrderBy(ParseIndex).ToList();
        }

        private int ParseIndex(string link)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(link);

            int.TryParse(fileName, out var index);

            return index;
        }
    }
}