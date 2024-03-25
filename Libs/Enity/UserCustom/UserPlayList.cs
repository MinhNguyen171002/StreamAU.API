using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Libs.Enity.UserCustom
{
    public class UserPlayList
    {
        [Key]
        public Guid PlayListId { get; set; }
        public string PlayListName { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User user { get; set; }
        public virtual ICollection<YoutubeVideo> videos { get; set; }
    }
}
