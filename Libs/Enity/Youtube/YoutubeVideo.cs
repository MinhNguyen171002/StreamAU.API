using Libs.Enity.UserCustom;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Enity
{
    public class YoutubeVideo
    {
        [Key]
        public string? VideoId { get; internal set; }
        public string? Title { get; internal set; }
        public string? Thumbnail { get; internal set; }
        public Guid PlayListId { get; set; }
        [ForeignKey(nameof(PlayListId))]
        public UserPlayList PlayList { get; set;}
        
    }
}
