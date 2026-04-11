namespace KooliProjekt.WpfApplication
{
    public class Users : NotifyPropertyChangedBase
    {
        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; NotifyPropertyChanged(); }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; NotifyPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; NotifyPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged(); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; NotifyPropertyChanged(); }
        }
    }

    public class SaveUserCommand
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class DeleteUserCommand
    {
        public int UserId { get; set; }
    }
}