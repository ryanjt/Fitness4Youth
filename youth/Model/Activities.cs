using System;
using SQLite;
namespace youth.Activities
{
    public class Act1
    {
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();
        public string Name
        {
            get;
            set;
        }
        public string Duration
        {
            get;
            set;
        }
        public DateTime DateUtc
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }
        public string Color
        {
            get;
            set;
        }

        public Act1()
        {
        }
    }
}
