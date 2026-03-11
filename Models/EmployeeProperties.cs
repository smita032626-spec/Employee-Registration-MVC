using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementCRUD.Models
{
    public class EmployeeProperties
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        public DateTime? DOB { get; set; }

        public string Address { get; set; }

        public int CityId { get; set; }

        public int TalukaId { get; set; }

        public int DistrictId { get; set; }

        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

        public string Pincode { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase UploadFile { get; set; }


        // Dropdown Lists
        public List<EmployeeProperties> DepartmentList { get; set; }

        public List<EmployeeProperties> DesignationList { get; set; }

        public List<EmployeeProperties> CityList { get; set; }

        public List<EmployeeProperties> TalukaList { get; set; }

        public List<EmployeeProperties> DistrictList { get; set; }


        // Display Names
        public string DepartmentName { get; set; }

        public string DesignationName { get; set; }

        public string CityName { get; set; }

        public string TalukaName { get; set; }

        public string DistrictName { get; set; }

        public bool IsActive { get; set; }

  
    }
}