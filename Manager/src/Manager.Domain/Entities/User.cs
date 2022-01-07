namespace Manager.Domain.Entities {

    public class User : Base {


        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        // só para o EF
        protected User(){}

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            _erros = new List<string>();
        }

        public void ChangeName(string name){
            Name = name;
            Validate();
        }

        public void ChangePassword(string password) {
            Password = password;
            Validate();
        }

        public void ChangeEmail(string email){
            Email = email;
            Validate();
        }

        public override bool Validate() {
            
        }
        
    }
}