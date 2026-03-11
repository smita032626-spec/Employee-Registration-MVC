using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace EmployeeManagementCRUD.Models
{
    public class BALEmployee
    {
        private readonly SqlConnection con = new SqlConnection(
            "Data Source=LAPTOP-N5I5J965;Initial Catalog=EmployeeRegistrationDB;Integrated Security=True;Encrypt=False;");

        #region Employee CRUD

        // Fetch all employees
        public List<EmployeeProperties> FetchEmployeeList()
        {
            List<EmployeeProperties> list = new List<EmployeeProperties>();

            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchEmployeeDetails");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    EmployeeProperties emp = new EmployeeProperties
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        Email = row["Email"].ToString(),
                        PhoneNumber = row["PhoneNumber"].ToString(),
                        DOB = row["DOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DOB"]),
                        Address = row["Address"].ToString(),
                        TalukaId = row["TalukaId"] == DBNull.Value ? 0 : Convert.ToInt32(row["TalukaId"]),
                        DepartmentId = row["DepartmentId"] == DBNull.Value ? 0 : Convert.ToInt32(row["DepartmentId"]),
                        DesignationId = row["DesignationId"] == DBNull.Value ? 0 : Convert.ToInt32(row["DesignationId"]),
                        DepartmentName = row["DepartmentName"]?.ToString(),
                        DesignationName = row["DesignationName"]?.ToString(),
                        CityName = row["CityName"]?.ToString(),
                        DistrictName = row["DistrictName"]?.ToString(),
                        TalukaName = row["TalukaName"]?.ToString(),
                        Pincode = row["Pincode"].ToString(),
                        FilePath = row["FilePath"]?.ToString(),
                        IsActive = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"])
                    };

                    list.Add(emp);
                }
            }

            return list;
        }

        // Fetch single employee by Id
        public EmployeeProperties FetchEmployeeById(int id)
        {
            EmployeeProperties emp = new EmployeeProperties();

            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchEmployeeById");
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    emp.Id = Convert.ToInt32(row["Id"]);
                    emp.Name = row["Name"].ToString();
                    emp.Email = row["Email"].ToString();
                    emp.PhoneNumber = row["PhoneNumber"].ToString();
                    emp.DOB = row["DOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DOB"]);
                    emp.Address = row["Address"].ToString();
                    emp.TalukaId = row["TalukaId"] == DBNull.Value ? 0 : Convert.ToInt32(row["TalukaId"]);
                    emp.DepartmentId = row["DepartmentId"] == DBNull.Value ? 0 : Convert.ToInt32(row["DepartmentId"]);
                    emp.DesignationId = row["DesignationId"] == DBNull.Value ? 0 : Convert.ToInt32(row["DesignationId"]);
                    emp.CityId = row["CityId"] == DBNull.Value ? 0 : Convert.ToInt32(row["CityId"]);
                    emp.DistrictId = row["DistrictId"] == DBNull.Value ? 0 : Convert.ToInt32(row["DistrictId"]);
                    emp.Pincode = row["Pincode"].ToString();
                    emp.FilePath = row["FilePath"]?.ToString();
                    emp.IsActive = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
                }
            }

            return emp;
        }

        // Insert employee
        public int InsertEmployee(EmployeeProperties emp)
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "InsertEmployee");
                cmd.Parameters.AddWithValue("@Name", emp.Name);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
                cmd.Parameters.AddWithValue("@DOB", emp.DOB.HasValue ? (object)emp.DOB.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", emp.Address);
                cmd.Parameters.AddWithValue("@CityId", emp.CityId);
                cmd.Parameters.AddWithValue("@DistrictId", emp.DistrictId);
                cmd.Parameters.AddWithValue("@TalukaId", emp.TalukaId);
                cmd.Parameters.AddWithValue("@DepartmentId", emp.DepartmentId);
                cmd.Parameters.AddWithValue("@DesignationId", emp.DesignationId);
                cmd.Parameters.AddWithValue("@Pincode", emp.Pincode);
                cmd.Parameters.AddWithValue("@FilePath", string.IsNullOrEmpty(emp.FilePath) ? (object)DBNull.Value : emp.FilePath);
                cmd.Parameters.AddWithValue("@IsActive", emp.IsActive);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();

                return result;
            }
        }

        // Update employee
        public int UpdateEmployee(EmployeeProperties emp)
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UpdateEmployee");
                cmd.Parameters.AddWithValue("@Id", emp.Id);
                cmd.Parameters.AddWithValue("@Name", emp.Name);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
                cmd.Parameters.AddWithValue("@DOB", emp.DOB.HasValue ? (object)emp.DOB.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", emp.Address);
                cmd.Parameters.AddWithValue("@CityId", emp.CityId);
                cmd.Parameters.AddWithValue("@DistrictId", emp.DistrictId);
                cmd.Parameters.AddWithValue("@TalukaId", emp.TalukaId);
                cmd.Parameters.AddWithValue("@DepartmentId", emp.DepartmentId);
                cmd.Parameters.AddWithValue("@DesignationId", emp.DesignationId);
                cmd.Parameters.AddWithValue("@Pincode", emp.Pincode);
                cmd.Parameters.AddWithValue("@FilePath", string.IsNullOrEmpty(emp.FilePath) ? (object)DBNull.Value : emp.FilePath);
                cmd.Parameters.AddWithValue("@IsActive", emp.IsActive);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();

                return result;
            }
        }

        // Delete employee
        public int DeleteEmployee(int id)
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DeleteEmployee");
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();

                return result;
            }
        }

        #endregion

        #region Dropdown Fetch Methods

        public DataTable FetchDepartments()
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchDepartments");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable FetchDesignation()
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchDesignation");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable FetchCities()
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchCities");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable FetchDistrict(int CityId)
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchDistrictByCityId");
                cmd.Parameters.AddWithValue("@CityId", CityId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable FetchTaluka(int DistrictId)
        {
            using (SqlCommand cmd = new SqlCommand("SPEmployeeManagement", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "FetchTalukaByDistrictId");
                cmd.Parameters.AddWithValue("@DistrictId", DistrictId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        #endregion
    }
}