using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_project.ViewModels
{
    public class AdressSelectViewModel
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}