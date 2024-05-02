using PracticeMAUI.Model;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PracticeMAUI;

public partial class EmployeeDetailPage : ContentPage
{
    private EmployeeListViewModel employees;
    FileUpload obj = new();
    FileUpload imageUpload { get; set; }
    private string SelectedImageBase64 { get; set; }
    public EmployeeDetailPage()
	{
		InitializeComponent();
	}
    public EmployeeDetailPage(EmployeeListViewModel employees, EmployeeViewModel employee)
    {
        InitializeComponent();
        this.employees = employees;
        BindingContext = employee;
        Title = "Edit Employee";
        btnAddEmployee.IsVisible = false;
        btnUpdateEmployee.IsVisible = true;

        (BindingContext as EmployeeViewModel)?.NotifyPropertyChanged("ImageUrl");

        imageUpload = new FileUpload();

    }
    public EmployeeDetailPage(EmployeeListViewModel employees)
    {
        InitializeComponent();
        this.employees = employees;
        BindingContext = new EmployeeViewModel();
        Title = "Add Employee";
    }
    private async void btnUpload_Clicked(object sender, EventArgs e)
    {
        var img = await MediaPicker.PickPhotoAsync();
        var imageFile = await obj.Upload(img);
        SelectedImageBase64 = imageFile?.ByteBase64;
        Upload_Image.Source = ImageSource.FromStream(() =>
        obj.ByteArrayToStream(obj.StringToByteBase64(imageFile.ByteBase64))

        );
    }

    private async void btnAddEmployee_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is EmployeeViewModel employeeViewModel)
            {
                JsonSerializerOptions _serializerOptions;
                _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                employeeViewModel.ImageUrl = SelectedImageBase64;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5156" : "http://localhost:5156");

                string json = System.Text.Json.JsonSerializer.Serialize<EmployeeViewModel>(employeeViewModel, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders
                 .Accept
                 .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync("api/employees", content);




                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Employee saved successfully.", "OK");
                    await Navigation.PopAsync(animated: true);
                }
                else
                {
                    await DisplayAlert("Error", "Failed to save employee.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void btnUpdateEmployee_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is EmployeeViewModel employeeViewModel)
            {
                JsonSerializerOptions _serializerOptions;
                _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                employeeViewModel.ImageUrl = SelectedImageBase64;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5156" : "http://localhost:5156");


                string json = System.Text.Json.JsonSerializer.Serialize<EmployeeViewModel>(employeeViewModel, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders
                 .Accept
                 .Add(new MediaTypeWithQualityHeaderValue("application/json"));



                HttpResponseMessage response = await client.PutAsync($"api/employees/{employeeViewModel.EmployeeId}", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Employee updated successfully.", "OK");
                    await Navigation.PopAsync(animated: true);
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update employee.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}