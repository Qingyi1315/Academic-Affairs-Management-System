namespace 教务管理系统
{
    public static class GlobalVariables
    {
        // 存储当前登录的用户信息
        public static string CurrentUserNumber { get; set; }

        public static string CurrentUserName { get; set; }

        public static string CurrentRole { get; set; }

        public static string AdminStatusTeacherNumber { get; set; }

        public static void Clear()
        {
            CurrentUserNumber = string.Empty;
            CurrentRole = string.Empty;
            AdminStatusTeacherNumber = string.Empty;
        }
    }
}
