//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CharityManager.Service.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Research
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> ResearchDate { get; set; }
        public int NeedTypeEntityID { get; set; }
        public string Place { get; set; }
        public Nullable<int> Cost { get; set; }
        public string Comment { get; set; }
        public int CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> ModifyUser { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool Active { get; set; }
    
        public virtual Entity Entity { get; set; }
        public virtual Request Request { get; set; }
        public virtual User User { get; set; }
    }
}
