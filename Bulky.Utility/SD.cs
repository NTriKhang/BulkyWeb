using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public static class SD
    {
        public const string RoleUserIndi = "Individual"; 
        public const string RoleUserComp = "Company"; 
        public const string RoleUserAdmin = "Admin"; 
        public const string RoleUserEmp = "Employee";

        public const string StatusPending = "Pending";
        public const string StatusApprove = "Approve";
        public const string StatusInProcess = "Process";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApprove = "Approve";
        public const string PaymentStatusDelayApprove = "DelayApprove";
        public const string PaymentStatusRejected = "Rejected";
	}
}
