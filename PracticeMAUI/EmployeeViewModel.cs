using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PracticeMAUI
{
    public class EmployeeViewModel: INotifyPropertyChanged
    {
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string employeeId;
        private string employeeName;
        private DateTime joinDate;
        private decimal salary;
        private bool isActive;
        private string imageUrl;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string EmployeeId
        {
            get => employeeId;
            set
            {
                employeeId = value;
                NotifyPropertyChanged();
            }
        }
        public string EmployeeName
        {
            get => employeeName;
            set
            {
                employeeName = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime JoinDate
        {
            get => joinDate;
            set
            {
                joinDate = value;
                NotifyPropertyChanged();
            }
        }
        public decimal Salary
        {
            get => salary;
            set
            {
                salary = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                NotifyPropertyChanged();
            }
        }
        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                imageUrl = value;
                NotifyPropertyChanged();
            }
        }
    }
}

