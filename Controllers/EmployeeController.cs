using EmployeeManagementCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagementCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly BALEmployee bal = new BALEmployee();

        #region Employee List
        public ActionResult EmployeeList()
        {
            List<EmployeeProperties> employees = bal.FetchEmployeeList();
            return View(employees);
        }
        #endregion

        #region Create Employee
        // GET: Create Employee
        public ActionResult CreateEmployee()
        {
            EmployeeProperties emp = new EmployeeProperties();
            PopulateDropdowns();
            return View(emp);
        }

        // POST: Insert Employee
        //[HttpPost]
        //public ActionResult CreateEmployee(EmployeeProperties emp)
        //{
        //    try
        //    {
        //        // Save Employee
        //        bal.InsertEmployee(emp);
        //        return RedirectToAction("EmployeeList");
        //    }
        //    catch
        //    {
        //        PopulateDropdowns();
        //        return View(emp);
        //    }
        //}

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeProperties emp, HttpPostedFileBase UploadFile)
        {
            try
            {
                if (UploadFile != null && UploadFile.ContentLength > 0)
                {
                    // Set folder path (e.g., "~/Uploads/")
                    string folder = Server.MapPath("~/Uploads/");

                    // Ensure folder exists
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    // Unique file name to avoid overwrite
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadFile.FileName);
                    string path = Path.Combine(folder, fileName);

                    // Save file on server
                    UploadFile.SaveAs(path);

                    // Save relative path in database (for example)
                    emp.FilePath = "/Uploads/" + fileName;
                }

                // Insert into DB
                bal.InsertEmployee(emp);

                return RedirectToAction("EmployeeList");
            }
            catch
            {
                return View(emp);
            }
        }

        #endregion

        #region Edit Employee
        // GET: Edit Employee
        public ActionResult EditEmployee(int id)
        {
            EmployeeProperties emp = bal.FetchEmployeeById(id);
            PopulateDropdowns(emp);
            return View(emp);
        }

        // POST: Update Employee
        //[HttpPost]
        //public ActionResult EditEmployee(EmployeeProperties emp)
        //{
        //    try
        //    {
        //        bal.UpdateEmployee(emp);
        //        return RedirectToAction("EmployeeList");
        //    }
        //    catch
        //    {
        //        PopulateDropdowns(emp);
        //        return View(emp);
        //    }
        //}
        [HttpPost]
        public ActionResult EditEmployee(EmployeeProperties emp, HttpPostedFileBase UploadFile)
        {
            try
            {
                // Handle new file upload
                if (UploadFile != null && UploadFile.ContentLength > 0)
                {
                    // Save folder path
                    string folder = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    // Unique file name to avoid overwrite
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadFile.FileName);
                    string path = Path.Combine(folder, fileName);

                    // Save file on server
                    UploadFile.SaveAs(path);

                    // Update FilePath to new file
                    emp.FilePath = "/Uploads/" + fileName;
                }
                else
                {
                    // Keep existing file if no new file uploaded
                    EmployeeProperties existingEmp = bal.FetchEmployeeById(emp.Id);
                    emp.FilePath = existingEmp.FilePath;
                }

                // Update employee in DB
                bal.UpdateEmployee(emp);

                return RedirectToAction("EmployeeList");
            }
            catch
            {
                PopulateDropdowns(emp);
                return View(emp);
            }
        }
        #endregion

        #region Delete Employee

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(int id)
        {
            bal.DeleteEmployee(id);
            return RedirectToAction("EmployeeList");
        }
        #endregion

        #region Cascading Dropdowns
        // Get Districts based on City
        public JsonResult GetDistrict(int CityId)
        {
            DataTable dt = bal.FetchDistrict(CityId);
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = row["DistrictName"].ToString(),
                    Value = row["DistrictId"].ToString()
                });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Get Talukas based on District
        public JsonResult GetTaluka(int DistrictId)
        {
            DataTable dt = bal.FetchTaluka(DistrictId);
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = row["TalukaName"].ToString(),
                    Value = row["TalukaId"].ToString()
                });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region Private Methods
        //// Populate all dropdowns for Create/Edit views
        //private void PopulateDropdowns(EmployeeProperties emp = null)
        //{
        //    // Departments
        //    DataTable dtDept = bal.FetchDepartments();
        //    ViewBag.DepartmentList = new SelectList(dtDept.AsDataView(), "DepartmentId", "DepartmentName", emp?.DepartmentId);

        //    // Designations
        //    DataTable dtDesig = bal.FetchDesignation();
        //    ViewBag.DesignationList = new SelectList(dtDesig.AsDataView(), "DesignationId", "DesignationName", emp?.DesignationId);

        //    // Cities
        //    DataTable dtCity = bal.FetchCities();
        //    ViewBag.CityList = new SelectList(dtCity.AsDataView(), "CityId", "CityName", emp?.DistrictId);

        //    // Districts - if editing, prepopulate based on selected City
        //    if (emp != null && emp.DistrictId > 0)
        //    {
        //        DataTable dtDistrict = bal.FetchDistrict(emp.TalukaId); // Will fetch districts by city
        //        ViewBag.DistrictList = new SelectList(dtDistrict.AsDataView(), "DistrictId", "DistrictName", emp.DistrictId);
        //    }
        //    else
        //    {
        //        ViewBag.DistrictList = new SelectList(new List<SelectListItem>(), "Value", "Text");
        //    }

        //    // Talukas - if editing, prepopulate based on selected District
        //    if (emp != null && emp.TalukaId > 0)
        //    {
        //        DataTable dtTaluka = bal.FetchTaluka(emp.DistrictId);
        //        ViewBag.TalukaList = new SelectList(dtTaluka.AsDataView(), "TalukaId", "TalukaName", emp.TalukaId);
        //    }
        //    else
        //    {
        //        ViewBag.TalukaList = new SelectList(new List<SelectListItem>(), "Value", "Text");
        //    }
        //}
        //#endregion
        #region Private Methods
        // Populate all dropdowns for Create/Edit views
        private void PopulateDropdowns(EmployeeProperties emp = null)
        {
            // 1️⃣ Departments
            DataTable dtDept = bal.FetchDepartments();
            ViewBag.DepartmentList = new SelectList(
                dtDept.AsDataView(),
                "DepartmentId",
                "DepartmentName",
                emp?.DepartmentId ?? 0
            );

            // 2️⃣ Designations
            DataTable dtDesig = bal.FetchDesignation();
            ViewBag.DesignationList = new SelectList(
                dtDesig.AsDataView(),
                "DesignationId",
                "DesignationName",
                emp?.DesignationId ?? 0
            );

            // 3️⃣ Cities
            DataTable dtCity = bal.FetchCities();
            ViewBag.CityList = new SelectList(
                dtCity.AsDataView(),
                "CityId",
                "CityName",
                emp?.CityId ?? 0
            );

            // 4️⃣ Districts (based on selected City)
            DataTable dtDistrict;
            if (emp != null && emp.CityId > 0)
            {
                dtDistrict = bal.FetchDistrict(emp.CityId);
                ViewBag.DistrictList = new SelectList(
                    dtDistrict.AsDataView(),
                    "DistrictId",
                    "DistrictName",
                    emp.DistrictId
                );
            }
            else
            {
                dtDistrict = new DataTable(); // empty table
                ViewBag.DistrictList = new SelectList(
                    dtDistrict.AsDataView(),
                    "DistrictId",
                    "DistrictName"
                );
            }

            // 5️⃣ Talukas (based on selected District)
            DataTable dtTaluka;
            if (emp != null && emp.DistrictId > 0)
            {
                dtTaluka = bal.FetchTaluka(emp.DistrictId);
                ViewBag.TalukaList = new SelectList(
                    dtTaluka.AsDataView(),
                    "TalukaId",
                    "TalukaName",
                    emp.TalukaId
                );
            }
            else
            {
                dtTaluka = new DataTable(); // empty table
                ViewBag.TalukaList = new SelectList(
                    dtTaluka.AsDataView(),
                    "TalukaId",
                    "TalukaName"
                );
            }
        }
        #endregion
    }
}