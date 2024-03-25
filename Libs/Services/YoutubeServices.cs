
using Google.Apis.YouTube.v3;
using Libs.Enity;
using Libs.Enity.UserCustom;
using Microsoft.AspNetCore.Identity;
using SVMusic.Models.Youtube;
using YoutubeExplode.Videos;

namespace Libs.Services
{
    public class YoutubeServices
    {
        public List<UserPlayList> userPlayLists { get; set; } = new List<UserPlayList>();
        public List<YoutubeVideo> youtubeVideos { get; private set; } = new List<YoutubeVideo>();
        public List<YoutubeVideos> Videos { get; private set; } = new List<YoutubeVideos>();
        private ApplicationDbContext applicationDbContext;
        private readonly YouTubeService youTubeService;
        private UserServices userServices;
        private readonly UserManager<IdentityUser> _userManager;

        public YoutubeServices(ApplicationDbContext applicationDbContext, YouTubeService youTubeService, UserServices userServices, UserManager<IdentityUser> _userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.youTubeService = youTubeService;
            this.userServices = userServices;
            this._userManager = _userManager;
        }
        public void Save()
        {
            this.applicationDbContext.SaveChanges();
        }
        public async Task PlayList(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            userPlayLists = applicationDbContext.userPlayLists.Where(x => x.UserId == user.Id).Select(x=>x).ToList();
        }
        public async Task DetailsPlayList(Guid id)
        {
            youtubeVideos = applicationDbContext.youtubePlayLists.Where(x => x.PlayListId == id).ToList();
        }
        public async Task Addvideo(string Id, Guid idPlay)
        {
            var searchRequest = youTubeService.Videos.List("snippet");
            searchRequest.Id = Id;
            var searchResponse = await searchRequest.ExecuteAsync();

            var youTubeVideo = searchResponse.Items.FirstOrDefault();
            if (youTubeVideo != null)
            {
                YoutubeVideo youtubevideo = new YoutubeVideo()
                {
                    VideoId = Id,
                    Title = youTubeVideo.Snippet.Title,
                    Thumbnail = youTubeVideo.Snippet.Thumbnails.High.Url,
                    PlayListId = idPlay, 
                };
                userServices.insertVideoDetails(youtubevideo);
            }

        }
        public async Task OnGet(string key)
        {
            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = key;
            searchListRequest.Type = "video";
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;

            var searchListResponse = await searchListRequest.ExecuteAsync();
            Videos.AddRange(searchListResponse.Items.Select(video => new YoutubeVideos
            {
                Thumbnail = video.Snippet.Thumbnails.High.Url,
                Title = video.Snippet.Title,
                VideoId = video.Id.VideoId,
            }));
        }
        public async Task Recom(string reg)
        {
            var searchListRequest = youTubeService.Videos.List("snippet");
            searchListRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular;
            searchListRequest.VideoCategoryId = "10";
            searchListRequest.RegionCode = reg;

            var searchListResponse = await searchListRequest.ExecuteAsync();
            Videos.AddRange(searchListResponse.Items.Select(video => new YoutubeVideos
            {
                Thumbnail = video.Snippet.Thumbnails.High.Url,
                Title = video.Snippet.Title,
                VideoId = video.Id.Trim(),
            }));
        }
    }
}
