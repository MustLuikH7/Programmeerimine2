using System;
using System.Windows.Forms;
using System.Threading.Tasks;
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
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            var response = await _apiClient.List(1, 100);
            if (response != null && response.Value != null)
            {
                dataGridView1.DataSource = response.Value.Results;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga andmete laadimisel: " + ex.Message);
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        // Simple Add capability
        var command = new SaveUserCommand 
        { 
            FirstName = "Test",
            LastName = "User " + DateTime.Now.Ticks.ToString().Substring(10),
            Email = "test" + DateTime.Now.Ticks.ToString().Substring(10) + "@example.com",
            Phone = "123456789",
            PasswordHash = "hashedpassword"
        };
        
        try 
        {
            await _apiClient.Save(command);
            await LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga salvestamisel: " + ex.Message);
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count == 0)
        {
            MessageBox.Show("Palun vali rida kustutamiseks.");
            return;
        }
        
        var selectedItem = dataGridView1.SelectedRows[0].DataBoundItem as UserDto;
        if (selectedItem != null)
        {
            var confirmResult = MessageBox.Show($"Kas soovid kindlasti kustutada {selectedItem.FullName}?", "Kustuta", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try 
                {
                    await _apiClient.Delete(selectedItem.UserId);
                    await LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Viga kustutamisel: " + ex.Message);
                }
            }
        }
    }
}
