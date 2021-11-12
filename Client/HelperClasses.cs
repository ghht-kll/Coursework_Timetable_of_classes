using System;

namespace Timetable_of_classes
{
    public class A2Class
    {
        public A2Class(string name, string group)
        {
            this.name = name;
            this.group = group;
        }
        public string name { get; set; }
        public string group { get; set; }
    }
    public class B2Class
    {
        public B2Class(int Id, string name, int quantity)
        {
            this.Id = Id;
            this.name = name;
            this.quantity = quantity;
        }
        public int Id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
    }
    public class C2Class
    {
        public C2Class(int Id, string classroomName, int capacity, string groupName, string fullNameTeacher)
        {
            this.Id = Id;
            this.classroomName = classroomName;
            this.capacity = capacity;
            this.groupName = groupName;
            this.fullNameTeacher = fullNameTeacher;
        }
        public int Id { get; set; }
        public string classroomName { get; set; }
        public int capacity { get; set; }
        public string groupName { get; set; }
        public string fullNameTeacher { get; set; }
    }
    public class History
    {
        public History(int Id, int groupId, string operation, string createAt)
        {
            this.Id = Id;
            this.groupId = groupId;
            this.operation = operation;
            this.createAt = createAt;
        }
        public int Id { get; set; }
        public int groupId { get; set; }
        public string operation { get; set; }
        public string createAt { get; set; }
    }
    public class E2Class
    {
        public E2Class(string name)
        {
            this.name = name;
        }
        public string name { get; set; }
    }
}

