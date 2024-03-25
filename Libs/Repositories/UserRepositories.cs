using Libs.Data;
using Libs.Enity;
using Libs.Enity.UserCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Repositories
{
    public interface IUserRepositories : IRepository<User>
    {
        public void insertUser(User user);
        public void insertVideoDetails(YoutubeVideo playList);
        public void insertPlaylist(UserPlayList userPlayList);
    }
    public class UserRepository : RepositoryBase<User>, IUserRepositories
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public void insertUser(User userCustom)
        {
            _dbContext.userCustoms.AddAsync(userCustom);
        }
        public void insertVideoDetails(YoutubeVideo playList)
        {
            _dbContext.youtubePlayLists.AddAsync(playList);
        }
        public void insertPlaylist(UserPlayList userPlayList)
        {
            _dbContext.userPlayLists.AddAsync(userPlayList);
        }

    }
}
