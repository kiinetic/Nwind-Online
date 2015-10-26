using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class Category
    {
        #region Fields

        private int categoryID;
        private string categoryName;
        //private string filename;

        #endregion

        #region Properties

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        #endregion

        public Category(int id, string name)
        {
            categoryID = id;
            categoryName = name;
        }

    }
}
