using System.ComponentModel;
using KooliProjekt.WindowsForms.Api;


namespace KooliProjekt.WindowsForms

{

    public partial class Form1 : Form, IMainView

    {

        private readonly IApiClient _apiClient;

        private MainViewPresenter _mainViewPresenter;
 

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public IList<Users> DataSource

        {

            get { return (IList<Users>)dataGridView1.DataSource; }

            set { dataGridView1.DataSource = value; }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public Users SelectedItem
        {
            get => dataGridView1.CurrentRow?.DataBoundItem as Users;
            set
            {
                if (value == null)
                {
                    dataGridView1.ClearSelection();
                    return;
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.DataBoundItem == value)
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public int CurrentId

        {

            get { return int.Parse(idField.Text); }

            set { idField.Text = value.ToString(); }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public string FirstName

        {

            get { return firstNameField.Text; }

            set { firstNameField.Text = value; }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LastName

        {

            get { return lastNameField.Text; }

            set { lastNameField.Text = value; }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public string Email
        {
            get { return emailField.Text; }
            set { emailField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Phone
        {
            get { return phoneField.Text; }
            set { phoneField.Text = value; }
        }   

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FullName { get; set; }

        public void SetPresenter(MainViewPresenter presenter)

        {

            _mainViewPresenter = presenter;

        }

        public Form1(IApiClient apiClient)

        {

            _apiClient = apiClient;

            InitializeComponent();

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

            saveCommand.Click += SaveCommand_Click;

            addCommand.Click += AddCommand_Click;

            deleteCommand.Click += DeleteCommand_Click;

        }

        private async void DeleteCommand_Click(object sender, EventArgs e)

        {

            var message = "Oled kindel, et soovid kustutada " + firstNameField.Text + "?";

            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)

            {

                return;

            }

            var id = int.Parse(idField.Text);

            var result = await _apiClient.Delete(id);

            if (result.HasErrors)

            {

                ShowError("Viga kustutamisel", result);

            }

            await _mainViewPresenter.LoadData();

        }

        private void AddCommand_Click(object sender, EventArgs e)

        {

            idField.Text = "0";

            firstNameField.Text = "";

            lastNameField.Text = "";

            emailField.Text = "";

            phoneField.Text = "";

            FullName = "";

        }

        private async void SaveCommand_Click(object sender, EventArgs e)

        {

            var user = new Users();

            user.UserId = int.Parse(idField.Text);

            user.FirstName = firstNameField.Text;
            user.LastName = lastNameField.Text;
            user.Email = emailField.Text;
            user.Phone = phoneField.Text;

            var result = await _apiClient.Save(user);

            if (result.HasErrors)

            {

                ShowError("Viga salvestamisel", result);

            }

            await _mainViewPresenter.LoadData();

        }

        // Koosta etteantud veateatest ja OperationResult sees olevatest vigadest

        // veateade ja näita seda kasutajale

        public void ShowError(string message, OperationResult result)

        {

            var error = message + "\r\n";

            var apiErrors = "";

            var propertyErrors = "";

            if (result.Errors != null)

            {

                foreach (var apiError in result.Errors)

                {

                    apiErrors += apiError + "\r\n";

                }

            }

            if (result.PropertyErrors != null)

            {

                foreach (var propertyError in result.PropertyErrors)

                {

                    propertyErrors += propertyError.Key + ": " + propertyError.Value;

                }

            }

            if (!string.IsNullOrEmpty(apiErrors))

            {

                error += "\r\n" + apiErrors + "\r\n";

            }

            if (!string.IsNullOrEmpty(propertyErrors))

            {

                error += "\r\n" + propertyErrors;

            }

            error = error.Trim();

            MessageBox.Show(error, "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)

        {

            if (dataGridView1.CurrentRow == null)

            {

                _mainViewPresenter.SetSelection(null);

                return;

            }

            var selectedList = (Users)dataGridView1.CurrentRow.DataBoundItem;

            _mainViewPresenter.SetSelection(selectedList);

        }

        private async void Form1_Load(object sender, EventArgs e)

        {

            await _mainViewPresenter.LoadData();

        }

    }

}
