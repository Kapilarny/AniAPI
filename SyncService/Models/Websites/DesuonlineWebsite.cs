﻿using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncService.Models.Websites
{
    public class DesuonlineWebsite : IWebsite
    {
        public DesuonlineWebsite(Website website) : base(website)
        {
        }

        public override bool AnalyzeMatching(Anime anime, AnimeMatching matching, string sourceTitle)
        {
            Console.WriteLine(anime.Titles["en"] + ": " + matching.Title);

            return base.AnalyzeMatching(anime, matching, sourceTitle);
        }

        public override Dictionary<string, string> GetVideoProxyHeaders(AnimeMatching matching, Dictionary<string, string> values = null)
        {
            return null;
        }
    }
}
