using System.Text.Json.Serialization;

namespace Manager.Services.DTO
{
    public class UserDTO {

        public long Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        /* 
            when serializing the objects, passwords will be 
            ignored and thus not be displayed to the user
        */
        [JsonIgnore]
        public string Password { get; set; }

        public UserDTO()
        { }

        public UserDTO(long id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}