using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using DemoWebApi.ViewModel;
using System;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        db1045Context db = new db1045Context();

        [HttpGet]
        [Route("ListDept")]
        public IActionResult GetDept()
        {
            //var data = db.Depts.ToList();
            //var data = from dept in db.Depts select dept;
            var data = from dept in db.Depts select new { Id = dept.Id, Name = dept.Name, Location = dept.Location };
            return Ok(data);
        }

        [HttpGet]
        [Route("ListDept/{id}")]
        public IActionResult GetDept(int id)
        {
            if(id == null)
            {
                return BadRequest("Id Cannot Be NULL");
            }
            //var data = db.Depts.Find(id);
            //var data = db.Depts.Where(d => d.Id == id).Select(d => new { Id = d.Id, Name = d.Name, Location = d.Location }).FirstOrDefault();
            var data = (from dept in db.Depts where dept.Id == id select new { Id = dept.Id, Name = dept.Name, Location = dept.Location }).FirstOrDefault();
            if (data == null)
                return NotFound($"Department {id} not present");
            return Ok(data);
        }

        [HttpGet]
        [Route("ListCity")]
        public IActionResult GetCity([FromQuery] string city)
        {
            var data = (from dept in db.Depts where dept.Location == city select new { Id = dept.Id, Name = dept.Name, Location = dept.Location });
            if (data.Count() == 0)
                return NotFound($"No Location found in {city}");
            return Ok(data);
        }

        [HttpGet]
        [Route("ShowDept")]
        public IActionResult GetDeptInfo()
        {
            var data = db.DeptInfo_VMs.FromSqlInterpolated<DeptInfo_VM>($"DeptInfo");
            return Ok(data);
        }

        [HttpPost]
        [Route("AddDept")]
        public IActionResult PostDept(Dept dept)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //db.Depts.Add(dept);
                    //db.SaveChanges();
                    //call stored procedure to add record
                    db.Database.ExecuteSqlInterpolated($"deptadd {dept.Id}, {dept.Name}, {dept.Location}");
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.InnerException.Message);
                }
            }
            return Created("Record Successfully Added", dept);
        }

        [HttpPut]
        [Route("EditDept/{id}")]
        public IActionResult PutDept(int id, Dept dept)
        {
            if(ModelState.IsValid)
            {
                Dept odept = db.Depts.Find(id);
                odept.Name = dept.Name;
                odept.Location = dept.Location;
                db.SaveChanges();
                return Ok();
            }
            return BadRequest("Unable to Edit Record");
        }

        [HttpDelete]
        [Route("DeleteDept/{id}")]
        public IActionResult DeleteDept(int id)
        {
            var data = db.Depts.Find(id);
            db.Depts.Remove(data);
            db.SaveChanges();
            return Ok();
        }
    }
}
