using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Pinterest_V2
{
     static public class FixedValues
     {
          public static string ordb = "Data source=orcl;User Id=scott; Password=tiger;";


          public static string getCurrentPath()
          {
               string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
               projectPath = projectPath.Substring(0, projectPath.Length - 3);
               projectPath = projectPath.Replace("\\", "\\\\");

               return projectPath;
          }

          public static BitmapImage LoadImage(byte[] imageData)
          {
               if (imageData == null || imageData.Length == 0) return null;
               var image = new BitmapImage();
               using (var mem = new MemoryStream(imageData))
               {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
               }
               image.Freeze();
               return image;
          }
     }
}
