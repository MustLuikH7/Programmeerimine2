using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KooliProjekt.WindowsForms;

public partial class Form1 : Form
{
    private readonly HttpClient _httpClient;

    public Form1()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            // API port on localhost based on WebAPI launchSettings.json
            var response = await _httpClient.GetFromJsonAsync<OperationResult<PagedResult<DoctorDto>>>("http://localhost:5086/api/Doctors/List");
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
}

public class DoctorDto
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Specialty { get; set; }
    public string FullName { get; set; }
}

public class PagedResult<T>
{
    public IList<T> Results { get; set; }
}

public class OperationResult<T>
{
    public T Value { get; set; }
    public bool HasErrors { get; set; }
}
