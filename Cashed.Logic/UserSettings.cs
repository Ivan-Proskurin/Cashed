using Cashed.Logic.Contract;

namespace Cashed.Logic
{
    public class UserSettings : IUserSettings
    {
        public int ItemsPerPage { get; set; } = 5;
    }
}