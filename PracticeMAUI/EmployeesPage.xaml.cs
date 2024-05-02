using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PracticeMAUI;

public partial class EmployeesPage : ContentPage
{
	public EmployeesPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        EmployeeListViewModel viewModel = new();
        viewModel.Clear();
        LoadEmployees();
    }

    private void LoadEmployees()
    {
        EmployeeListViewModel viewModel = new();
        try
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5156" : "http://localhost:5156")
            };
            infoLabel.Text = $"BaseAddress:{client.BaseAddress}";
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/employees").Result;
            response.EnsureSuccessStatusCode();

            IEnumerable<EmployeeViewModel> employeesFromService = response.Content.ReadFromJsonAsync<IEnumerable<EmployeeViewModel>>().Result;
            foreach (EmployeeViewModel e in employeesFromService.OrderBy(emp => emp.EmployeeName))
            {
                viewModel.Add(e);
            }
            infoLabel.Text += $"\n{viewModel.Count} Employees Loaded";
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message + "\n Using sample data.";
            ErrorLabel.IsVisible = true;
        }
        BindingContext = viewModel;
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EmployeeDetailPage(BindingContext as EmployeeListViewModel));
    }

    private async void Employee_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is not EmployeeViewModel c) return;
        await Navigation.PushAsync(new EmployeeDetailPage(BindingContext as EmployeeListViewModel, c));
    }

    private async void Employee_Refreshing(object sender, EventArgs e)
    {
        if (sender is not ListView listView) return;
        listView.IsRefreshing = true;
        await Task.Delay(1500);
        listView.IsRefreshing = false;
    }

    private async void EmployeeMenuItem_Clicked(object sender, EventArgs e)
    {
        MenuItem menuItem = sender as MenuItem;
        if (menuItem.BindingContext is not EmployeeViewModel emp) return;
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5156" : "http://localhost:5156");
            HttpResponseMessage response = await client.DeleteAsync($"api/employees/{emp.EmployeeId}");
            if (response.IsSuccessStatusCode)
            {
                (BindingContext as EmployeeListViewModel)?.Remove(emp);
            }
            else
            {
                await DisplayAlert("Error", "Falied to delete", "OK");
            }
        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", $"Error Occured: {ex.Message}", "OK");
        }
    }
}