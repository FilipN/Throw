﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Throw.Models
{
    public class UserDataAccessLayer
    {

        ThrowSQLContext db = new ThrowSQLContext();
        ErrorLogDataAccessLayer error = new ErrorLogDataAccessLayer();

        public IEnumerable<User> GetUsers ()
        {

            try
            {
                return db.User.ToList();
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        public int? AddUser(User user)
        {
            try
            {
                db.User.Add(user);
                db.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        public int? UpdateUser(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        //Get the details of a particular user    
        public bool? CheckIfUserExists(User reqUser)
        {
            try
            {
                if (db.User.Where(u => u.Email == reqUser.Email && u.Name == reqUser.Name).Any())
                    return true;
                return false;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }


        //Get the details of a particular user    
        public User GetUserData(int id)
        {
            try
            {
                User user = db.User.Find(id);
                return user;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        //To Delete the record of a particular user    
        public int? DeleteUser(int id)
        {
            try
            {
                User user = db.User.Find(id);
                db.User.Remove(user);
                db.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }


    }
}
