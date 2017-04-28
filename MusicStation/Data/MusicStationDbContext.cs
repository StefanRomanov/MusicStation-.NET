using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicStation.Models;
using MusicStation.Data;

namespace MusicStation.Data
{

    public class MusicStationDbContext : IdentityDbContext<User>
    {
        public MusicStationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Song> Songs { get; set; }

        public static MusicStationDbContext Create()
        {
            return new MusicStationDbContext();
        }
    }
}