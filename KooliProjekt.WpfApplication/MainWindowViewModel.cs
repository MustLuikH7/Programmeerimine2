using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel
    {
        public IList<Users> Data
        {
            get
            {
                var items = new List<Users>
                {
                    new Users { UserId = 1, FirstName = "Test 1" },
                    new Users { UserId = 2, FirstName = "Test 2" },
                    new Users { UserId = 3, FirstName = "Test 3" }
                };

                return items;
            }
        }

        public object SelectedItem { get; set; }
    }
}
