using System;

namespace Zvonilka.Models
{
    public class User
    {
        public string Username { get; set; }
        public string? AvatarPath { get; set; }
        public bool IsMuted { get; set; }
        public bool IsDeafened { get; set; }
        public DateTime JoinTime { get; set; }
        
        public User(string username, string? avatarPath = null)
        {
            Username = username;
            AvatarPath = avatarPath;
            IsMuted = false;
            IsDeafened = false;
            JoinTime = DateTime.Now;
        }
    }
}