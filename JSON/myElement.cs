using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppXamarin
{
  class Object
  {
    public string section { get; set; }
    public string type { get; set; }
    public string[] value { get; set; }
    public Boolean mandatory { get; set; }
  }
}
