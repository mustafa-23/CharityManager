using CharityManager.UI.Models;

namespace CharityManager.UI
{
    static class GlobalVar
    {
        public static UserModel User { get; private set; }

        public static void SetUser(UserModel user) => User = user;
    }
}
