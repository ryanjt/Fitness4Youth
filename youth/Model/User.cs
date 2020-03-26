using System;
using System.Collections.Generic;
using SQLite;
using youth.Activities;
using youth.Ach;
using youth.Foo;
using youth.ShopClass;


namespace youth.Users
{
    public class User
    {
       
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Points { get; set; }
        public List<Act1> Acts { get; set; } = new List<Act1>();
        public List<Achievements> Achs { get; set; } = new List<Achievements>();
        public string ProfilePic { get; set; }
        public List<Food1> Foods { get; set; } = new List<Food1>();
        public List<Shop1> Items { get; set; } = new List<Shop1>();


    }
   

    
}
