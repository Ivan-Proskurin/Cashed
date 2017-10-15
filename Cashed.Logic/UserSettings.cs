using Logic.Cashed.Contract;

namespace Logic.Cashed.Logic
{
    public class UserSettings : IUserSettings
    {
        public int ItemsPerPage { get; set; } = 5;
    }
}