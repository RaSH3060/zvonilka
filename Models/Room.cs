using System.Collections.ObjectModel;

namespace Zvonilka.Models
{
    public class Room
    {
        public string Name { get; set; }
        public int MembersCount { get; set; }
        public bool IsPrivate { get; set; }
        public ObservableCollection<User> Members { get; }

        public Room(string name, int membersCount, bool isPrivate = false)
        {
            Name = name;
            MembersCount = membersCount;
            IsPrivate = isPrivate;
            Members = new ObservableCollection<User>();
        }
    }
}