using Google.Apis.YouTube.v3;
using Libs.Enity;
using Libs.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoAnStreamingMusic.Controllers.API
{
    [Route("API/[controller]")]
    [ApiController]
    public class YoutubeAPI : Controller
    {
        private YoutubeServices services;

        public YoutubeAPI(YoutubeServices services)
        {
            
            this.services = services;
        }
        [HttpGet]
        [Route("search")]
        public IActionResult Search(string key)
        {
            services.OnGet(key).Wait();
            return Ok(new { services.Videos });
        }
        [HttpGet]
        [Route("list")]
        public IActionResult List(string username) 
        {
            services.PlayList(username).Wait();
            return Ok(new { services.userPlayLists });
        }
        [HttpGet]
        [Route("recommend")]
        public IActionResult Recommend(string reg)
        {
            services.Recom(reg).Wait();
            return Ok(new { services.Videos });
        }
        [HttpGet]
        [Route("details")]
        public IActionResult detailsList(Guid id)
        {
            services.DetailsPlayList(id).Wait();
            return Ok(new { services.youtubeVideos });
        }
    }
}
