using System;

namespace Zvonilka.Models
{
    public class Message
    {
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string? SenderAvatarPath { get; set; }
        public bool IsPrivate { get; set; }
        
        public Message(string senderName, string content, string? senderAvatarPath = null, bool isPrivate = false)
        {
            SenderName = senderName;
            Content = content;
            SenderAvatarPath = senderAvatarPath;
            Timestamp = DateTime.Now;
            IsPrivate = isPrivate;
        }
    }
}