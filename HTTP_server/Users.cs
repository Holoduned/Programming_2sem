﻿namespace Http_Server;

[HttpController("users")]
internal class Users
{
    [HttpGET("")]
    public User? GetUser(int id)
    {
        List<User> users = new List<User>();
        users.Add(new User() {Id = 1, name = "Ivan"});

        return users.FirstOrDefault(t => t.Id == id);
    }
}

internal class User
{
    public int Id { get; set; }
    public string? name { get; set; }
}

