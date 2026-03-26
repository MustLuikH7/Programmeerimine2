using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms;

public partial class Form1 : Form
{
    private readonly IApiClient _apiClient;

    public Form1() 
    {
        InitializeComponent();
    }

    public Form1(IApiClient apiClient) : this()
    {
        _apiClient = apiClient;

        dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        saveCommand.Click += SaveCommand_Click;
        addCommand.Click += AddCommand_Click;
        
        // Kui sul on disaineris olemas deleteCommand nupp, siis seo see siin. 
        // Vastasel juhul lisa nupp Form1.Designer.cs failis ja eemalda siit kommentaar.
        // deleteCommand.Click += DeleteCommand_Click; 
    }

    private void AddCommand_Click(object sender, EventArgs e)
    {
        idField.Text = "0";
        firstNameField.Text = "";
        lastNameField.Text = "";
        emailField.Text = "";
        phoneField.Text = "";
    }

    private async void SaveCommand_Click(object sender, EventArgs e)
    {
        // VIGA OLI SIIN: "new User()" asemel tuleb kasutada uut DTO mudelit "SaveUserCommand", 
        // nagu on defineeritud ApiClient.cs sees
        var command = new SaveUserCommand()
        {
            UserId = int.TryParse(idField.Text, out int id) ? id : 0,
            FirstName = firstNameField.Text,
            LastName = lastNameField.Text,
            Email = emailField.Text,
            Phone = phoneField.Text,
            PasswordHash = "dummy-password" 
        };

        var result = await _apiClient.Save(command);
        
        if (result.HasErrors)
        {
            ShowError("Viga salvestamisel", result);
        }
        else 
        {
            MessageBox.Show("Andmed salvestatud edukalt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        await LoadUsers();
    }

    private async void DeleteCommand_Click(object sender, EventArgs e)
    {
        var message = "Oled kindel, et soovid kustutada " + firstNameField.Text + " " + lastNameField.Text + "?";
        var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes)
        {
            return;
        }

        var id = int.TryParse(idField.Text, out int resultId) ? resultId : 0;
        var result = await _apiClient.Delete(id);
        
        if (result.HasErrors)
        {
            ShowError("Viga kustutamisel", result);
        }

        await LoadUsers();
    }

    private void DataGridView1_SelectionChanged(object sender, EventArgs e)
    {
        if(dataGridView1.CurrentRow == null)
        {
            return;
        }

        var selectedUser = (UserDto)dataGridView1.CurrentRow.DataBoundItem;
        if(selectedUser == null)
        {
            return;
        }

        idField.Text = selectedUser.UserId.ToString();
        firstNameField.Text = selectedUser.FirstName;
        lastNameField.Text = selectedUser.LastName;
        emailField.Text = selectedUser.Email;
        phoneField.Text = selectedUser.Phone;
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        var response = await _apiClient.List(1, 100);
        
        if(response.HasErrors)
        {
            ShowError("Viga andmete laadimisel", response);
            dataGridView1.DataSource = null;
            return;
        }

        dataGridView1.DataSource = response.Value.Results;
    }

    private void ShowError(string message, OperationResult result)
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
            foreach(var propertyError in result.PropertyErrors)
            {
                propertyErrors += propertyError.Key + ": " + propertyError.Value + "\r\n";
            }
        }

        if(!string.IsNullOrEmpty(apiErrors))
        {
            error += "\r\n" + apiErrors + "\r\n";
        }

        if(!string.IsNullOrEmpty(propertyErrors))
        {
            error += "\r\n" + propertyErrors;
        }

        error = error.Trim();

        MessageBox.Show(error, "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
