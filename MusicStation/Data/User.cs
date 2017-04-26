using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicStation.Data;
using MusicStation.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class User : IdentityUser
{
    public virtual ICollection<Song> Songs { get; set; }
    
    public User()
    {
        this.Songs = new HashSet<Song>();
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
    {
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

        return userIdentity;
    }
}