using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Caching
{
    public class ModelCacheEventConsumer
    {
        public const string FileExtension_KEY_BY_ID = "Nop.pres.FileExtension-{0}--";
        public const string PICTURE_KEY_BY_ID = "Nop.pres.picture-{0}-";
        public const string PICTURE_DATA_BY_ID = "Nop.pres.picture--data-{0}-";
        public const string PICTURE_NAME_KEY_BY_ID = "Nop.pres.picture-name-{0}-";
        public const string SETTINGS_KEY = "Nop.pres.picture-setting";
        public const string All_Langauge_KEY = "Nop.pres.language";
        public const string All_List_Langauge_KEY = "Nop.pres.list.language";
        public const string Language_KEY = "Nop.pres.language.key.{0}";
        public const string Current_Customer_Language_KEY = "Nop.pres.current.language.key.{0}";
        public const string Alllocalized_KEY = "Nop.pres.localizedproperty";
        public const string LocalizedProperty_KEY = "Nop.localizedproperty.value-{0}-{1}-{2}-{3}";
        public const string MENU_KEY = "Nop.pres.user.menu-{0}";
        public const string User_Data_KEY = "Nop.pres.user.data-{0}";
        public const string RESOURCE_KEY = "Nop.pres.resource-{0}-";
        

    }
}
