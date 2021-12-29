using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProcessingLibrary
{
 public static class Validator
 {

  public static bool ValidateImportOrder(BigShoeDataImportOrder order, out List<string> errors)
  {
   errors = new List<string>();
   if (!ValidateCustomerName(order.CustomerName))
   {
    errors.Add("Customer name is missing.");
   }
   if (!ValidateCustomerEmail(order.CustomerEmail))
   {
    errors.Add($"Customer email ({order.CustomerEmail}) is not in corect format.");
   }

   if (!ValidateDate(order.DateRequired))
   {
    errors.Add($"Date ({order.DateRequired}) is not valid.");
   }

   if(!ValidateQuantity(order.Quantity))
   {
    errors.Add($"Quantity ({order.Quantity}) is not valid.");
   }

   if (!ValidateSize(order.Size))
   {
    errors.Add($"Size ({order.Size}) is not valid.");
   }
   if(errors.Count > 0)
   {
    errors.Insert(0, "Some entries have the following errors: ");
   }
   return errors.Count == 0;
  }

  public static bool ValidateCustomerName(string name)
  {
   //Customer Name must be provided
   return !string.IsNullOrEmpty(name);
  }

  public static bool ValidateCustomerEmail(string email)
  {
   //Customer Email must be a valid email address
   try
   {
    if (string.IsNullOrEmpty(email)) return false;
    MailAddress m = new MailAddress(email);
    return true;
   }
   catch (FormatException)
   {
    return false;
   }
  }

  public static bool ValidateDate(DateTime date)
  {
   //Date must be valid and at least 10 working days into the future

   //check total days
   if (date <= DateTime.UtcNow.AddDays(10)) return false;

   //check working days:
   return GetWorkingDaysCount(DateTime.UtcNow, date) >= 10;
  }

  public static int GetWorkingDaysCount(this DateTime current, DateTime finishDate)
  {
   try
   {
    return Enumerable.Range(0, (finishDate - current).Days).Select(a => current.AddDays(a)).Count(a => a.DayOfWeek != DayOfWeek.Sunday || a.DayOfWeek != DayOfWeek.Saturday);
   }
   catch
   {
    return 0;
   }
  }

  public static bool ValidateSize(float size)
  {
   //Size must be 11.5 to 15 including half sizes
   List<float> ValidSizes = new List<float> { 11.5f, 12f, 12.5f, 13f, 13.5f, 14f, 14.5f, 15f };
   return ValidSizes.Contains(size);
  }

  public static bool ValidateQuantity(int quantity)
  {
   //Quantity must be in multiples of 1000
   return quantity > 0 && quantity % 1000 == 0;
  }
 }
}
