using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace bamsak
{
    interface IQuickSnapPlugin
    {
        string getUrl();
        void upload(CroppedBitmap image);
    }
}
