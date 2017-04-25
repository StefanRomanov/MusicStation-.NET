using Microsoft.Owin;
using MusicStation.Migrations;
using MusicStation.Models;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(MusicStation.Startup))]
namespace MusicStation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<MusicStationDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
