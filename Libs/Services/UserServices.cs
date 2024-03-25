using Libs.Enity;
using Libs.Enity.UserCustom;
using Libs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Services
{
    public class UserServices
    {
        private ApplicationDbContext applicationDbContext;
        private IUserRepositories userRepositories;
        public UserServices(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.userRepositories = new UserRepository(applicationDbContext);
        }
        public void Save()
        {
            this.applicationDbContext.SaveChangesAsync();
        }
        public void insertVideoDetails(YoutubeVideo playList)
        {
            userRepositories.insertVideoDetails(playList);
            Save();
        }
        public void insertPlaylist(UserPlayList userPlayList)
        {
            userRepositories.insertPlaylist(userPlayList);
            Save();
        }
        public void insertUser(User user)
        {
            userRepositories.insertUser(user);
            Save();
        }

    }
}
